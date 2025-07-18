﻿using Microsoft.CodeAnalysis;
using Rocks.Analysis.Extensions;
using System.Collections.Immutable;

namespace Rocks.Analysis.Discovery;

internal sealed class MockableConstructorDiscovery
{
   internal MockableConstructorDiscovery(ITypeSymbol mockType,
	   IAssemblySymbol containingAssemblyOfInvocationSymbol, INamedTypeSymbol obsoleteAttribute,
		Compilation compilation) =>
			// We can't use constructors that are obsolete in error.
			this.Constructors = mockType.TypeKind == TypeKind.Class ?
				[.. mockType.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => _.MethodKind == MethodKind.Constructor &&
						_.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol, compilation) &&
						!_.GetAttributes().Any(
							a => a.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
								a.ConstructorArguments.Any(_ => _.Value is bool error && error)))] :
				[];

   internal ImmutableArray<IMethodSymbol> Constructors { get; }
}