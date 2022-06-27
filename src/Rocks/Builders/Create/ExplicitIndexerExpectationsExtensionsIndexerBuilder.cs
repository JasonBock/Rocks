using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class ExplicitIndexerExpectationsExtensionsIndexerBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, string containingTypeName)
	{
		var property = result.Value;
		var propertyGetMethod = property.GetMethod!;

		var mockTypeName = result.MockType.GetName();
		var thisTypeName = $"{WellKnownNames.Explicit}{WellKnownNames.Indexer}{WellKnownNames.Getter}{WellKnownNames.Expectations}";
		var thisParameter = $"this {thisTypeName}<{mockTypeName}, {containingTypeName}> self";

		var delegateTypeName = propertyGetMethod.RequiresProjectedDelegate() ?
			propertyGetMethod.ReturnType.IsRefLikeType ?
				MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateName(propertyGetMethod) :
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateName(propertyGetMethod) :
			DelegateBuilder.Build(property.Parameters, property.Type);
		var propertyReturnValue = propertyGetMethod.ReturnType.IsRefLikeType ?
			delegateTypeName : propertyGetMethod.ReturnType.GetReferenceableName();
		var adornmentsType = propertyGetMethod.RequiresProjectedDelegate() ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(property.Type, AdornmentType.Indexer, true)}<{mockTypeName}, {delegateTypeName}>" :
			$"{WellKnownNames.Indexer}{WellKnownNames.Adornments}<{mockTypeName}, {delegateTypeName}, {propertyReturnValue}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		var instanceParameters = string.Join(", ", thisParameter,
			string.Join(", ", propertyGetMethod.Parameters.Select(_ =>
			{
				return $"{nameof(Argument)}<{_.Type.GetReferenceableName()}> {_.Name}";
			})));

		writer.WriteLine($"internal static {returnValue} {WellKnownNames.This}({instanceParameters}) =>");
		writer.Indent++;

		var parameters = string.Join(", ", propertyGetMethod.Parameters.Select(
			_ => _.HasExplicitDefaultValue ? $"{_.Name}.{WellKnownNames.Transform}({_.ExplicitDefaultValue.GetDefaultValue(_.Type.IsValueType)})" : _.Name));
		var addMethod = property.Type.IsPointer() ?
			MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodName(property.Type) : 
			$"Add<{propertyReturnValue}>";
		writer.WriteLine($"{newAdornments}(self.{addMethod}({memberIdentifier}, new List<{nameof(Argument)}>({propertyGetMethod.Parameters.Length}) {{ {parameters} }}));");
		writer.Indent--;
	}

	private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, string containingTypeName)
	{
		var property = result.Value;
		var mockTypeName = result.MockType.GetName();
		var thisTypeName = $"{WellKnownNames.Explicit}{WellKnownNames.Indexer}{WellKnownNames.Setter}{WellKnownNames.Expectations}";
		var thisParameter = $"this {thisTypeName}<{mockTypeName}, {containingTypeName}> self";

		var delegateTypeName = property.SetMethod!.RequiresProjectedDelegate() ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateName(property.SetMethod!) :
			DelegateBuilder.Build(property.SetMethod!.Parameters);
		var adornmentsType = property.SetMethod!.RequiresProjectedDelegate() ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(property.Type, AdornmentType.Indexer, true)}<{mockTypeName}, {delegateTypeName}>" :
			$"{WellKnownNames.Indexer}{WellKnownNames.Adornments}<{mockTypeName}, {delegateTypeName}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		var instanceParameters = string.Join(", ", thisParameter,
			string.Join(", ", property.SetMethod!.Parameters.Select(_ =>
			{
				return $"{nameof(Argument)}<{_.Type.GetReferenceableName()}> {_.Name}";
			})));

		writer.WriteLine($"internal static {returnValue} {WellKnownNames.This}({instanceParameters}) =>");
		writer.Indent++;

		// TODO: This doesn't seem right, the getter has an "add" qualified for projected names.
		var parameters = string.Join(", ", property.SetMethod!.Parameters.Select(
			_ => _.HasExplicitDefaultValue ? $"{_.Name}.{WellKnownNames.Transform}({_.ExplicitDefaultValue.GetDefaultValue(_.Type.IsValueType)})" : _.Name));
		writer.WriteLine($"{newAdornments}(self.Add({memberIdentifier}, new List<{nameof(Argument)}>({property.SetMethod!.Parameters.Length}) {{ {parameters} }}));");
		writer.Indent--;
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