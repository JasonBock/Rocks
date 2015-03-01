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
			this.Mock = this.MakeType();
		}

		private List<string> GetGeneratedConstructors()
		{
			var generatedConstructors = new List<string>();

			foreach(var constructor in this.baseType.GetConstructors(Constants.Reflection.PublicInstance))
			{
				var parameters = constructor.GetParameters();

				if(parameters.Length > 0)
				{
					generatedConstructors.Add(string.Format(Constants.CodeTemplates.ConstructorTemplate,
						this.mangledName, constructor.GetArgumentNameList(), constructor.GetParameters(this.namespaces)));
				}
			}

			return generatedConstructors;
		}

		private List<string> GetGeneratedMethods()
		{
			var generatedMethods = new List<string>();

			foreach (var baseMethod in this.baseType.GetMethods(Constants.Reflection.PublicInstance)
				.Where(_ => !_.IsSpecialName && _.IsVirtual))
			{
				var methodDescription = baseMethod.GetMethodDescription(namespaces);
				var containsRefAndOrOutParameters = baseMethod.ContainsRefAndOrOutParameters();

				if (!containsRefAndOrOutParameters || this.handlers.ContainsKey(methodDescription))
				{
					var delegateCast = !containsRefAndOrOutParameters ?
						baseMethod.GetDelegateCast() : 
						this.handlers[methodDescription].Method.GetType().GetSafeName(baseMethod, namespaces);
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

			return generatedMethods;
		}

		private Type MakeType()
		{
			var generatedMethods = this.GetGeneratedMethods();
			var generatedConstructors = this.GetGeneratedConstructors();
			var properties = this.baseType.GetImplementedProperties(this.namespaces);
			var events = this.baseType.GetImplementedEvents(this.namespaces);

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
				properties, events, string.Join(Environment.NewLine, generatedConstructors));

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

			var compilation = CSharpCompilation.Create("RockQuarry",
				options: new CSharpCompilationOptions(
					OutputKind.DynamicallyLinkedLibrary,
					optimizationLevel: this.options.Level),
				syntaxTrees: new[] { tree },
				references: new[]
				{
					MetadataReference.CreateFromAssembly(typeof(object).Assembly),
					MetadataReference.CreateFromAssembly(typeof(IRock).Assembly),
					MetadataReference.CreateFromAssembly(typeof(Action<,,,,,,,,>).Assembly),
               MetadataReference.CreateFromAssembly(this.baseType.Assembly)
				});

			return compilation;
		}
	}
}