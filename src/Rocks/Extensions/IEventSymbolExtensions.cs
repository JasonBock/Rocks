using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class IEventSymbolExtensions
{
	internal static Diagnostic? GetObsoleteDiagnostic(
		this IEventSymbol self, SyntaxNode node, INamedTypeSymbol obsoleteAttribute) =>
			self.Type.IsObsolete(obsoleteAttribute) ? MemberUsesObsoleteTypeDiagnostic.Create(node, self) : null;

	internal static bool CanBeSeenByContainingAssembly(this IEventSymbol self, IAssemblySymbol assembly) =>
		((ISymbol)self).CanBeSeenByContainingAssembly(assembly) &&
			self.Type.CanBeSeenByContainingAssembly(assembly);

   internal static ImmutableHashSet<INamespaceSymbol> GetNamespaces(this IEventSymbol self) => 
		[.. self.Type.GetNamespaces(), .. self.GetAttributes().SelectMany(_ => _.GetNamespaces())];
}