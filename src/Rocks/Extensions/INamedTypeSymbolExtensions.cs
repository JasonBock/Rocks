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

	/// <summary>
	/// This is needed because if a type has a generic parameter that is used 
	/// either with a method parameter or the return value and declares the type to be nullable,
	/// the override must create a <c>where T : default</c> constraint.
	/// Otherwise, CS0508 and CS0453 will occur.
	/// </summary>
	/// <param name="self">The target method.</param>
	/// <returns>A list of default constraints.</returns>
	internal static ImmutableArray<string> GetDefaultConstraints(this INamedTypeSymbol self)
	{
		var builder = ImmutableArray.CreateBuilder<string>();

		if (self.TypeParameters.Length > 0)
		{
			foreach (var typeParameter in self.TypeParameters)
			{
				// TODO: This is starting to get convoluted.
				// Arguably, it would be good to have one method that gets
				// all constraints for a method, rather than doing it via
				// GetConstraints() and GetDefaultConstraints().
				if (typeParameter.HasReferenceTypeConstraint)
				{
					builder.Add($"where {typeParameter.GetName()} : class");
				}
				else if (typeParameter.HasValueTypeConstraint)
				{
					builder.Add($"where {typeParameter.GetName()} : struct");
				}
			}
		}

		return builder.ToImmutable();
	}

	internal static bool HasOpenGenerics(this INamedTypeSymbol self) => 
		self.TypeArguments.Any(_ => _.TypeKind == TypeKind.TypeParameter ||
			_ is INamedTypeSymbol argumentTypeSymbol && argumentTypeSymbol.HasOpenGenerics());
}