using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;

namespace Rocks.Analysis.Extensions;

internal static class IEventSymbolExtensions
{
	internal static Diagnostic? GetObsoleteDiagnostic(
		this IEventSymbol self, SyntaxNode node, INamedTypeSymbol obsoleteAttribute) =>
			self.Type.IsObsolete(obsoleteAttribute) ? MemberUsesObsoleteTypeDiagnostic.Create(node, self) : null;

	internal static bool CanBeSeenByContainingAssembly(this IEventSymbol self, IAssemblySymbol assembly,
		Compilation compilation) =>
		((ISymbol)self).CanBeSeenByContainingAssembly(assembly, compilation) &&
			self.Type.CanBeSeenByContainingAssembly(assembly, compilation);
}