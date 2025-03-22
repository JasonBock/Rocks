using Microsoft.CodeAnalysis;
using Rocks.Models;

namespace Rocks.Builders.Create;

internal static class MockProjectedDelegateBuilder
{
	internal static string GetProjectedCallbackDelegateFullyQualifiedName(
		MethodModel method, ITypeReferenceModel typeToMock, string expectationsFullyQualifiedName, uint memberIdentifier)
	{
		var typeArgumentsNamingContext = method.IsGenericMethod ?
			new TypeArgumentsNamingContext(method) :
			new TypeArgumentsNamingContext();

		var methodArguments = method.IsGenericMethod ?
			$"<{string.Join(", ", method.TypeArguments.Select(_ => _.BuildName(typeArgumentsNamingContext)))}>" : string.Empty;

		return $"{expectationsFullyQualifiedName}.Handler{memberIdentifier}{methodArguments}.CallbackForHandler";
	}

	internal static string GetProjectedDelegate(MethodModel method)
	{
		var typeArgumentsNamingContext = method.IsGenericMethod ?
			new TypeArgumentsNamingContext(method) :
			new TypeArgumentsNamingContext();

		var returnType = method.ReturnType.BuildName(typeArgumentsNamingContext);
		var methodParameters = string.Join(", ", method.Parameters.Select(_ =>
		{
			var defaultValue = _.HasExplicitDefaultValue && method.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ?
				$" = {_.ExplicitDefaultValue}" : string.Empty;
			var scoped = _.IsParams ? string.Empty :
				_.IsScoped ? "scoped " : string.Empty;
			var direction = _.RefKind == RefKind.Ref ? "ref " : _.RefKind == RefKind.Out ? "out " : string.Empty;
			var parameter = $"{scoped}{direction}{(_.IsParams ? "params " : string.Empty)}{_.Type.BuildName(typeArgumentsNamingContext)} @{_.Name}{defaultValue}";
			return $"{_.AttributesDescription}{parameter}";
		}));
		var isUnsafe = method.IsUnsafe ? "unsafe " : string.Empty;

		return $"internal {isUnsafe}delegate {returnType} CallbackForHandler({methodParameters});";
	}
}