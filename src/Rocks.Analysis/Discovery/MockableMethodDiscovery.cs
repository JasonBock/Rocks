using Microsoft.CodeAnalysis;
using Rocks.Analysis.Extensions;
using System.Collections.Immutable;

namespace Rocks.Analysis.Discovery;

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
				_.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol, compilation)).ToImmutableArray();

		this.Methods =
			mockType.TypeKind == TypeKind.Interface ?
				MockableMethodDiscovery.GetMethodsForInterface(
					mockType, containingAssemblyOfInvocationSymbol, shims, objectMethods, ref memberIdentifier, compilation) :
				MockableMethodDiscovery.GetMethodsForClass(
					mockType, containingAssemblyOfInvocationSymbol, shims, objectMethods, ref memberIdentifier, compilation);
	}

	private static MockableMethods GetMethodsForClass(ITypeSymbol mockType, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		HashSet<ITypeSymbol> shims, ImmutableArray<IMethodSymbol> objectMethods, ref uint memberIdentifier,
		Compilation compilation)
	{
		var methods = new List<MockableMethodResult>();
		var inaccessibleAbstractMembers = false;

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
					var canBeSeen = hierarchyMethod.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol, compilation);

					if (canBeSeen)
					{
						// This is the case where a class does something like this:
						// `protected static new string ToString()`
						// We can see it, and it's hiding a method from a class in the hierarchy,
						// so we have to remove the one that we currently have in the list.
						// If it "shows up" again in a class lower in hierarchy,
						// we'll just add it again.
						var methodToRemove = methods.SingleOrDefault(_ => !(_.Value.Match(hierarchyMethod, compilation) == MethodMatch.None) &&
							!_.Value.ContainingType.Equals(hierarchyMethod.ContainingType));

						if (methodToRemove is not null)
						{
							methods.Remove(methodToRemove);
						}
					}
					
					if (!hierarchyMethod.IsStatic && (!mockType.IsRecord || hierarchyMethod.Name != nameof(Equals)))
					{
						if (hierarchyMethod.IsAbstract || hierarchyMethod.IsOverride || hierarchyMethod.IsVirtual)
						{
							if (!canBeSeen && hierarchyMethod.IsAbstract)
							{
								inaccessibleAbstractMembers = true;
							}
							else if (canBeSeen)
							{
								if (!hierarchyMethod.IsSealed)
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
							var methodToRemove = methods.SingleOrDefault(_ => !(_.Value.Match(hierarchyMethod, compilation) == MethodMatch.None) &&
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
					hierarchyNonMockable => !(_.Value.Match(hierarchyNonMockable, compilation) == MethodMatch.None)));
			}
		}

		return new MockableMethods([.. methods], inaccessibleAbstractMembers, false);
	}

	private static MockableMethods GetMethodsForInterface(ITypeSymbol mockType, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		HashSet<ITypeSymbol> shims, ImmutableArray<IMethodSymbol> objectMethods, ref uint memberIdentifier,
		Compilation compilation)
	{
		// We don't want to include non-virtual static methods.
		// Later on in MockModel I make a check for static abstract methods.
		static bool IsMethodToExamine(IMethodSymbol method) =>
			!method.IsStatic && (method.IsAbstract || method.IsVirtual) &&
			method.MethodKind == MethodKind.Ordinary && method.CanBeReferencedByName;

		var methods = new List<MockableMethodResult>();
		var inaccessibleAbstractMembers = false;
		var hasStaticAbstractMethods = false;

		foreach (var selfMethod in mockType.GetMembers().OfType<IMethodSymbol>())
		{
			// Report if this method is a static abstract method.
			hasStaticAbstractMethods |= selfMethod.IsStatic && selfMethod.IsAbstract;

			if (IsMethodToExamine(selfMethod))
			{
				if (!selfMethod.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol, compilation) &&
					(selfMethod.IsAbstract || selfMethod.IsVirtual))
				{
					inaccessibleAbstractMembers = true;
				}
				else
				{
					// We need to explicitly implement matching methods from the object type,
					// but if they exactly match from methods on the type itself,
					// we don't even add it to the list.
					if (!methods.Any(_ => _.Value.Match(selfMethod, compilation) == MethodMatch.Exact))
					{
						var selfMethodRequiresExplicit = objectMethods.Any(
							_ => _.Match(selfMethod, compilation) switch
							{
								MethodMatch.DifferByReturnTypeOrConstraintOnly or MethodMatch.Exact => true,
								_ => false
							}) || methods.Any(
							_ => _.Value.Match(selfMethod, compilation) switch
							{
								MethodMatch.DifferByReturnTypeOrConstraintOnly => true,
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
		}

		var baseInterfaceMethodGroups = new List<List<IMethodSymbol>>();

		foreach (var selfBaseInterface in mockType.AllInterfaces)
		{
			foreach (var selfBaseMethod in selfBaseInterface.GetMembers().OfType<IMethodSymbol>())
			{
				// Report if this method is a static abstract method.
				hasStaticAbstractMethods |= selfBaseMethod.IsStatic && selfBaseMethod.IsAbstract;

				if (IsMethodToExamine(selfBaseMethod))
				{
					if (!selfBaseMethod.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol, compilation))
					{
						inaccessibleAbstractMembers = true;
					}
					else
					{
						if (!methods.Any(_ => _.Value.Match(selfBaseMethod, compilation) == MethodMatch.Exact))
						{
							var foundMatch = false;

							foreach (var baseInterfaceMethodGroup in baseInterfaceMethodGroups)
							{
								if (baseInterfaceMethodGroup.Any(_ => _.Match(selfBaseMethod, compilation) == MethodMatch.Exact))
								{
									baseInterfaceMethodGroup.Add(selfBaseMethod);
									foundMatch = true;
									break;
								}
							}

							if (!foundMatch)
							{
								baseInterfaceMethodGroups.Add([selfBaseMethod]);
							}
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
						_ =>
						{
							var match = _.Value.Match(baseInterfaceMethodGroup[0], compilation);
							return match == MethodMatch.DifferByReturnTypeOrConstraintOnly;
						}) ||
						objectMethods.Any(_ => _.Match(baseInterfaceMethodGroup[0], compilation) switch
						{
							MethodMatch.DifferByReturnTypeOrConstraintOnly or MethodMatch.Exact => true,
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

		return new MockableMethods([.. methods], inaccessibleAbstractMembers, hasStaticAbstractMethods);
	}

	internal MockableMethods Methods { get; }
}