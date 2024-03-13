using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class INamedTypeSymbolExtensions
{
	internal static ImmutableArray<string> GetConstraints(this INamedTypeSymbol self, Compilation compilation)
	{
		var constraints = new List<string>();

		if (self.TypeParameters.Length > 0)
		{
			for (var i = 0; i < self.TypeParameters.Length; i++)
			{
				var typeParameter = self.TypeParameters[i];

				if (typeParameter.Equals(self.TypeArguments[i]))
				{
					constraints.Add(typeParameter.GetConstraints(compilation));
				}
			}
		}

		return constraints.Where(_ => !string.IsNullOrWhiteSpace(_)).ToImmutableArray();
	}

	internal static bool HasOpenGenerics(this INamedTypeSymbol self) => 
		self.TypeArguments.Any(_ => _.TypeKind == TypeKind.TypeParameter ||
			_ is INamedTypeSymbol argumentTypeSymbol && argumentTypeSymbol.HasOpenGenerics());
}