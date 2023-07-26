using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class IEventSymbolExtensions
{
	internal static ImmutableArray<Diagnostic> GetObsoleteDiagnostics(
		this IEventSymbol self, INamedTypeSymbol obsoleteAttribute, bool treatWarningsAsErrors)
	{
		var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();

		if (self.GetAttributes().Any(
			_ => _.AttributeClass!.Equals(obsoleteAttribute, SymbolEqualityComparer.Default) &&
				(_.ConstructorArguments.Any(_ => _.Value is bool error && error) || treatWarningsAsErrors)) ||
			self.Type.IsObsolete(obsoleteAttribute, treatWarningsAsErrors))
		{
			diagnostics.Add(MemberIsObsoleteDiagnostic.Create(self));
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