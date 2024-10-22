using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Discovery;

internal sealed class MockablePropertyDiscovery
{
	internal MockablePropertyDiscovery(ITypeSymbol mockType, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		HashSet<ITypeSymbol> shims, ref uint memberIdentifier) =>
			this.Properties =
				mockType.TypeKind == TypeKind.Interface ?
					MockablePropertyDiscovery.GetPropertiesForInterface(mockType, containingAssemblyOfInvocationSymbol,
						shims, ref memberIdentifier) :
					MockablePropertyDiscovery.GetPropertiesForClass(mockType, containingAssemblyOfInvocationSymbol,
						shims, ref memberIdentifier);

	private static MockableProperties GetPropertiesForClass(ITypeSymbol mockType, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		HashSet<ITypeSymbol> shims, ref uint memberIdentifier)
	{
		static bool AreParametersEqual(IPropertySymbol property1, IPropertySymbol property2)
		{
			if (property1.Parameters.Length == property2.Parameters.Length)
			{
				for (var i = 0; i < property1.Parameters.Length; i++)
				{
					var property1Parameter = property1.Parameters[i];
					var property2Parameter = property2.Parameters[i];

					if (!property1Parameter.Type.Equals(property2Parameter.Type))
					{
						return false;
					}
				}

				return true;
			}

			return false;
		}

		var properties = ImmutableArray.CreateBuilder<MockablePropertyResult>();
		var inaccessibleAbstractMembers = false;

		var hierarchy = mockType.GetInheritanceHierarchy();

		foreach (var hierarchyType in hierarchy)
		{
			foreach (var hierarchyProperty in hierarchyType.GetMembers().OfType<IPropertySymbol>()
				.Where(_ => _.IsIndexer || _.CanBeReferencedByName))
			{
				var canBeSeen = hierarchyProperty.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol);

				if (canBeSeen)
				{
					// This is the case where a class does something like this:
					// `protected static new int Data { get; }`
					// We can see it, and it's hiding a property from a class in the hierarchy,
					// so we have to remove the one that we currently have in the list.
					// If it "shows up" again in a class lower in hierarchy,
					// we'll just add it again.
					var propertyToRemove = properties.SingleOrDefault(_ => _.Value.Name == hierarchyProperty.Name &&
						!_.Value.ContainingType.Equals(hierarchyProperty.ContainingType) &&
						AreParametersEqual(_.Value, hierarchyProperty));

					if (propertyToRemove is not null)
					{
						properties.Remove(propertyToRemove);
					}
				}

				if (!hierarchyProperty.IsStatic)
				{
					if (!canBeSeen && hierarchyProperty.IsAbstract)
					{
						inaccessibleAbstractMembers = true;
					}
					else if (canBeSeen)
					{
						if ((hierarchyProperty.IsAbstract || hierarchyProperty.IsOverride || hierarchyProperty.IsVirtual) && 
							!hierarchyProperty.IsSealed)
						{
							var result = new MockablePropertyResult(
								hierarchyProperty, mockType, RequiresExplicitInterfaceImplementation.No, RequiresOverride.Yes, memberIdentifier);
							properties.Add(result);

							if (hierarchyProperty.ContainingType.TypeKind == TypeKind.Interface && hierarchyProperty.IsVirtual)
							{
								shims.Add(hierarchyProperty.ContainingType);
							}

							memberIdentifier++;

							if (result.Accessors == PropertyAccessor.GetAndSet || result.Accessors == PropertyAccessor.GetAndInit)
							{
								memberIdentifier++;
							}
						}
					}
				}
			}
		}

		return new(properties.ToImmutable(), inaccessibleAbstractMembers, false);
	}

	private static MockableProperties GetPropertiesForInterface(ITypeSymbol mockType, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		HashSet<ITypeSymbol> shims, ref uint memberIdentifier)
	{
		static bool IsPropertyToExamine(IPropertySymbol property) =>
			!property.IsStatic && (property.IsAbstract || property.IsVirtual) &&
			(property.IsIndexer || property.CanBeReferencedByName);

		var properties = ImmutableArray.CreateBuilder<MockablePropertyResult>();
		var inaccessibleAbstractMembers = false;
		var hasStaticAbstractProperties = false;

		foreach (var selfProperty in mockType.GetMembers().OfType<IPropertySymbol>())
		{
			// Report if this method is a static abstract method.
			hasStaticAbstractProperties |= selfProperty.IsStatic && selfProperty.IsAbstract;

			if (IsPropertyToExamine(selfProperty))
			{
				if (!selfProperty.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol))
				{
					inaccessibleAbstractMembers = true;
				}
				else
				{
					var result = new MockablePropertyResult(selfProperty, mockType,
						RequiresExplicitInterfaceImplementation.No, RequiresOverride.No, memberIdentifier);
					properties.Add(result);

					if (selfProperty.IsVirtual)
					{
						shims.Add(mockType);
					}

					memberIdentifier++;

					if (result.Accessors == PropertyAccessor.GetAndSet || result.Accessors == PropertyAccessor.GetAndInit)
					{
						memberIdentifier++;
					}
				}
			}
		}

		var baseInterfacePropertyGroups = new List<List<IPropertySymbol>>();

		foreach (var selfBaseInterface in mockType.AllInterfaces)
		{
			foreach (var selfBaseProperty in selfBaseInterface.GetMembers().OfType<IPropertySymbol>())
			{
				// Report if this method is a static abstract method.
				hasStaticAbstractProperties |= selfBaseProperty.IsStatic && selfBaseProperty.IsAbstract;

				if (IsPropertyToExamine(selfBaseProperty))
				{
					if (!selfBaseProperty.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol))
					{
						inaccessibleAbstractMembers = true;
					}
					else
					{
						if (!properties.Any(_ =>
							selfBaseProperty.IsIndexer && _.Value.IsIndexer &&
								selfBaseProperty.GetMethod is not null && _.Value.GetMethod is not null && selfBaseProperty.GetMethod.Match(_.Value.GetMethod) == MethodMatch.Exact ||
								selfBaseProperty.SetMethod is not null && _.Value.SetMethod is not null && selfBaseProperty.SetMethod.Match(_.Value.SetMethod) == MethodMatch.Exact ||
							!selfBaseProperty.IsIndexer && !_.Value.IsIndexer &&
								_.Value.Name == selfBaseProperty.Name &&
								SymbolEqualityComparer.Default.Equals(_.Value.Type, selfBaseProperty.Type) &&
								_.Value.GetAccessors() == selfBaseProperty.GetAccessors()))
						{
							var foundMatch = false;

							foreach (var baseInterfacePropertyGroup in baseInterfacePropertyGroups)
							{
								if (baseInterfacePropertyGroup.Any(_ =>
									selfBaseProperty.IsIndexer && _.IsIndexer &&
										selfBaseProperty.GetMethod is not null && _.GetMethod is not null && selfBaseProperty.GetMethod.Match(_.GetMethod) == MethodMatch.Exact ||
										selfBaseProperty.SetMethod is not null && _.SetMethod is not null && selfBaseProperty.SetMethod.Match(_.SetMethod) == MethodMatch.Exact ||
									!selfBaseProperty.IsIndexer && !_.IsIndexer &&
										_.Name == selfBaseProperty.Name &&
										SymbolEqualityComparer.Default.Equals(_.Type, selfBaseProperty.Type) &&
										_.GetAccessors() == selfBaseProperty.GetAccessors()))
								{
									baseInterfacePropertyGroup.Add(selfBaseProperty);
									foundMatch = true;
									break;
								}
							}

							if (!foundMatch)
							{
								baseInterfacePropertyGroups.Add([selfBaseProperty]);
							}
						}
					}
				}
			}
		}

		foreach (var baseInterfacePropertyGroup in baseInterfacePropertyGroups)
		{
			if (baseInterfacePropertyGroup.Count == 1)
			{
				// If there are any properties that have the same name
				// but different types or different accessors.
				// then we must require explicit implementation.
				var requiresExplicitImplementation = properties.Any(
					_ => baseInterfacePropertyGroup[0].Name == _.Value.Name &&
						(!SymbolEqualityComparer.Default.Equals(baseInterfacePropertyGroup[0].Type, _.Value.Type) ||
						baseInterfacePropertyGroup[0].GetAccessors() != _.Value.GetAccessors())) ?
						RequiresExplicitInterfaceImplementation.Yes :
						RequiresExplicitInterfaceImplementation.No;

				var result = new MockablePropertyResult(baseInterfacePropertyGroup[0], mockType,
					requiresExplicitImplementation, RequiresOverride.No, memberIdentifier);
				var accessors = baseInterfacePropertyGroup[0].GetAccessors();
				properties.Add(result);

				if (baseInterfacePropertyGroup[0].IsVirtual)
				{
					shims.Add(baseInterfacePropertyGroup[0].ContainingType);
				}

				memberIdentifier++;

				if (result.Accessors == PropertyAccessor.GetAndSet || result.Accessors == PropertyAccessor.GetAndInit)
				{
					memberIdentifier++;
				}
			}
			else
			{
				foreach (var baseInterfaceProperty in baseInterfacePropertyGroup)
				{
					var result = new MockablePropertyResult(baseInterfaceProperty, mockType,
						RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.No, memberIdentifier);
					var accessors = baseInterfaceProperty.GetAccessors();
					properties.Add(result);

					if (baseInterfaceProperty.IsVirtual)
					{
						shims.Add(baseInterfaceProperty.ContainingType);
					}

					memberIdentifier++;

					if (result.Accessors == PropertyAccessor.GetAndSet || result.Accessors == PropertyAccessor.GetAndInit)
					{
						memberIdentifier++;
					}
				}
			}
		}

		return new MockableProperties(properties.ToImmutable(), inaccessibleAbstractMembers, hasStaticAbstractProperties);
	}

	internal MockableProperties Properties { get; }
}