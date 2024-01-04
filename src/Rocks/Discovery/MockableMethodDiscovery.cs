using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Discovery;

internal sealed class MockableMethodDiscovery
{
	internal MockableMethodDiscovery(ITypeSymbol mockType, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		HashSet<ITypeSymbol> shims, Compilation compilation, ref uint memberIdentifier)
	{
		// We need to get the set of methods from object (static, instance, virtual, non-virtual, doesn't matter)
		// because the mock object will derive from object,
		// and interfaces like IEquatable<T> have a method (Equals(T? other))
		// that have the possibility of colliding with methods from interfaces.
		// We don't want to include them, we just need to consider them to see
		// if a method requires explicit implementation.
		// We also need to flag methods (especially if the mock type is a class)
		// that are discovered like GetHashCode(), because the generated method expectation class
		// will generate a "GetHashCode()" method for users to set an expectation,
		// and that expectation method needs to hide the base class method (e.g. mark it as "new")
		var objectSymbol = compilation.GetTypeByMetadataName(typeof(object).FullName)!;
		var objectMethods = objectSymbol.GetMembers().OfType<IMethodSymbol>()
			.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeReferencedByName &&
				_.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol)).ToImmutableArray();

		this.Methods =
			mockType.TypeKind == TypeKind.Interface ?
				GetMethodsForInterface(mockType, containingAssemblyOfInvocationSymbol, shims, objectMethods, ref memberIdentifier) :
				GetMethodsForClass(mockType, containingAssemblyOfInvocationSymbol, shims, objectMethods, ref memberIdentifier);
	}

	private static MockableMethods GetMethodsForClass(ITypeSymbol mockType, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		HashSet<ITypeSymbol> shims, ImmutableArray<IMethodSymbol> objectMethods, ref uint memberIdentifier)
	{
		var methods = ImmutableArray.CreateBuilder<MockableMethodResult>();
		var inaccessibleAbstractMembers = false;
		var hasMatchWithNonVirtual = false;

		var hierarchy = mockType.GetInheritanceHierarchy();

		foreach (var hierarchyType in hierarchy)
		{
			var hierarchyMethods = hierarchyType.GetMembers().OfType<IMethodSymbol>()
				.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeReferencedByName).ToArray();

			if (hierarchyMethods.Length > 0)
			{
				var hierarchyNonMockableMethods = hierarchyType.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => _.MethodKind == MethodKind.Ordinary && _.CanBeReferencedByName &&
						!(_.IsAbstract || _.IsOverride || _.IsVirtual)).ToArray();

				foreach (var hierarchyMethod in hierarchyMethods)
				{
					if (hierarchyMethod.IsStatic && hierarchyMethod.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol))
					{
						// This is the case where a class does something like this:
						// `protected static new string ToString()`
						// We can see it, and it's hiding a method from a class in the hierarchy,
						// so we have to remove the one that we currently have in the list.
						// If it "shows up" again in a class lower in hierarchy,
						// we'll just add it again.
						var methodToRemove = methods.SingleOrDefault(_ => !(_.Value.Match(hierarchyMethod) == MethodMatch.None) &&
							!_.Value.ContainingType.Equals(hierarchyMethod.ContainingType));

						if (methodToRemove is not null)
						{
							methods.Remove(methodToRemove);
						}
					}
					else if (!hierarchyMethod.IsStatic && (!mockType.IsRecord || hierarchyMethod.Name != nameof(Equals)))
					{
						if (hierarchyMethod.IsAbstract || hierarchyMethod.IsOverride || hierarchyMethod.IsVirtual)
						{
							var canBeSeen = hierarchyMethod.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol);

							if (!canBeSeen && hierarchyMethod.IsAbstract)
							{
								inaccessibleAbstractMembers = true;
							}
							else if (canBeSeen)
							{
								var methodToRemove = methods.SingleOrDefault(_ => !(_.Value.Match(hierarchyMethod) == MethodMatch.None));

								if (methodToRemove is not null)
								{
									methods.Remove(methodToRemove);
								}

								if (hierarchyNonMockableMethods.Any(_ => _.Match(hierarchyMethod) != MethodMatch.None) &&
									!hierarchyMethod.IsStatic && hierarchyMethod.IsAbstract)
								{
									// This is a case where the mockable method matches a non-virtual method
									// on the type and it's abstract. This effectively makes the entire type 
									// not mockable.
									hasMatchWithNonVirtual = true;
								}
								else if ((methodToRemove is null || !methodToRemove.Value.ContainingType.Equals(hierarchyMethod.ContainingType)) &&
									!hierarchyMethod.IsSealed)
								{
									methods.Add(new(hierarchyMethod, mockType, RequiresExplicitInterfaceImplementation.No, RequiresOverride.Yes, 
										objectMethods.Any(_ => hierarchyMethod.Name == _.Name && hierarchyMethod.Parameters.Length == 0) ?
											RequiresHiding.Yes : RequiresHiding.No, memberIdentifier));

									if (hierarchyMethod.ContainingType.TypeKind == TypeKind.Interface && hierarchyMethod.IsVirtual)
									{
										shims.Add(hierarchyMethod.ContainingType);
									}

									memberIdentifier++;
								}
							}
						}
						else
						{
							// This is an instance method that could be hiding a virtual method in a base class.
							// If it is, then we need to remove that base method from the list.
							var methodToRemove = methods.SingleOrDefault(_ => !(_.Value.Match(hierarchyMethod) == MethodMatch.None) &&
								!_.Value.ContainingType.Equals(hierarchyMethod.ContainingType));

							if (methodToRemove is not null)
							{
								methods.Remove(methodToRemove);
							}
						}
					}
				}

				// One more sweep. If any of the candidates match a non-mockable method, 
				// we have to remove it.
				methods.RemoveAll(_ => hierarchyNonMockableMethods.Any(
					hierarchyNonMockable => !(_.Value.Match(hierarchyNonMockable) == MethodMatch.None)));
			}
		}

		return new(methods.ToImmutable(), inaccessibleAbstractMembers, hasMatchWithNonVirtual);
	}

	private static MockableMethods GetMethodsForInterface(ITypeSymbol mockType, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		HashSet<ITypeSymbol> shims, ImmutableArray<IMethodSymbol> objectMethods, ref uint memberIdentifier)
	{
		// We don't want to include non-virtual static methods.
		// Later on in MockModel I make a check for static abstract methods.
		static bool IsMethodToExamine(IMethodSymbol method) =>
			!(method.IsStatic && !method.IsAbstract && !method.IsVirtual) &&
			(method.IsAbstract || method.IsVirtual) &&
			method.MethodKind == MethodKind.Ordinary && method.CanBeReferencedByName;

		var methods = ImmutableArray.CreateBuilder<MockableMethodResult>();
		var inaccessibleAbstractMembers = false;
		var hasMatchWithNonVirtual = false;

		foreach (var selfMethod in mockType.GetMembers().OfType<IMethodSymbol>()
			.Where(IsMethodToExamine))
		{
			if (!selfMethod.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol) &&
				selfMethod.IsAbstract)
			{
				inaccessibleAbstractMembers = true;
			}
			else
			{
				// We need to explicitly implement matching methods from the object type,
				// but if they exactly match from methods on the type itself,
				// we don't even add it to the list.
				if (!methods.Any(_ => _.Value.Match(selfMethod) == MethodMatch.Exact))
				{
					var selfMethodRequiresExplicit = objectMethods.Any(
						_ => _.Match(selfMethod) switch
						{
							MethodMatch.DifferByReturnTypeOnly or MethodMatch.Exact => true,
							_ => false
						}) || methods.Any(
						_ => _.Value.Match(selfMethod) switch
						{
							MethodMatch.DifferByReturnTypeOnly => true,
							_ => false
						}) ? RequiresExplicitInterfaceImplementation.Yes : RequiresExplicitInterfaceImplementation.No;
					methods.Add(new(selfMethod, mockType, selfMethodRequiresExplicit, RequiresOverride.No,
						objectMethods.Any(_ => selfMethod.Name == _.Name && selfMethod.Parameters.Length == 0) ?
							RequiresHiding.Yes : RequiresHiding.No, memberIdentifier));

					if (selfMethod.IsVirtual)
					{
						shims.Add(mockType);
					}

					memberIdentifier++;
				}
			}
		}

		var baseInterfaceMethodGroups = new List<List<IMethodSymbol>>();

		foreach (var selfBaseInterface in mockType.AllInterfaces)
		{
			foreach (var selfBaseMethod in selfBaseInterface.GetMembers().OfType<IMethodSymbol>()
				.Where(IsMethodToExamine))
			{
				if (!selfBaseMethod.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol))
				{
					inaccessibleAbstractMembers = true;
				}
				else
				{
					if (!methods.Any(_ => _.Value.Match(selfBaseMethod) == MethodMatch.Exact))
					{
						var foundMatch = false;

						foreach (var baseInterfaceMethodGroup in baseInterfaceMethodGroups)
						{
							if (baseInterfaceMethodGroup.Any(_ => _.Match(selfBaseMethod) == MethodMatch.Exact))
							{
								baseInterfaceMethodGroup.Add(selfBaseMethod);
								foundMatch = true;
								break;
							}
						}

						if (!foundMatch)
						{
							baseInterfaceMethodGroups.Add(new List<IMethodSymbol>(1) { selfBaseMethod });
						}
					}
				}
			}
		}

		if (baseInterfaceMethodGroups.Count > 0)
		{
			foreach (var baseInterfaceMethodGroup in baseInterfaceMethodGroups)
			{
				if (baseInterfaceMethodGroup.Count == 1)
				{
					// If this one method differs by return type only
					// from any other method currently captured in the list
					// (think IEnumerable<T> -> IEnumerable and their GetEnumerator() methods),
					// or any of the virtual methods from object,
					// then we must require explicit implementation.
					var requiresExplicitImplementation = methods.Any(
						_ => _.Value.Match(baseInterfaceMethodGroup[0]) == MethodMatch.DifferByReturnTypeOnly) ||
						objectMethods.Any(_ => _.Match(baseInterfaceMethodGroup[0]) switch
						{
							MethodMatch.DifferByReturnTypeOnly or MethodMatch.Exact => true,
							_ => false
						}) ?
						RequiresExplicitInterfaceImplementation.Yes :
						RequiresExplicitInterfaceImplementation.No;

					methods.Add(new(baseInterfaceMethodGroup[0], mockType,
						requiresExplicitImplementation, RequiresOverride.No,
						objectMethods.Any(_ => baseInterfaceMethodGroup[0].Name == _.Name && baseInterfaceMethodGroup[0].Parameters.Length == 0) ?
							RequiresHiding.Yes : RequiresHiding.No,
						memberIdentifier));

					if (baseInterfaceMethodGroup[0].IsVirtual)
					{
						shims.Add(baseInterfaceMethodGroup[0].ContainingType);
					}

					memberIdentifier++;
				}
				else
				{
					foreach (var baseInterfaceMethod in baseInterfaceMethodGroup)
					{
						methods.Add(new(baseInterfaceMethod, mockType,
							RequiresExplicitInterfaceImplementation.Yes, RequiresOverride.No,
							objectMethods.Any(_ => baseInterfaceMethod.Name == _.Name && baseInterfaceMethod.Parameters.Length == 0) ?
								RequiresHiding.Yes : RequiresHiding.No,
							memberIdentifier));

						if (baseInterfaceMethod.IsVirtual)
						{
							shims.Add(baseInterfaceMethod.ContainingType);
						}

						memberIdentifier++;
					}
				}
			}
		}

		return new(methods.ToImmutable(), inaccessibleAbstractMembers, hasMatchWithNonVirtual);
	}

	internal MockableMethods Methods { get; }
}