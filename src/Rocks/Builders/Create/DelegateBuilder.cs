using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class DelegateBuilder
{
	internal static string Build(ImmutableArray<IParameterSymbol> parameters, ITypeSymbol? returnType = null)
	{
		if (parameters.Length > 0)
		{
			var parameterTypes = string.Join(", ", parameters.Select(
				_ => $"{_.Type.GetFullyQualifiedName()}{(_.RequiresForcedNullableAnnotation() ? "?" : string.Empty)}"));
			return returnType is not null ?
				$"global::System.Func<{parameterTypes}, {returnType.GetFullyQualifiedName()}>" :
				$"global::System.Action<{parameterTypes}>";
		}
		else
		{
			return returnType is not null ?
				$"global::System.Func<{returnType.GetFullyQualifiedName()}>" :
				"global::System.Action";
		}
	}
}