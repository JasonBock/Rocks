using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class ExplicitIndexerExpectationsExtensionsIndexerBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, string containingTypeName)
	{
		var property = result.Value;
		var propertyGetMethod = property.GetMethod!;
		var namingContext = new VariableNamingContext(propertyGetMethod);
		var mockTypeName = result.MockType.GetName();
		var thisParameter = $"this global::Rocks.Expectations.ExplicitIndexerGetterExpectations<{mockTypeName}, {containingTypeName}> @{namingContext["self"]}";

		var delegateTypeName = propertyGetMethod.RequiresProjectedDelegate() ?
			propertyGetMethod.ReturnType.IsRefLikeType ?
				MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(propertyGetMethod, result.MockType) :
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, result.MockType) :
			DelegateBuilder.Build(property.Parameters, property.Type);
		var propertyReturnValue = propertyGetMethod.ReturnType.IsRefLikeType ?
			delegateTypeName : propertyGetMethod.ReturnType.GetReferenceableName();
		var adornmentsType = propertyGetMethod.RequiresProjectedDelegate() ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, result.MockType, AdornmentType.Indexer, true)}<{mockTypeName}, {delegateTypeName}>" :
			$"global::Rocks.IndexerAdornments<{mockTypeName}, {delegateTypeName}, {propertyReturnValue}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		var instanceParameters = string.Join(", ", thisParameter,
			string.Join(", ", propertyGetMethod.Parameters.Select(_ =>
			{
				return $"global::Rocks.Argument<{_.Type.GetReferenceableName()}> @{_.Name}";
			})));

		writer.WriteLine($"internal static {returnValue} This({instanceParameters})");
		writer.WriteLine("{");
		writer.Indent++;

		foreach (var parameter in propertyGetMethod.Parameters)
		{
			writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{parameter.Name});");
		}

		var parameters = string.Join(", ", propertyGetMethod.Parameters.Select(
			_ => _.HasExplicitDefaultValue ? $"@{_.Name}.Transform({_.ExplicitDefaultValue.GetDefaultValue(_.Type)})" : $"@{_.Name}"));
		var addMethod = property.Type.IsPointer() ?
			MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodFullyQualifiedName(property.Type, result.MockType) : 
			$"Add<{propertyReturnValue}>";
		writer.WriteLine($"return {newAdornments}(@{namingContext["self"]}.{addMethod}({memberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>({propertyGetMethod.Parameters.Length}) {{ {parameters} }}));");

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, string containingTypeName)
	{
		var property = result.Value;
		var propertySetMethod = property.SetMethod!;
		var namingContext = new VariableNamingContext(propertySetMethod);
		var mockTypeName = result.MockType.GetName();
		var thisParameter = $"this global::Rocks.Expectations.ExplicitIndexerSetterExpectations<{mockTypeName}, {containingTypeName}> @{namingContext["self"]}";

		var delegateTypeName = propertySetMethod.RequiresProjectedDelegate() ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertySetMethod, result.MockType) :
			DelegateBuilder.Build(propertySetMethod.Parameters);
		var adornmentsType = propertySetMethod.RequiresProjectedDelegate() ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, result.MockType, AdornmentType.Indexer, true)}<{mockTypeName}, {delegateTypeName}>" :
			$"global::Rocks.IndexerAdornments<{mockTypeName}, {delegateTypeName}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		var instanceParameters = string.Join(", ", thisParameter,
			string.Join(", ", propertySetMethod.Parameters.Select(_ =>
			{
				return $"global::Rocks.Argument<{_.Type.GetReferenceableName()}> @{_.Name}";
			})));

		writer.WriteLine($"internal static {returnValue} This({instanceParameters})");
		writer.WriteLine("{");
		writer.Indent++;

		foreach (var parameter in propertySetMethod.Parameters)
		{
			writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{parameter.Name});");
		}

		// TODO: This doesn't seem right, the getter has an "add" qualified for projected names.
		var parameters = string.Join(", ", propertySetMethod.Parameters.Select(
			_ => _.HasExplicitDefaultValue ? $"@{_.Name}.Transform({_.ExplicitDefaultValue.GetDefaultValue(_.Type)})" : $"@{_.Name}"));
		writer.WriteLine($"return {newAdornments}({namingContext["self"]}.Add({memberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>({propertySetMethod.Parameters.Length}) {{ {parameters} }}));");

		writer.Indent--;
		writer.WriteLine("}");
	}

	// TODO: This isn't good. I'm passing in a PropertyAccessor value to state 
	// if I should be doing a "get", "set", or "init", but then I also look at the 
	// property's accessor value for the member identifier increment. This
	// doesn't feel "right".
	internal static void Build(IndentedTextWriter writer, PropertyMockableResult result, PropertyAccessor accessor, string containingTypeName)
	{
		var memberIdentifier = result.MemberIdentifier;

		if (accessor == PropertyAccessor.Get)
		{
			ExplicitIndexerExpectationsExtensionsIndexerBuilder.BuildGetter(writer, result, memberIdentifier, containingTypeName);
		}
		else if(accessor == PropertyAccessor.Set)
		{
			if (result.Accessors == PropertyAccessor.GetAndSet)
			{
				memberIdentifier++;
			}

			ExplicitIndexerExpectationsExtensionsIndexerBuilder.BuildSetter(writer, result, memberIdentifier, containingTypeName);
		}
	}
}