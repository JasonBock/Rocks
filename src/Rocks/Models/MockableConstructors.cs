using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Models;

internal sealed class MockableConstructors
{
   internal MockableConstructors(ITypeSymbol mockType,
	   IAssemblySymbol containingAssemblyOfInvocationSymbol, INamedTypeSymbol obsoleteAttribute) =>
			// We can't use constructors that are obsolete in error.
			this.Constructors = mockType.TypeKind == TypeKind.Class ?
				mockType.GetMembers().OfType<IMethodSymbol>()
					.Where(_ => _.MethodKind == MethodKind.Constructor &&
						_.CanBeSeenByContainingAssembly(containingAssemblyOfInvocationSymbol) &&
						!_.GetAttributes().Any(
							a => a.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
								(a.ConstructorArguments.Any(_ => _.Value is bool error && error)))).ToImmutableArray() :
				Array.Empty<IMethodSymbol>().ToImmutableArray();

   internal ImmutableArray<IMethodSymbol> Constructors { get; }
}