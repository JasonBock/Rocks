using Microsoft.CodeAnalysis;
using Rocks.Models;

namespace Rocks.Builders.Create;

internal static class DelegateBuilder
{
	internal static string Build(MethodModel method)
	{
		var parameters = method.Parameters;
		var typeArgumentsNamingContext = method.IsGenericMethod ?
			new TypeArgumentsNamingContext(method) :
			new TypeArgumentsNamingContext();

		if (parameters.Length > 0)
		{
			var parameterTypes = string.Join(", ", parameters.Select(
				_ => $"{(method.TypeArguments.Any(m => m.FullyQualifiedName == _.Type.FullyQualifiedName) ? _.Type.BuildName(typeArgumentsNamingContext) : _.Type.FullyQualifiedName)}{(_.RequiresNullableAnnotation ? "?" : string.Empty)}"));
			return !method.ReturnsVoid ?
				$"global::System.Func<{parameterTypes}, {method.ReturnType.BuildName(typeArgumentsNamingContext)}>" :
				$"global::System.Action<{parameterTypes}>";
		}
		else
		{
			return !method.ReturnsVoid ?
				$"global::System.Func<{method.ReturnType.BuildName(typeArgumentsNamingContext)}>" :
				"global::System.Action";
		}
	}
}