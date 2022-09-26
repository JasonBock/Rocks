using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class IndexerExpectationsExtensionsIndexerBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier)
	{
		var property = result.Value;
		var propertyGetMethod = property.GetMethod!;

		var mockTypeName = result.MockType.GetReferenceableName();
		var thisParameter = $"this global::Rocks.Expectations.IndexerGetterExpectations<{mockTypeName}> self";

		var delegateTypeName = propertyGetMethod.RequiresProjectedDelegate() ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateName(propertyGetMethod) :
			DelegateBuilder.Build(property.Parameters, property.Type);
		var propertyReturnValue = propertyGetMethod.ReturnType.IsRefLikeType ?
			MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateName(propertyGetMethod) : 
			propertyGetMethod.ReturnType.GetReferenceableName();
		var adornmentsType = propertyGetMethod.ReturnType.IsPointer() ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(property.Type, AdornmentType.Indexer, false)}<{mockTypeName}, {delegateTypeName}>" :
			$"global::Rocks.IndexerAdornments<{mockTypeName}, {delegateTypeName}, {propertyReturnValue}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		var instanceParameters = string.Join(", ", thisParameter,
			string.Join(", ", propertyGetMethod.Parameters.Select(_ =>
			{
				return $"global::Rocks.Argument<{_.Type.GetReferenceableName()}> {_.Name}";
			})));

		writer.WriteLine($"internal static {returnValue} This({instanceParameters})");
		writer.WriteLine("{");
		writer.Indent++;

		foreach (var parameter in propertyGetMethod.Parameters)
		{
			writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull({parameter.Name});");
		}

		var parameters = string.Join(", ", propertyGetMethod.Parameters.Select(
			_ => _.HasExplicitDefaultValue ? $"{_.Name}.Transform({_.ExplicitDefaultValue.GetDefaultValue(_.Type.IsValueType)})" : _.Name));
		var addMethod = property.Type.IsPointer() ?
			MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodName(property.Type) : 
			$"Add<{propertyReturnValue}>";
		writer.WriteLine($"return {newAdornments}(self.{addMethod}({memberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>({propertyGetMethod.Parameters.Length}) {{ {parameters} }}));");

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier)
	{
		var property = result.Value;
		var mockTypeName = result.MockType.GetReferenceableName();
		var thisParameter = $"this global::Rocks.Expectations.IndexerSetterExpectations<{mockTypeName}> self";
		var delegateTypeName = property.SetMethod!.RequiresProjectedDelegate() ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateName(property.SetMethod!) :
			DelegateBuilder.Build(property.SetMethod!.Parameters);
		var adornmentsType = property.SetMethod!.RequiresProjectedDelegate() ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(property.Type, AdornmentType.Indexer, false)}<{mockTypeName}, {delegateTypeName}>" :
			$"global::Rocks.IndexerAdornments<{mockTypeName}, {delegateTypeName}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		var instanceParameters = string.Join(", ", thisParameter,
			string.Join(", ", property.SetMethod!.Parameters.Select(_ =>
			{
				return $"global::Rocks.Argument<{_.Type.GetReferenceableName()}> {_.Name}";
			})));

		writer.WriteLine($"internal static {returnValue} This({instanceParameters})");
		writer.WriteLine("{");
		writer.Indent++;

		foreach (var parameter in property.SetMethod!.Parameters)
		{
			writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull({parameter.Name});");
		}

		// TODO: This doesn't seem right, the getter has an "add" qualified for projected names.
		var parameters = string.Join(", ", property.SetMethod!.Parameters.Select(
			_ => _.HasExplicitDefaultValue ? $"{_.Name}.Transform({_.ExplicitDefaultValue.GetDefaultValue(_.Type.IsValueType)})" : _.Name));
		writer.WriteLine($"return {newAdornments}(self.Add({memberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>({property.SetMethod!.Parameters.Length}) {{ {parameters} }}));");

		writer.Indent--;
		writer.WriteLine("}");
	}

	// TODO: This isn't good. I'm passing in a PropertyAccessor value to state 
	// if I should be doing a "get", "set", or "init", but then I also look at the 
	// property's accessor value for the member identifier increment. This
	// doesn't feel "right".
	internal static void Build(IndentedTextWriter writer, PropertyMockableResult result, PropertyAccessor accessor)
	{
		var memberIdentifier = result.MemberIdentifier;

		if (accessor == PropertyAccessor.Get)
		{
			IndexerExpectationsExtensionsIndexerBuilder.BuildGetter(writer, result, memberIdentifier);
		}
		else if(accessor == PropertyAccessor.Set)
		{
			if (result.Accessors == PropertyAccessor.GetAndSet)
			{
				memberIdentifier++;
			}

			IndexerExpectationsExtensionsIndexerBuilder.BuildSetter(writer, result, memberIdentifier);
		}
	}
}