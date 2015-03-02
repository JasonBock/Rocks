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
	internal sealed class Builder
	{
		internal Builder(Type baseType,
			ReadOnlyDictionary<string, HandlerInformation> handlers,
			SortedSet<string> namespaces, Options options)
		{
			this.TypeName = $"Rock{Guid.NewGuid().ToString("N")}";
         this.BaseType = baseType;
			this.Handlers = handlers;
			this.Namespaces = namespaces;
			this.Options = options;
			this.Tree = this.MakeTree();
		}

		private List<string> GetGeneratedConstructors()
		{
			var generatedConstructors = new List<string>();

			foreach (var constructor in this.BaseType.GetConstructors(Constants.Reflection.PublicInstance))
			{
				var parameters = constructor.GetParameters();

				if (parameters.Length > 0)
				{
					generatedConstructors.Add(string.Format(Constants.CodeTemplates.ConstructorTemplate,
						this.TypeName, constructor.GetArgumentNameList(), constructor.GetParameters(this.Namespaces)));
				}
			}

			return generatedConstructors;
		}

		private List<string> GetGeneratedMethods()
		{
			var generatedMethods = new List<string>();

			foreach (var baseMethod in this.BaseType.GetMethods(Constants.Reflection.PublicInstance)
				.Where(_ => !_.IsSpecialName && _.IsVirtual))
			{
				var methodDescription = baseMethod.GetMethodDescription(this.Namespaces);
				var containsRefAndOrOutParameters = baseMethod.ContainsRefAndOrOutParameters();

				if (!containsRefAndOrOutParameters || this.Handlers.ContainsKey(methodDescription))
				{
					var delegateCast = !containsRefAndOrOutParameters ?
						baseMethod.GetDelegateCast() :
						this.Handlers[methodDescription].Method.GetType().GetSafeName(baseMethod, this.Namespaces);
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

		private SyntaxTree MakeTree()
		{
			var methods = this.GetGeneratedMethods();
			var constructors = this.GetGeneratedConstructors();
			var properties = this.BaseType.GetImplementedProperties(this.Namespaces);
			var events = this.BaseType.GetImplementedEvents(this.Namespaces);

			this.Namespaces.Add(this.BaseType.Namespace);
			this.Namespaces.Add(typeof(ExpectationException).Namespace);
			this.Namespaces.Add(typeof(IRock).Namespace);
			this.Namespaces.Add(typeof(HandlerInformation).Namespace);
			this.Namespaces.Add(typeof(string).Namespace);
			this.Namespaces.Add(typeof(ReadOnlyDictionary<,>).Namespace);

			var @class = string.Format(Constants.CodeTemplates.ClassTemplate,
				string.Join(Environment.NewLine,
					(from @namespace in this.Namespaces
					 select $"using {@namespace};")),
				this.TypeName, this.BaseType.GetSafeName(),
				string.Join(Environment.NewLine, methods),
				properties, events, string.Join(Environment.NewLine, constructors));

			SyntaxTree tree = null;

			if(this.Options.ShouldCreateCodeFile)
			{
				var fileName = Path.Combine(Directory.GetCurrentDirectory(), $"{this.TypeName}.cs");
				tree = SyntaxFactory.SyntaxTree(
					SyntaxFactory.ParseSyntaxTree(@class)
						.GetCompilationUnitRoot().NormalizeWhitespace(),
					path: fileName, encoding: new UTF8Encoding(false, true));
				File.WriteAllText(fileName, tree.GetText().ToString());
			}
			else
			{
				tree = SyntaxFactory.ParseSyntaxTree(@class);
			}

			return tree;
		}

		internal SyntaxTree Tree { get; private set; }
		internal Options Options { get; private set; }
		internal Type BaseType { get; private set; }
		internal ReadOnlyDictionary<string, HandlerInformation> Handlers { get; private set; }
		internal SortedSet<string> Namespaces { get; private set; }
		internal string TypeName { get; private set; }
	}
}
