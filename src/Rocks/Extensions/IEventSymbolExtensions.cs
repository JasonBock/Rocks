using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class IEventSymbolExtensions
{
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