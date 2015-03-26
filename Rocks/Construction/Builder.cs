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

namespace Rocks.Construction
{
	internal abstract class Builder
	{
		internal Builder(Type baseType,
			ReadOnlyDictionary<string, HandlerInformation> handlers,
			SortedSet<string> namespaces, bool shouldCreateCodeFile)
		{
         this.BaseType = baseType;
			this.Handlers = handlers;
			this.Namespaces = namespaces;
			this.ShouldCreateCodeFile = shouldCreateCodeFile;
		}

		internal virtual void Build()
		{
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

				// Either the base method contains no refs/outs, or the user specified a delegate
				// to use to handle that method (remember, types with methods with refs/outs are gen'd
				// each time, and that's the only reason the handlers are passed in.
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

		private List<string> GetGeneratedProperties()
		{
			var generatedProperties = new List<string>();

			foreach (var baseProperty in this.BaseType.GetProperties(Constants.Reflection.PublicInstance)
				.Where(_ => (_.CanRead ? _.GetMethod : _.SetMethod).IsVirtual))
			{
				var propertyImplementations = new List<string>();

				if(baseProperty.CanRead)
				{
					var getMethod = baseProperty.GetMethod;
					var getMethodDescription = getMethod.GetMethodDescription(this.Namespaces);
					var getArgumentNameList = getMethod.GetArgumentNameList();
					var getExpectationChecks = getMethod.GetExpectationChecks();
               var getDelegateCast = getMethod.GetDelegateCast();
					propertyImplementations.Add(string.Format(getMethod.ReturnType.IsValueType ?
						Constants.CodeTemplates.PropertyGetWithValueTypeReturnValueTemplate : 
						Constants.CodeTemplates.PropertyGetWithReferenceTypeReturnValueTemplate,
						getMethodDescription, getArgumentNameList, getMethod.ReturnType.GetSafeName(), getExpectationChecks, getDelegateCast));
				}

				if (baseProperty.CanWrite)
				{
					var setMethod = baseProperty.SetMethod;
					var setMethodDescription = setMethod.GetMethodDescription(this.Namespaces);
					var setArgumentNameList = setMethod.GetArgumentNameList();
					var setExpectationChecks = setMethod.GetExpectationChecks();
					var setDelegateCast = setMethod.GetDelegateCast();
					propertyImplementations.Add(string.Format(Constants.CodeTemplates.PropertySetTemplate,
						setMethodDescription, setArgumentNameList, setExpectationChecks, setDelegateCast));
				}

				// Generate the property template, based on indexes or not.
				this.Namespaces.Add(baseProperty.PropertyType.Namespace);
				var indexers = baseProperty.GetIndexParameters();

				if (indexers.Length > 0)
				{
					var parameters = string.Join(", ",
						from indexer in indexers
						let _ = this.Namespaces.Add(indexer.ParameterType.Namespace)
						select $"{indexer.ParameterType.Name} {indexer.Name}");

					// Indexer
					generatedProperties.Add(string.Format(Constants.CodeTemplates.PropertyIndexerTemplate,
						baseProperty.PropertyType.Name, parameters, string.Join(Environment.NewLine, propertyImplementations)));
				}
				else
				{
					// Normal
					generatedProperties.Add(string.Format(Constants.CodeTemplates.PropertyTemplate,
						baseProperty.PropertyType.GetSafeName(), baseProperty.Name,
						string.Join(Environment.NewLine, propertyImplementations)));
				}
			}

			return generatedProperties;
		}

		private string MakeCode()
		{
			var methods = this.GetGeneratedMethods();
			var constructors = this.GetGeneratedConstructors();
			var properties = this.GetGeneratedProperties(); 
			var events = this.BaseType.GetImplementedEvents(this.Namespaces);

			this.Namespaces.Add(this.BaseType.Namespace);
			this.Namespaces.Add(typeof(ExpectationException).Namespace);
			this.Namespaces.Add(typeof(IRock).Namespace);
			this.Namespaces.Add(typeof(HandlerInformation).Namespace);
			this.Namespaces.Add(typeof(string).Namespace);
			this.Namespaces.Add(typeof(ReadOnlyDictionary<,>).Namespace);

			return string.Format(Constants.CodeTemplates.ClassTemplate,
				string.Join(Environment.NewLine,
					(from @namespace in this.Namespaces
					 select $"using {@namespace};")),
				this.TypeName, this.BaseType.GetSafeName(),
				string.Join(Environment.NewLine, methods),
				string.Join(Environment.NewLine, properties), events, 
				string.Join(Environment.NewLine, constructors),
				this.BaseType.Namespace);
		}

		private SyntaxTree MakeTree()
		{
			var @class = this.MakeCode();
			SyntaxTree tree = null;

			if (this.ShouldCreateCodeFile)
			{
				var fileName = Path.Combine(this.GetDirectoryForFile(), $"{this.TypeName}.cs");
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

		protected abstract string GetDirectoryForFile();

		internal bool ShouldCreateCodeFile { get; private set; }
      internal SyntaxTree Tree { get; private set; }
		internal Type BaseType { get; private set; }
		internal ReadOnlyDictionary<string, HandlerInformation> Handlers { get; private set; }
		internal SortedSet<string> Namespaces { get; private set; }
		internal string TypeName { get; set; }
	}
}
