using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocks.Exceptions;
using Rocks.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rocks
{
	internal sealed class Maker
	{
		private string mangledName = $"Rock{Guid.NewGuid().ToString("N")}";
		private Type baseType;
		private ReadOnlyDictionary<string, HandlerInformation> handlers;
		private SortedSet<string> namespaces;
		private Options options;

		internal Type Mock { get; private set; }

		internal Maker(Type baseType,
			ReadOnlyDictionary<string, HandlerInformation> handlers,
			SortedSet<string> namespaces, Options options)
		{
			this.baseType = baseType;
			this.handlers = handlers;
			this.namespaces = new SortedSet<string>(namespaces);
			this.options = options;

			if (baseType.IsInterface)
			{
				this.Mock = this.MakeInterfaceMock();
			}
		}

		private Type MakeInterfaceMock()
		{
			var generatedMethods = new List<string>();

			foreach (var baseMethod in this.baseType.GetMethods().Where(_ => !_.IsSpecialName))
			{
				var methodDescription = baseMethod.GetMethodDescription(namespaces);
				var containsRefAndOrOutParameters = baseMethod.ContainsRefAndOrOutParameters();

            if (!containsRefAndOrOutParameters || this.handlers.ContainsKey(methodDescription))
				{
					var delegateCast = !containsRefAndOrOutParameters ?
						baseMethod.GetDelegateCast() : this.handlers[methodDescription].Method.GetType().GetSafeName();
					var argumentNameList = baseMethod.GetArgumentNameList();
					var expectationChecks = !containsRefAndOrOutParameters ? baseMethod.GetExpectationChecks() : string.Empty;
					var outInitializers = !containsRefAndOrOutParameters ? string.Empty : baseMethod.GetOutInitializers();

               if (baseMethod.ReturnType != typeof(void))
					{
						generatedMethods.Add(string.Format(baseMethod.ReturnType.IsValueType ||
							(baseMethod.ReturnType.IsGenericParameter && (baseMethod.ReturnType.GenericParameterAttributes & GenericParameterAttributes.ReferenceTypeConstraint) == 0) ?
								Constants.CodeTemplates.FunctionWithValueTypeReturnValueMethodTemplate :
								Constants.CodeTemplates.FunctionWithReferenceTypeReturnValueMethodTemplate,
							methodDescription, argumentNameList, baseMethod.ReturnType.GetSafeName(), expectationChecks, delegateCast, outInitializers));
					}
					else
					{
						generatedMethods.Add(string.Format(Constants.CodeTemplates.ActionMethodTemplate,
							methodDescription, argumentNameList, expectationChecks, delegateCast, outInitializers));
					}
				}
				else
				{
					generatedMethods.Add(string.Format(Constants.CodeTemplates.RefOutNotImplementedMethodTemplate, methodDescription));
				}
			}

			var properties = this.baseType.GetImplementedProperties(namespaces);
			var events = this.baseType.GetImplementedEvents(namespaces);

         this.namespaces.Add(baseType.Namespace);
			this.namespaces.Add(typeof(ExpectationException).Namespace);
			this.namespaces.Add(typeof(IRock).Namespace);
			this.namespaces.Add(typeof(HandlerInformation).Namespace);
			this.namespaces.Add(typeof(string).Namespace);
			this.namespaces.Add(typeof(ReadOnlyDictionary<,>).Namespace);

			var classCode = string.Format(Constants.CodeTemplates.ClassTemplate,
				string.Join(Environment.NewLine,
					(from @namespace in this.namespaces
					 select $"using {@namespace};")),
				this.mangledName, this.baseType.GetSafeName(),
				string.Join(Environment.NewLine, generatedMethods), 
				properties, events);

			var compilation = this.CreateCompilation(classCode);
			var assembly = this.CreateAssembly(compilation);
			return assembly.GetType(this.mangledName);
		}

		private Assembly CreateAssembly(CSharpCompilation compilation)
		{
			using (MemoryStream assemblyStream = new MemoryStream(),
				pdbStream = new MemoryStream())
			{
				var results = compilation.Emit(assemblyStream,
					pdbStream: pdbStream);

				if (!results.Success)
				{
					throw new CompilationException(results.Diagnostics);
				}

				return Assembly.Load(assemblyStream.GetBuffer(), pdbStream.GetBuffer());
			}
		}

		private CSharpCompilation CreateCompilation(string classCode)
		{
			var fileName = this.options.ShouldCreateCodeFile ?
				Path.Combine(Directory.GetCurrentDirectory(), $"{this.mangledName}.cs") : string.Empty;

			var tree = SyntaxFactory.SyntaxTree(
				SyntaxFactory.ParseSyntaxTree(classCode)
					.GetCompilationUnitRoot().NormalizeWhitespace(), 
				path: fileName, encoding: new UTF8Encoding(false, true));

			if (this.options.ShouldCreateCodeFile)
			{
				File.WriteAllText(fileName, tree.GetText().ToString());
			}

			var compilation = CSharpCompilation.Create(
				"RockQuarry",
				options: new CSharpCompilationOptions(
					OutputKind.DynamicallyLinkedLibrary,
					optimizationLevel: this.options.Level),
				syntaxTrees: new[] { tree },
				references: new[]
				{
					MetadataReference.CreateFromAssembly(typeof(object).Assembly),
					MetadataReference.CreateFromAssembly(typeof(IRock).Assembly),
					MetadataReference.CreateFromAssembly(this.baseType.Assembly)
				});

			return compilation;
		}
	}
}