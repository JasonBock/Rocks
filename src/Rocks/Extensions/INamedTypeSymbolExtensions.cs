using Microsoft.CodeAnalysis;
using Rocks.Models;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class INamedTypeSymbolExtensions
{
	internal static EquatableArray<Constraints> GetConstraints(this INamedTypeSymbol self, Compilation compilation)
	{
		var constraints = new List<Constraints>();

		if (self.TypeParameters.Length > 0)
		{
			for (var i = 0; i < self.TypeParameters.Length; i++)
			{
				var typeParameter = self.TypeParameters[i];

				if (typeParameter.Equals(self.TypeArguments[i]))
				{
					var constraint = typeParameter.GetConstraints(compilation);

					if(constraint is not null)
					{
						constraints.Add(constraint);
					}
				}
			}
		}

		return constraints.ToImmutableArray();
	}

	internal static bool HasOpenGenerics(this INamedTypeSymbol self) => 
		self.TypeArguments.Any(_ => _.TypeKind == TypeKind.TypeParameter ||
			_ is INamedTypeSymbol argumentTypeSymbol && argumentTypeSymbol.HasOpenGenerics());
}