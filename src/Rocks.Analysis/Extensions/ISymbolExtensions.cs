using Microsoft.CodeAnalysis;
using System.ComponentModel;

namespace Rocks.Analysis.Extensions;

internal static class ISymbolExtensions
{
	internal static string GetAccessibilityValue(this ISymbol self, IAssemblySymbol compilationAssembly) =>
		self.DeclaredAccessibility switch
		{
			Accessibility.Public => "public",
			Accessibility.Private => "private",
			Accessibility.Protected => "protected",
			Accessibility.Internal => "internal",
			Accessibility.ProtectedOrInternal => SymbolEqualityComparer.Default.Equals(self.ContainingAssembly, compilationAssembly) ? "protected internal" : "protected",
			Accessibility.ProtectedAndInternal => "private protected",
			_ => throw new InvalidEnumArgumentException(nameof(self.DeclaredAccessibility), (int)self.DeclaredAccessibility, typeof(Accessibility))
		};

	internal static bool CanBeSeenByContainingAssembly(this ISymbol self, IAssemblySymbol containingAssemblyOfInvocationSymbol,
		Compilation compilation)
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
				self.ContainingAssembly.ExposesInternalsTo(containingAssemblyOfInvocationSymbol, compilation);
		}
		else
		{
			return false;
		}
	}
}