using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Extensions
{
	internal static class IEventSymbolExtensions
	{
		internal static ImmutableHashSet<INamespaceSymbol> GetNamespaces(this IEventSymbol self)
		{
			var namespaces = ImmutableHashSet.CreateBuilder<INamespaceSymbol>();

			namespaces.Add(self.Type.ContainingNamespace);
			namespaces.AddRange(self.Type.GetAttributes().SelectMany(_ => _.GetNamespaces()));

			return namespaces.ToImmutable();
		}
	}
}