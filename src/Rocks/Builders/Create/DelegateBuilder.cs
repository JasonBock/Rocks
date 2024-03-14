﻿using Microsoft.CodeAnalysis;
using Rocks.Models;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class DelegateBuilder
{
	internal static string Build(MethodModel method)
	{
		var parameters = method.Parameters;
		var typeArgumentNamingContext = method.IsGenericMethod ?
			new VariableNamingContext(method.MockType.TypeArguments.ToImmutableHashSet()) :
			new VariableNamingContext();

		if (parameters.Length > 0)
		{
			var parameterTypes = string.Join(", ", parameters.Select(
				_ => $"{typeArgumentNamingContext[_.Type.FullyQualifiedName]}{(_.RequiresNullableAnnotation ? "?" : string.Empty)}"));
			return !method.ReturnsVoid ?
				$"global::System.Func<{parameterTypes}, {typeArgumentNamingContext[method.ReturnType.FullyQualifiedName]}>" :
				$"global::System.Action<{parameterTypes}>";
		}
		else
		{
			return !method.ReturnsVoid ?
				$"global::System.Func<{typeArgumentNamingContext[method.ReturnType.FullyQualifiedName]}>" :
				"global::System.Action";
		}
	}
}