using Microsoft.CodeAnalysis;
using System.ComponentModel;

namespace Rocks.Extensions;

internal static class AccessibilityExtensions
{
	internal static string GetOverridingCodeValue(this Accessibility self) =>
		self switch
		{
			Accessibility.Public => "public",
			Accessibility.Private => "private",
			Accessibility.Protected => "protected",
			Accessibility.Internal => "internal",
			Accessibility.ProtectedOrInternal => "protected",
			Accessibility.ProtectedAndInternal => "private protected",
			_ => throw new InvalidEnumArgumentException(nameof(self), (int)self, typeof(Accessibility))
		};
}