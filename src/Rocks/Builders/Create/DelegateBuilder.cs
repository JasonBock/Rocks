using Microsoft.CodeAnalysis;
using Rocks.Models;

namespace Rocks.Builders.Create;

internal static class DelegateBuilder
{
	internal static string Build(EquatableArray<ParameterModel> parameters, TypeReferenceModel? returnType = null)
	{
		if (parameters.Length > 0)
		{
			var parameterTypes = string.Join(", ", parameters.Select(
				_ => $"{_.Type.FullyQualifiedName}{(_.RequiresNullableAnnotation ? "?" : string.Empty)}"));
			return returnType is not null ?
				$"global::System.Func<{parameterTypes}, {returnType.FullyQualifiedName}>" :
				$"global::System.Action<{parameterTypes}>";
		}
		else
		{
			return returnType is not null ?
				$"global::System.Func<{returnType.FullyQualifiedName}>" :
				"global::System.Action";
		}
	}
}