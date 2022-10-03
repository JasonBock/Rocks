using Microsoft.CodeAnalysis;

namespace Rocks.Extensions;

internal static class IParameterSymbolExtensions
{
	internal static bool RequiresForcedNullableAnnotation(this IParameterSymbol self) =>
		self.HasExplicitDefaultValue && self.ExplicitDefaultValue is null &&
			!self.Type.IsValueType && self.NullableAnnotation != NullableAnnotation.Annotated;
}