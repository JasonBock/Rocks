using Microsoft.CodeAnalysis;

namespace Rocks.Extensions;

internal static class IParameterSymbolExtensions
{
	internal static bool IsScoped(this IParameterSymbol self) =>
		(self.Type.IsRefLikeType && !self.IsParams && self.ScopedKind == ScopedKind.ScopedValue) ||
		(self.Type.IsValueType && self.RefKind == RefKind.Ref && self.ScopedKind == ScopedKind.ScopedRef);

	internal static bool RequiresForcedNullableAnnotation(this IParameterSymbol self) =>
		self.HasExplicitDefaultValue && self.ExplicitDefaultValue is null &&
			self.Type.TypeKind != TypeKind.TypeParameter &&
			!self.Type.IsValueType && self.NullableAnnotation != NullableAnnotation.Annotated;
}