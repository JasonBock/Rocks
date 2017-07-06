using Rocks.Extensions;
using Rocks.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Rocks.Extensions.PropertyInfoExtensions;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Construction.Generators
{
	internal sealed class PropertiesGenerator
	{
		internal GenerateResults Generate(Type baseType, SortedSet<string> namespaces,
			NameGenerator generator, MethodInformationBuilder informationBuilder,
			bool isMake, bool hasEvents)
		{
			var requiresObsoleteSuppression = false;

			var generatedProperties = new List<string>();

			foreach (var property in baseType.GetMockableProperties(generator))
			{
				var baseProperty = property.Value;

				namespaces.Add(baseProperty.PropertyType.Namespace);
				var indexers = baseProperty.GetIndexParameters();
				var propertyMethod = baseProperty.GetDefaultMethod();
				var methodInformation = informationBuilder.Build(new MockableResult<MethodInfo>(
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
						var returnType = getMethod.ReturnType.GetFullName(namespaces);

						if (isMake)
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
										getExpectationChecks, getDelegateCast, getExpectationExceptionMessage, getVisibility, hasEvents) :
									PropertyTemplates.GetPropertyGetWithReferenceTypeReturnValue(
										getMethod.MetadataToken, getArgumentNameList, returnType,
										getExpectationChecks, getDelegateCast, getExpectationExceptionMessage, getVisibility, hasEvents));
							}
							else
							{
								propertyImplementations.Add(getMethod.ReturnType.RequiresExplicitCast() ?
									PropertyTemplates.GetPropertyGetWithValueTypeReturnValueAndNoIndexers(
										getMethod.MetadataToken, getArgumentNameList, returnType, getDelegateCast, getVisibility, hasEvents) :
									PropertyTemplates.GetPropertyGetWithReferenceTypeReturnValueAndNoIndexers(
										getMethod.MetadataToken, getArgumentNameList, returnType, getDelegateCast, getVisibility, hasEvents));
							}
						}
					}

					if (property.Accessors == PropertyAccessors.Set || property.Accessors == PropertyAccessors.GetAndSet)
					{
						var setMethod = baseProperty.SetMethod;
						var setVisibility = setMethod.IsPublic ? string.Empty : CodeTemplates.GetVisibility(setMethod.IsFamily, setMethod.IsFamilyOrAssembly);
						var setArgumentNameList = setMethod.GetArgumentNameList();
						var setDelegateCast = setMethod.GetDelegateCast();

						if (isMake)
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
									setMethod.MetadataToken, setArgumentNameList, setExpectationChecks, setDelegateCast, 
									setExpectationExceptionMessage, setVisibility, hasEvents));
							}
							else
							{
								propertyImplementations.Add(PropertyTemplates.GetPropertySetAndNoIndexers(
									setMethod.MetadataToken, setArgumentNameList, setDelegateCast, setVisibility, hasEvents));
							}
						}
					}

					var visibility = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
						string.Empty : CodeTemplates.Public;
					var explicitInterfaceName = property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.Yes ?
						$"{property.Value.DeclaringType.GetFullName(namespaces)}." : string.Empty;

					if (indexers.Length > 0)
					{
						var parameters = string.Join(", ",
							from indexer in indexers
							let _ = namespaces.Add(indexer.ParameterType.Namespace)
							select $"{indexer.ParameterType.Name} {indexer.Name}");

						// Indexer
						generatedProperties.Add(PropertyTemplates.GetPropertyIndexer(
							$"{@override}{baseProperty.PropertyType.GetFullName(namespaces)}", parameters,
							string.Join(Environment.NewLine, propertyImplementations), visibility, explicitInterfaceName));
					}
					else
					{
						// Normal
						generatedProperties.Add(PropertyTemplates.GetProperty(
							$"{@override}{baseProperty.PropertyType.GetFullName(namespaces)}", baseProperty.Name,
							string.Join(Environment.NewLine, propertyImplementations), visibility, explicitInterfaceName));
					}

					requiresObsoleteSuppression |= baseProperty.GetCustomAttribute<ObsoleteAttribute>() != null;
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
						$"{property.Value.DeclaringType.GetFullName(namespaces)}." : string.Empty;

					if (indexers.Length > 0)
					{
						var parameters = string.Join(", ",
							from indexer in indexers
							let _ = namespaces.Add(indexer.ParameterType.Namespace)
							select $"{indexer.ParameterType.Name} {indexer.Name}");

						// Indexer
						generatedProperties.Add(PropertyTemplates.GetNonPublicPropertyIndexer(visibility,
							$"{baseProperty.PropertyType.GetFullName(namespaces)}", parameters,
							string.Join(Environment.NewLine, propertyImplementations), explicitInterfaceName));
					}
					else
					{
						// Normal
						generatedProperties.Add(PropertyTemplates.GetNonPublicProperty(visibility,
							$"{baseProperty.PropertyType.GetFullName(namespaces)}", baseProperty.Name,
							string.Join(Environment.NewLine, propertyImplementations), explicitInterfaceName));
					}

					requiresObsoleteSuppression |= baseProperty.GetCustomAttribute<ObsoleteAttribute>() != null;
				}
			}

			return new GenerateResults(string.Join(Environment.NewLine, generatedProperties),
				requiresObsoleteSuppression, false);
		}
	}
}