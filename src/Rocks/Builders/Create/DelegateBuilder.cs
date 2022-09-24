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
			var parameterTypes = string.Join(", ", parameters.Select(_ => _.Type.GetReferenceableName()));
			return returnType is not null ?
				$"global::System.{WellKnownNames.Func}<{parameterTypes}, {returnType.GetReferenceableName()}>" :
				$"global::System.{WellKnownNames.Action}<{parameterTypes}>";
		}
		else
		{
			return returnType is not null ?
				$"global::System.{WellKnownNames.Func}<{returnType.GetReferenceableName()}>" :
				$"global::System.{WellKnownNames.Action}";
		}
	}
}