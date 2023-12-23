using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class IEventSymbolExtensions
{
	internal static ImmutableArray<Diagnostic> GetObsoleteDiagnostics(
		this IEventSymbol self, SyntaxNode node, INamedTypeSymbol obsoleteAttribute)
	{
		var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();

		if (self.Type.IsObsolete(obsoleteAttribute))
		{
			diagnostics.Add(MemberUsesObsoleteTypeDiagnostic.Create(node, self));
		}

		return diagnostics.ToImmutable();
	}

	internal static bool CanBeSeenByContainingAssembly(this IEventSymbol self, IAssemblySymbol assembly) =>
		((ISymbol)self).CanBeSeenByContainingAssembly(assembly) &&
			self.Type.CanBeSeenByContainingAssembly(assembly);

	internal static ImmutableHashSet<INamespaceSymbol> GetNamespaces(this IEventSymbol self)
	{
		var namespaces = ImmutableHashSet.CreateBuilder<INamespaceSymbol>();

		namespaces.AddRange(self.Type.GetNamespaces());
		namespaces.AddRange(self.GetAttributes().SelectMany(_ => _.GetNamespaces()));

		return namespaces.ToImmutable();
	}
}