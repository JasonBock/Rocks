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
using static Rocks.Extensions.ConstructorInfoExtensions;
using static Rocks.Extensions.MethodBaseExtensions;
using static Rocks.Extensions.MethodInfoExtensions;
using static Rocks.Extensions.PropertyInfoExtensions;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Construction
{
	internal abstract class Builder<TInformationBuilder>
		where TInformationBuilder : MethodInformationBuilder
	{
		internal Builder(Type baseType,
			ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers,
			SortedSet<string> namespaces, Options options, NameGenerator generator,
			TInformationBuilder informationBuilder, TypeNameGenerator typeNameGenerator)
		{
			this.BaseType = baseType;
			this.IsUnsafe = this.BaseType.IsUnsafeToMock();
			this.Handlers = handlers;
			this.Namespaces = namespaces;
			this.Options = options;
			this.NameGenerator = generator;
			this.InformationBuilder = informationBuilder;
			this.TypeName = typeNameGenerator.Generate(baseType);
      }

		protected abstract GetGeneratedEventsResults GetGeneratedEvents();

		internal virtual void Build()
		{
			this.Tree = this.MakeTree();
		}

		private List<string> GetGeneratedConstructors()
		{
			var generatedConstructors = new List<string>();
			var constructorName = this.GetTypeNameWithNoGenerics();

			if (this.BaseType.IsInterface)
			{
				generatedConstructors.Add(CodeTemplates.GetConstructorTemplate(
					constructorName, string.Empty, string.Empty));
			}
			else
			{
				foreach (var constructor in this.BaseType.GetMockableConstructors(this.NameGenerator))
				{
					var baseConstructor = constructor.Value;

					var parameters = baseConstructor.GetParameters(this.Namespaces);

					if (!string.IsNullOrWhiteSpace(parameters))
					{
						parameters = $", {parameters}";
					}

					generatedConstructors.Add(CodeTemplates.GetConstructorTemplate(
						constructorName, baseConstructor.GetArgumentNameList(), parameters));
					this.IsUnsafe |= baseConstructor.IsUnsafeToMock();
					this.RequiresObsoleteSuppression |= baseConstructor.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
			}

			return generatedConstructors;
		}

		private List<string> GetGeneratedMethods()
		{
			var generatedMethods = new List<string>();

			foreach (var method in this.BaseType.GetMockableMethods(this.NameGenerator))
			{				
				var methodInformation = this.InformationBuilder.Build(method);
				var baseMethod = method.Value;
				var argumentNameList = baseMethod.GetArgumentNameList();
				var outInitializers = !methodInformation.ContainsDelegateConditions ? string.Empty : baseMethod.GetOutInitializers();

				if (baseMethod.IsPublic)
				{
					var visibility = method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ? 
						string.Empty : CodeTemplates.Public;

					// Either the base method contains no refs/outs, or the user specified a delegate
					// to use to handle that method (remember, types with methods with refs/outs are gen'd
					// each time, and that's the only reason the handlers are passed in).
					if (!methodInformation.ContainsDelegateConditions || !string.IsNullOrWhiteSpace(methodInformation.DelegateCast))
					{
						if (!methodInformation.ContainsDelegateConditions && baseMethod.GetParameters().Length > 0)
						{
							generatedMethods.Add(this.GenerateMethodWithNoRefOutParameters(
								baseMethod, methodInformation.DelegateCast, argumentNameList, outInitializers, methodInformation.DescriptionWithOverride,
								visibility, method.RequiresNewImplementation));
						}
						else
						{
							generatedMethods.Add(this.GenerateMethodWithRefOutOrNoParameters(
								baseMethod, methodInformation.DelegateCast, argumentNameList, outInitializers, methodInformation.DescriptionWithOverride,
								visibility, method.RequiresNewImplementation));

							if (methodInformation.ContainsDelegateConditions)
							{
								this.HandleRefOutMethod(baseMethod, methodInformation);
							}
						}
					}
					else
					{
						generatedMethods.Add(CodeTemplates.GetRefOutNotImplementedMethodTemplate(methodInformation.DescriptionWithOverride));
					}

					this.RequiresObsoleteSuppression |= baseMethod.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
				else if (!baseMethod.IsPrivate && baseMethod.IsAbstract)
				{
					var visibility = CodeTemplates.GetVisibility(baseMethod.IsFamily, baseMethod.IsFamilyOrAssembly);

					generatedMethods.Add(baseMethod.ReturnType != typeof(void) ?
						CodeTemplates.GetNonPublicFunctionImplementationTemplate(visibility, methodInformation.Description,
							outInitializers, $"{baseMethod.ReturnType.GetFullName()}",
							method.RequiresNewImplementation == RequiresIsNewImplementation.Yes ? "new" : string.Empty, 
							baseMethod.ReturnParameter.GetAttributes(true, this.Namespaces)) :
						CodeTemplates.GetNonPublicActionImplementationTemplate(visibility, methodInformation.Description,
							outInitializers, method.RequiresNewImplementation == RequiresIsNewImplementation.Yes ? "new" : string.Empty));

					this.RequiresObsoleteSuppression |= baseMethod.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
			}

			return generatedMethods;
		}

		private string GenerateMethodWithNoRefOutParameters(MethodInfo baseMethod, string delegateCast, string argumentNameList, string outInitializers, string methodDescriptionWithOverride,
			string visibility, RequiresIsNewImplementation requiresIsNewImplementation)
		{
			var expectationChecks = baseMethod.GetExpectationChecks();
			var expectationExceptionMessage = baseMethod.GetExpectationExceptionMessage();

			if (baseMethod.ReturnType != typeof(void))
			{
				var returnTypeAttributes = baseMethod.ReturnParameter.GetAttributes(true, this.Namespaces);
            return baseMethod.ReturnType.RequiresExplicitCast() ?
						CodeTemplates.GetFunctionWithValueTypeReturnValueMethodTemplate(
							baseMethod.MetadataToken, argumentNameList, $"{baseMethod.ReturnType.GetFullName()}",
							expectationChecks, delegateCast, outInitializers, expectationExceptionMessage, methodDescriptionWithOverride,
							visibility, requiresIsNewImplementation == RequiresIsNewImplementation.Yes ? "new" : string.Empty,
							returnTypeAttributes):
                  CodeTemplates.GetFunctionWithReferenceTypeReturnValueMethodTemplate(
							baseMethod.MetadataToken, argumentNameList, $"{baseMethod.ReturnType.GetFullName()}",
							expectationChecks, delegateCast, outInitializers, expectationExceptionMessage, methodDescriptionWithOverride,
							visibility, requiresIsNewImplementation == RequiresIsNewImplementation.Yes ? "new" : string.Empty,
							returnTypeAttributes);
			}
			else
			{
				return CodeTemplates.GetActionMethodTemplate(
					baseMethod.MetadataToken, argumentNameList, expectationChecks, delegateCast, outInitializers, expectationExceptionMessage, methodDescriptionWithOverride,
					visibility);
			}
		}

		protected virtual void HandleRefOutMethod(MethodInfo baseMethod, MethodInformation methodDescription) { }

		private string GenerateMethodWithRefOutOrNoParameters(MethodInfo baseMethod, string delegateCast, string argumentNameList, string outInitializers, string methodDescriptionWithOverride,
			string visibility, RequiresIsNewImplementation requiresIsNewImplementation)
		{
			if (baseMethod.ReturnType != typeof(void))
			{
				var returnTypeAttributes = baseMethod.ReturnParameter.GetAttributes(true, this.Namespaces);
            return baseMethod.ReturnType.RequiresExplicitCast() ?
						CodeTemplates.GetFunctionWithValueTypeReturnValueAndNoArgumentsMethodTemplate(
							baseMethod.MetadataToken, argumentNameList, $"{baseMethod.ReturnType.GetFullName(this.Namespaces)}",
							delegateCast, outInitializers, methodDescriptionWithOverride, visibility,
							requiresIsNewImplementation == RequiresIsNewImplementation.Yes ? "new" : string.Empty,
							returnTypeAttributes) :
						CodeTemplates.GetFunctionWithReferenceTypeReturnValueAndNoArgumentsMethodTemplate(
							baseMethod.MetadataToken, argumentNameList, $"{baseMethod.ReturnType.GetFullName(this.Namespaces)}",
							delegateCast, outInitializers, methodDescriptionWithOverride, visibility,
							requiresIsNewImplementation == RequiresIsNewImplementation.Yes ? "new" : string.Empty,
							returnTypeAttributes);
			}
			else
			{
				return CodeTemplates.GetActionMethodWithNoArgumentsTemplate(
					baseMethod.MetadataToken, argumentNameList, delegateCast, outInitializers, methodDescriptionWithOverride,
					visibility);
			}
		}

		private List<string> GetGeneratedProperties()
		{
			var generatedProperties = new List<string>();

			foreach (var property in this.BaseType.GetMockableProperties(this.NameGenerator))
			{
				var baseProperty = property.Value;

				this.Namespaces.Add(baseProperty.PropertyType.Namespace);
				var indexers = baseProperty.GetIndexParameters();
				var propertyMethod = baseProperty.GetDefaultMethod();
				var methodInformation = this.InformationBuilder.Build(new MockableResult<MethodInfo>(
					propertyMethod, RequiresExplicitInterfaceImplementation.No));
				var @override = methodInformation.DescriptionWithOverride.Contains("override") ? "override " : string.Empty;

				if (propertyMethod.IsPublic)
				{
					var propertyImplementations = new List<string>();

					if (property.Accessors == PropertyAccessors.Get || property.Accessors == PropertyAccessors.GetAndSet)
					{
						var getMethod = baseProperty.GetMethod;
						var getVisibility = getMethod.IsPublic ? string.Empty : CodeTemplates.GetVisibility(getMethod.IsFamily, getMethod.IsFamilyOrAssembly);
						var getArgumentNameList = getMethod.GetArgumentNameList();
						var getDelegateCast = getMethod.GetDelegateCast();

						if (getMethod.GetParameters().Length > 0)
						{
							var getExpectationChecks = getMethod.GetExpectationChecks();
							var getExpectationExceptionMessage = getMethod.GetExpectationExceptionMessage();
							propertyImplementations.Add(getMethod.ReturnType.RequiresExplicitCast() ?
								CodeTemplates.GetPropertyGetWithValueTypeReturnValueTemplate(
									getMethod.MetadataToken, getArgumentNameList, $"{getMethod.ReturnType.GetFullName(this.Namespaces)}",
									getExpectationChecks, getDelegateCast, getExpectationExceptionMessage, getVisibility) :
								CodeTemplates.GetPropertyGetWithReferenceTypeReturnValueTemplate(
									getMethod.MetadataToken, getArgumentNameList, $"{getMethod.ReturnType.GetFullName(this.Namespaces)}",
									getExpectationChecks, getDelegateCast, getExpectationExceptionMessage, getVisibility));
						}
						else
						{
							propertyImplementations.Add(getMethod.ReturnType.RequiresExplicitCast() ?
								CodeTemplates.GetPropertyGetWithValueTypeReturnValueAndNoIndexersTemplate(
									getMethod.MetadataToken, getArgumentNameList,
									$"{getMethod.ReturnType.GetFullName(this.Namespaces)}", getDelegateCast, getVisibility) :
								CodeTemplates.GetPropertyGetWithReferenceTypeReturnValueAndNoIndexersTemplate(
									getMethod.MetadataToken, getArgumentNameList,
									$"{getMethod.ReturnType.GetFullName(this.Namespaces)}", getDelegateCast, getVisibility));
						}
					}

					if (property.Accessors == PropertyAccessors.Set || property.Accessors == PropertyAccessors.GetAndSet)
					{
						var setMethod = baseProperty.SetMethod;
						var setVisibility = setMethod.IsPublic ? string.Empty : CodeTemplates.GetVisibility(setMethod.IsFamily, setMethod.IsFamilyOrAssembly);
						var setArgumentNameList = setMethod.GetArgumentNameList();
						var setDelegateCast = setMethod.GetDelegateCast();

						if (setMethod.GetParameters().Length > 0)
						{
							var setExpectationChecks = setMethod.GetExpectationChecks();
							var setExpectationExceptionMessage = setMethod.GetExpectationExceptionMessage();
							propertyImplementations.Add(CodeTemplates.GetPropertySetTemplate(
								setMethod.MetadataToken, setArgumentNameList, setExpectationChecks, setDelegateCast, setExpectationExceptionMessage, setVisibility));
						}
						else
						{
							propertyImplementations.Add(CodeTemplates.GetPropertySetAndNoIndexersTemplate(
								setMethod.MetadataToken, setArgumentNameList, setDelegateCast, setVisibility));
						}
					}

					var visibility = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ? 
						string.Empty : CodeTemplates.Public;
					var explicitInterfaceName = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
						$"{property.Value.DeclaringType.GetFullName(this.Namespaces)}." : string.Empty;

					if (indexers.Length > 0)
					{
						var parameters = string.Join(", ",
							from indexer in indexers
							let _ = this.Namespaces.Add(indexer.ParameterType.Namespace)
							select $"{indexer.ParameterType.Name} {indexer.Name}");

						// Indexer
						generatedProperties.Add(CodeTemplates.GetPropertyIndexerTemplate(
							$"{@override}{baseProperty.PropertyType.GetFullName(this.Namespaces)}", parameters,
							string.Join(Environment.NewLine, propertyImplementations), visibility, explicitInterfaceName));
					}
					else
					{
						// Normal
						generatedProperties.Add(CodeTemplates.GetPropertyTemplate(
							$"{@override}{baseProperty.PropertyType.GetFullName(this.Namespaces)}", baseProperty.Name,
							string.Join(Environment.NewLine, propertyImplementations), visibility, explicitInterfaceName));
					}

					this.RequiresObsoleteSuppression |= baseProperty.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
				else if (!propertyMethod.IsPrivate && propertyMethod.IsAbstract)
				{
					var propertyImplementations = new List<string>();
					var visibility = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ? 
						string.Empty : CodeTemplates.GetVisibility(propertyMethod.IsFamily, propertyMethod.IsFamilyOrAssembly);

					if (property.Accessors == PropertyAccessors.Get || property.Accessors == PropertyAccessors.GetAndSet)
					{
						var getVisibility = CodeTemplates.GetVisibility(baseProperty.GetMethod.IsFamily, baseProperty.GetMethod.IsFamilyOrAssembly);

						if (getVisibility == visibility)
						{
							getVisibility = string.Empty;
						}

						propertyImplementations.Add(CodeTemplates.GetNonPublicPropertyGetTemplate(getVisibility));
					}

					if (property.Accessors == PropertyAccessors.Set || property.Accessors == PropertyAccessors.GetAndSet)
					{
						var setVisibility = CodeTemplates.GetVisibility(baseProperty.SetMethod.IsFamily, baseProperty.SetMethod.IsFamilyOrAssembly);

						if (setVisibility == visibility)
						{
							setVisibility = string.Empty;
						}

						propertyImplementations.Add(CodeTemplates.GetNonPublicPropertySetTemplate(setVisibility));
					}

					var explicitInterfaceName = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
						$"{property.Value.DeclaringType.GetFullName(this.Namespaces)}." : string.Empty;

					if (indexers.Length > 0)
					{
						var parameters = string.Join(", ",
							from indexer in indexers
							let _ = this.Namespaces.Add(indexer.ParameterType.Namespace)
							select $"{indexer.ParameterType.Name} {indexer.Name}");

						// Indexer
						generatedProperties.Add(CodeTemplates.GetNonPublicPropertyIndexerTemplate(visibility,
							$"{baseProperty.PropertyType.GetFullName(this.Namespaces)}", parameters,
							string.Join(Environment.NewLine, propertyImplementations), explicitInterfaceName));
					}
					else
					{
						// Normal
						generatedProperties.Add(CodeTemplates.GetNonPublicPropertyTemplate(visibility,
							$"{baseProperty.PropertyType.GetFullName(this.Namespaces)}", baseProperty.Name,
							string.Join(Environment.NewLine, propertyImplementations), explicitInterfaceName));
					}

					this.RequiresObsoleteSuppression |= baseProperty.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
			}

			return generatedProperties;
		}

		protected string GetTypeNameWithNoGenerics() => this.TypeName.Split('<').First();

		protected string GetTypeNameWithGenericsAndNoTextFormatting() => $"{this.TypeName.Replace("<", string.Empty).Replace(">", string.Empty).Replace(", ", string.Empty)}";

		private string MakeCode()
		{
			var methods = this.GetGeneratedMethods();
			var constructors = this.GetGeneratedConstructors();
			var properties = this.GetGeneratedProperties();
			var events = this.GetGeneratedEvents();

			this.RequiresObsoleteSuppression |= this.BaseType.GetCustomAttribute<ObsoleteAttribute>() != null ||
				events.RequiresObsoleteSuppression;

			this.Namespaces.Add(typeof(ExpectationException).Namespace);
			this.Namespaces.Add(typeof(IMock).Namespace);
			this.Namespaces.Add(typeof(HandlerInformation).Namespace);
			this.Namespaces.Add(typeof(string).Namespace);
			this.Namespaces.Add(typeof(ReadOnlyDictionary<,>).Namespace);
			this.Namespaces.Add(typeof(BindingFlags).Namespace);
			this.Namespaces.Remove(this.BaseType.Namespace);

			var baseTypeGenericArguments = this.BaseType.GetGenericArguments(this.Namespaces);

			var @class = CodeTemplates.GetClassTemplate(
				string.Join(Environment.NewLine,
					(from @namespace in this.Namespaces
					 select $"using {@namespace};")),
				this.TypeName, this.BaseType.GetFullName(),
				string.Join(Environment.NewLine, methods),
				string.Join(Environment.NewLine, properties),
				string.Join(Environment.NewLine, events.Events),
				string.Join(Environment.NewLine, constructors),
				this.BaseType.Namespace,
				this.Options.Serialization == SerializationOptions.Supported ?
					"[Serializable]" : string.Empty,
				this.Options.Serialization == SerializationOptions.Supported ?
					CodeTemplates.GetConstructorNoArgumentsTemplate(this.GetTypeNameWithNoGenerics()) : string.Empty,
				this.GetAdditionNamespaceCode(),
				this.IsUnsafe, baseTypeGenericArguments.Constraints);

			if (this.RequiresObsoleteSuppression)
			{
				@class =
$@"#pragma warning disable CS0618
#pragma warning disable CS0672
{@class}
#pragma warning restore CS0672
#pragma warning restore CS0618";
			}

			return @class;
		}

		private SyntaxTree MakeTree()
		{
			var @class = this.MakeCode();
			SyntaxTree tree = null;

			if (this.Options.CodeFile == CodeFileOptions.Create)
			{
				Directory.CreateDirectory(this.GetDirectoryForFile());
				var fileName = Path.Combine(this.GetDirectoryForFile(),
					$"{this.GetTypeNameWithGenericsAndNoTextFormatting()}.cs");
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
		protected virtual string GetAdditionNamespaceCode() => string.Empty;

		internal Options Options { get; }
		internal SyntaxTree Tree { get; private set; }
		internal Type BaseType { get; }
		internal bool IsUnsafe { get; private set; }
		internal ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> Handlers { get; }
		internal SortedSet<string> Namespaces { get; }
		internal string TypeName { get; set; }
		private bool RequiresObsoleteSuppression { get; set; }
		protected NameGenerator NameGenerator { get; private set; }
		internal TInformationBuilder InformationBuilder { get; private set; }
	}
}
