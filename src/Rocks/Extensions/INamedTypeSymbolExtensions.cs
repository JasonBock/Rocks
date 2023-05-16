using Microsoft.CodeAnalysis;

namespace Rocks.Extensions;

internal static class INamedTypeSymbolExtensions
{
   internal static bool HasOpenGenerics(this INamedTypeSymbol self) => 
		self.TypeArguments.Any(_ => _.TypeKind == TypeKind.TypeParameter);
}