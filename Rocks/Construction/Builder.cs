using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocks.Exceptions;
using Rocks.Extensions;
using Rocks.Options;
using Rocks.Templates;
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
			SortedSet<string> namespaces, RockOptions options, NameGenerator generator,
			TInformationBuilder informationBuilder, TypeNameGenerator typeNameGenerator,
			bool isMake)
		{
			this.BaseType = baseType;
			this.IsUnsafe = this.BaseType.IsUnsafeToMock();
			this.Handlers = handlers;
			this.Namespaces = namespaces;
			this.Options = options;
			this.NameGenerator = generator;
			this.InformationBuilder = informationBuilder;
			this.TypeName = typeNameGenerator.Generate(baseType);
			this.IsMake = isMake;
		}

		protected abstract GetGeneratedEventsResults GetGeneratedEvents();

		internal virtual void Build()
		{
			this.Tree = this.MakeTree();
		}

		private string GetGeneratedConstructors()
		{
			var generatedConstructors = new List<string>();
			var constructorName = this.GetTypeNameWithNoGenerics();

			if (this.BaseType.IsInterface)
			{
				generatedConstructors.Add(ConstructorTemplates.GetConstructor(
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

					generatedConstructors.Add(ConstructorTemplates.GetConstructor(
						constructorName, baseConstructor.GetArgumentNameList(), parameters));
					this.IsUnsafe |= baseConstructor.IsUnsafeToMock();
					this.RequiresObsoleteSuppression |= baseConstructor.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
			}

			return string.Join(Environment.NewLine, generatedConstructors);
		}

		private string GetGeneratedMethods()
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
					if (this.IsMake || !methodInformation.ContainsDelegateConditions || !string.IsNullOrWhiteSpace(methodInformation.DelegateCast))
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
						generatedMethods.Add(MethodTemplates.GetRefOutNotImplementedMethod(methodInformation.DescriptionWithOverride));
					}

					this.RequiresObsoleteSuppression |= baseMethod.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
				else if (!baseMethod.IsPrivate && baseMethod.IsAbstract)
				{
					var visibility = CodeTemplates.GetVisibility(baseMethod.IsFamily, baseMethod.IsFamilyOrAssembly);

					generatedMethods.Add(baseMethod.ReturnType != typeof(void) ?
						MethodTemplates.GetNonPublicFunctionImplementation(visibility, methodInformation.Description,
							outInitializers, baseMethod.ReturnType,
							method.RequiresNewImplementation == RequiresIsNewImplementation.Yes ? "new" : string.Empty,
							baseMethod.ReturnParameter.GetAttributes(true, this.Namespaces)) :
						MethodTemplates.GetNonPublicActionImplementation(visibility, methodInformation.Description,
							outInitializers, method.RequiresNewImplementation == RequiresIsNewImplementation.Yes ? "new" : string.Empty));

					this.RequiresObsoleteSuppression |= baseMethod.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
			}

			return string.Join(Environment.NewLine, generatedMethods);
		}

		private string GenerateMethodWithNoRefOutParameters(MethodInfo baseMethod, string delegateCast, string argumentNameList, string outInitializers, string methodDescriptionWithOverride,
			string visibility, RequiresIsNewImplementation requiresIsNewImplementation)
		{
			var expectationChecks = baseMethod.GetExpectationChecks();
			var expectationExceptionMessage = baseMethod.GetExpectationExceptionMessage();
			var requiresNew = requiresIsNewImplementation == RequiresIsNewImplementation.Yes ? "new" : string.Empty;
			var returnTypeName = baseMethod.ReturnType.GetFullName();

         if (baseMethod.ReturnType != typeof(void))
			{
				var returnTypeAttributes = baseMethod.ReturnParameter.GetAttributes(true, this.Namespaces);
				return this.IsMake ?
					MethodTemplates.GetFunctionForMake(outInitializers, methodDescriptionWithOverride,
						visibility, requiresNew, returnTypeAttributes, baseMethod.ReturnType) :
					baseMethod.ReturnType.RequiresExplicitCast() ?
						MethodTemplates.GetFunctionWithValueTypeReturnValue(
							baseMethod.MetadataToken, argumentNameList, returnTypeName,
							expectationChecks, delegateCast, outInitializers, expectationExceptionMessage, methodDescriptionWithOverride,
							visibility, requiresNew, returnTypeAttributes) :
						MethodTemplates.GetFunctionWithReferenceTypeReturnValue(
							baseMethod.MetadataToken, argumentNameList, returnTypeName,
							expectationChecks, delegateCast, outInitializers, expectationExceptionMessage, methodDescriptionWithOverride,
							visibility, requiresNew, returnTypeAttributes);
			}
			else
			{
				return this.IsMake ?
					MethodTemplates.GetActionMethodForMake(outInitializers, methodDescriptionWithOverride, visibility) :
					MethodTemplates.GetActionMethod(baseMethod.MetadataToken, argumentNameList, expectationChecks,
						delegateCast, outInitializers, expectationExceptionMessage, methodDescriptionWithOverride, visibility);
			}
		}

		protected virtual void HandleRefOutMethod(MethodInfo baseMethod, MethodInformation methodDescription) { }

		private string GenerateMethodWithRefOutOrNoParameters(MethodInfo baseMethod, string delegateCast, string argumentNameList, string outInitializers, string methodDescriptionWithOverride,
			string visibility, RequiresIsNewImplementation requiresIsNewImplementation)
		{
			var requiresNew = requiresIsNewImplementation == RequiresIsNewImplementation.Yes ? "new" : string.Empty;
			var returnTypeName = baseMethod.ReturnType.GetFullName(this.Namespaces);

         if (baseMethod.ReturnType != typeof(void))
			{
				var returnTypeAttributes = baseMethod.ReturnParameter.GetAttributes(true, this.Namespaces);
				return this.IsMake ?
					MethodTemplates.GetFunctionForMake(outInitializers, methodDescriptionWithOverride,
						visibility, requiresNew, returnTypeAttributes, baseMethod.ReturnType) :
					baseMethod.ReturnType.RequiresExplicitCast() ?
						MethodTemplates.GetFunctionWithValueTypeReturnValueAndNoArguments(
							baseMethod.MetadataToken, argumentNameList, returnTypeName,
							delegateCast, outInitializers, methodDescriptionWithOverride, visibility,
							requiresNew, returnTypeAttributes) :
						MethodTemplates.GetFunctionWithReferenceTypeReturnValueAndNoArguments(
							baseMethod.MetadataToken, argumentNameList, returnTypeName,
							delegateCast, outInitializers, methodDescriptionWithOverride, visibility,
							requiresNew, returnTypeAttributes);
			}
			else
			{
				return this.IsMake ?
					MethodTemplates.GetActionMethodWithNoArgumentsForMake(outInitializers, methodDescriptionWithOverride, visibility) :
					MethodTemplates.GetActionMethodWithNoArguments(baseMethod.MetadataToken, argumentNameList, delegateCast, 
						outInitializers, methodDescriptionWithOverride, visibility);
			}
		}

		private string GetGeneratedProperties()
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
						var returnType = getMethod.ReturnType.GetFullName(this.Namespaces);

						if (this.IsMake)
						{
							propertyImplementations.Add(PropertyTemplates.GetPropertyGetForMake(getVisibility, returnType));
                  }
						else
						{
							if (getMethod.GetParameters().Length > 0)
							{
								var getExpectationChecks = getMethod.GetExpectationChecks();
								var getExpectationExceptionMessage = getMethod.GetExpectationExceptionMessage();
								propertyImplementations.Add(getMethod.ReturnType.RequiresExplicitCast() ?
									PropertyTemplates.GetPropertyGetWithValueTypeReturnValue(
										getMethod.MetadataToken, getArgumentNameList, returnType,
										getExpectationChecks, getDelegateCast, getExpectationExceptionMessage, getVisibility) :
									PropertyTemplates.GetPropertyGetWithReferenceTypeReturnValue(
										getMethod.MetadataToken, getArgumentNameList, returnType,
										getExpectationChecks, getDelegateCast, getExpectationExceptionMessage, getVisibility));
							}
							else
							{
								propertyImplementations.Add(getMethod.ReturnType.RequiresExplicitCast() ?
									PropertyTemplates.GetPropertyGetWithValueTypeReturnValueAndNoIndexers(
										getMethod.MetadataToken, getArgumentNameList, returnType, getDelegateCast, getVisibility) :
									PropertyTemplates.GetPropertyGetWithReferenceTypeReturnValueAndNoIndexers(
										getMethod.MetadataToken, getArgumentNameList, returnType, getDelegateCast, getVisibility));
							}
						}
					}

					if (property.Accessors == PropertyAccessors.Set || property.Accessors == PropertyAccessors.GetAndSet)
					{
						var setMethod = baseProperty.SetMethod;
						var setVisibility = setMethod.IsPublic ? string.Empty : CodeTemplates.GetVisibility(setMethod.IsFamily, setMethod.IsFamilyOrAssembly);
						var setArgumentNameList = setMethod.GetArgumentNameList();
						var setDelegateCast = setMethod.GetDelegateCast();

						if(this.IsMake)
						{
							propertyImplementations.Add(PropertyTemplates.GetPropertySetForMake(setVisibility));
						}
						else
						{
							if (setMethod.GetParameters().Length > 0)
							{
								var setExpectationChecks = setMethod.GetExpectationChecks();
								var setExpectationExceptionMessage = setMethod.GetExpectationExceptionMessage();
								propertyImplementations.Add(PropertyTemplates.GetPropertySet(
									setMethod.MetadataToken, setArgumentNameList, setExpectationChecks, setDelegateCast, setExpectationExceptionMessage, setVisibility));
							}
							else
							{
								propertyImplementations.Add(PropertyTemplates.GetPropertySetAndNoIndexers(
									setMethod.MetadataToken, setArgumentNameList, setDelegateCast, setVisibility));
							}
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
						generatedProperties.Add(PropertyTemplates.GetPropertyIndexer(
							$"{@override}{baseProperty.PropertyType.GetFullName(this.Namespaces)}", parameters,
							string.Join(Environment.NewLine, propertyImplementations), visibility, explicitInterfaceName));
					}
					else
					{
						// Normal
						generatedProperties.Add(PropertyTemplates.GetProperty(
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

						propertyImplementations.Add(PropertyTemplates.GetNonPublicPropertyGet(getVisibility));
					}

					if (property.Accessors == PropertyAccessors.Set || property.Accessors == PropertyAccessors.GetAndSet)
					{
						var setVisibility = CodeTemplates.GetVisibility(baseProperty.SetMethod.IsFamily, baseProperty.SetMethod.IsFamilyOrAssembly);

						if (setVisibility == visibility)
						{
							setVisibility = string.Empty;
						}

						propertyImplementations.Add(PropertyTemplates.GetNonPublicPropertySet(setVisibility));
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
						generatedProperties.Add(PropertyTemplates.GetNonPublicPropertyIndexer(visibility,
							$"{baseProperty.PropertyType.GetFullName(this.Namespaces)}", parameters,
							string.Join(Environment.NewLine, propertyImplementations), explicitInterfaceName));
					}
					else
					{
						// Normal
						generatedProperties.Add(PropertyTemplates.GetNonPublicProperty(visibility,
							$"{baseProperty.PropertyType.GetFullName(this.Namespaces)}", baseProperty.Name,
							string.Join(Environment.NewLine, propertyImplementations), explicitInterfaceName));
					}

					this.RequiresObsoleteSuppression |= baseProperty.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
			}

			return string.Join(Environment.NewLine, generatedProperties);
		}

		protected string GetTypeNameWithNoGenerics() => this.TypeName.Split('<').First();

		protected string GetTypeNameWithGenericsAndNoTextFormatting() => $"{this.TypeName.Replace("<", string.Empty).Replace(">", string.Empty).Replace(", ", string.Empty)}";

		private string MakeCode()
		{
			var methods = this.GetGeneratedMethods();
			var constructors = this.GetGeneratedConstructors();
			var properties = this.GetGeneratedProperties();
			var generatedEvents = this.GetGeneratedEvents();
			var events = generatedEvents.Events.Count > 0 ? EventTemplates.GetEvents(generatedEvents.Events) : string.Empty;

			this.RequiresObsoleteSuppression |= this.BaseType.GetCustomAttribute<ObsoleteAttribute>() != null ||
				generatedEvents.RequiresObsoleteSuppression;

			this.Namespaces.Remove(this.BaseType.Namespace);

			var baseTypeGenericArguments = this.BaseType.GetGenericArguments(this.Namespaces);

			var namespaces = string.Join(Environment.NewLine,
					(from @namespace in this.Namespaces
					 select $"using {@namespace};"));

			var @class = ClassTemplates.GetClass(namespaces,
				this.TypeName, this.BaseType.GetFullName(),
				methods, properties, events, constructors,
				this.BaseType.Namespace,
				this.Options.Serialization == SerializationOptions.Supported ?
					"[Serializable]" : string.Empty,
				this.Options.Serialization == SerializationOptions.Supported ?
					ConstructorTemplates.GetConstructorWithNoArguments(this.GetTypeNameWithNoGenerics()) : string.Empty,
				this.GetAdditionNamespaceCode(),
				this.IsUnsafe, baseTypeGenericArguments.Constraints);

			if (this.RequiresObsoleteSuppression)
			{
				@class = ClassTemplates.GetClassWithObsoleteSuppression(@class);
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

		internal RockOptions Options { get; }
		internal SyntaxTree Tree { get; private set; }
		internal Type BaseType { get; }
		internal bool IsUnsafe { get; private set; }
		internal ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> Handlers { get; }
		internal SortedSet<string> Namespaces { get; }
		internal string TypeName { get; set; }
		private bool RequiresObsoleteSuppression { get; set; }
		protected NameGenerator NameGenerator { get; private set; }
		internal TInformationBuilder InformationBuilder { get; private set; }
		internal bool IsMake { get; }
	}
}
