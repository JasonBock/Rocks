using Microsoft.CodeAnalysis;

namespace Rocks.Extensions;

internal static class ISymbolExtensions
{
	internal static bool CanBeSeenByContainingAssembly(this ISymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol)
	{
		/*
		protected internal -> ProtectedOrInternal
		private protected -> ProtectedAndInternal
		*/
		if (self.DeclaredAccessibility == Accessibility.Public ||
			self.DeclaredAccessibility == Accessibility.Protected ||
			self.DeclaredAccessibility == Accessibility.ProtectedOrInternal)
		{
			return true;
		}
		else if (self.DeclaredAccessibility == Accessibility.Internal ||
			self.DeclaredAccessibility == Accessibility.ProtectedAndInternal)
		{
			return self.ContainingAssembly.Equals(containingAssemblyOfInvocationSymbol, SymbolEqualityComparer.Default) ||
				self.ContainingAssembly.ExposesInternalsTo(containingAssemblyOfInvocationSymbol);
		}
		else
		{
			return false;
		}
	}
}