using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class ExplicitIndexerExpectationsExtensionsIndexerBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, MockInformation information, PropertyMockableResult result, uint memberIdentifier, string containingTypeName)
	{
		var property = result.Value;
		var propertyReturnValue = property.Type.GetName();
		var mockTypeName = result.MockType.GetName();
		var thisTypeName = $"{WellKnownNames.Explicit}{WellKnownNames.Indexer}{WellKnownNames.Getter}{WellKnownNames.Expectations}";
		var thisParameter = $"this {thisTypeName}<{mockTypeName}, {containingTypeName}> self";

		var delegateTypeName = property.GetMethod!.RequiresProjectedDelegate() ?
			$"ProjectionsFor{information.TypeToMock!.FlattenedName}.{MockProjectedDelegateBuilder.GetProjectedDelegateName(property.GetMethod!)}" :
			DelegateBuilder.Build(property.Parameters, property.Type);
		var adornmentsType = property.GetMethod!.RequiresProjectedDelegate() ?
			$"ProjectionsFor{information.TypeToMock!.FlattenedName}.{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(property.Type, AdornmentType.Indexer, true)}<{mockTypeName}, {delegateTypeName}>" :
			$"{WellKnownNames.Indexer}{WellKnownNames.Adornments}<{mockTypeName}, {delegateTypeName}, {propertyReturnValue}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		var instanceParameters = string.Join(", ", thisParameter,
			string.Join(", ", property.GetMethod!.Parameters.Select(_ =>
			{
				return $"{nameof(Argument)}<{_.Type.GetName()}> {_.Name}";
			})));

		writer.WriteLine($"internal static {returnValue} {WellKnownNames.This}({instanceParameters}) =>");
		writer.Indent++;

		var parameters = string.Join(", ", property.GetMethod!.Parameters.Select(
			_ => _.HasExplicitDefaultValue ? $"{_.Name}.{WellKnownNames.Transform}({_.ExplicitDefaultValue.GetDefaultValue(_.Type.IsValueType)})" : _.Name));
		var addMethod = property.Type.IsEsoteric() ?
			MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodName(property.Type) : $"Add<{propertyReturnValue}>";
		writer.WriteLine($"{newAdornments}(self.{addMethod}({memberIdentifier}, new List<{nameof(Argument)}>({property.GetMethod!.Parameters.Length}) {{ {parameters} }}));");
		writer.Indent--;
	}

	private static void BuildSetter(IndentedTextWriter writer, MockInformation information, PropertyMockableResult result, uint memberIdentifier, string containingTypeName)
	{
		var property = result.Value;
		var mockTypeName = result.MockType.GetName();
		var thisTypeName = $"{WellKnownNames.Explicit}{WellKnownNames.Indexer}{WellKnownNames.Setter}{WellKnownNames.Expectations}";
		var thisParameter = $"this {thisTypeName}<{mockTypeName}, {containingTypeName}> self";

		var delegateTypeName = property.SetMethod!.RequiresProjectedDelegate() ?
			$"ProjectionsFor{information.TypeToMock!.FlattenedName}.{MockProjectedDelegateBuilder.GetProjectedDelegateName(property.SetMethod!)}" :
			DelegateBuilder.Build(property.SetMethod!.Parameters);
		var adornmentsType = property.SetMethod!.RequiresProjectedDelegate() ?
			$"ProjectionsFor{information.TypeToMock!.FlattenedName}.{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(property.Type, AdornmentType.Indexer, true)}<{mockTypeName}, {delegateTypeName}>" :
			$"{WellKnownNames.Indexer}{WellKnownNames.Adornments}<{mockTypeName}, {delegateTypeName}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		var instanceParameters = string.Join(", ", thisParameter,
			string.Join(", ", property.SetMethod!.Parameters.Select(_ =>
			{
				return $"{nameof(Argument)}<{_.Type.GetName()}> {_.Name}";
			})));

		writer.WriteLine($"internal static {returnValue} {WellKnownNames.This}({instanceParameters}) =>");
		writer.Indent++;

		var parameters = string.Join(", ", property.SetMethod!.Parameters.Select(
			_ => _.HasExplicitDefaultValue ? $"{_.Name}.{WellKnownNames.Transform}({_.ExplicitDefaultValue.GetDefaultValue(_.Type.IsValueType)})" : _.Name));
		writer.WriteLine($"{newAdornments}(self.Add({memberIdentifier}, new List<{nameof(Argument)}>({property.SetMethod!.Parameters.Length}) {{ {parameters} }}));");
		writer.Indent--;
	}

	// TODO: This isn't good. I'm passing in a PropertyAccessor value to state 
	// if I should be doing a "get", "set", or "init", but then I also look at the 
	// property's accessor value for the member identifier increment. This
	// doesn't feel "right".
	internal static void Build(IndentedTextWriter writer, MockInformation information, PropertyMockableResult result, PropertyAccessor accessor, string containingTypeName)
	{
		var memberIdentifier = result.MemberIdentifier;

		if (accessor == PropertyAccessor.Get)
		{
			ExplicitIndexerExpectationsExtensionsIndexerBuilder.BuildGetter(writer, information, result, memberIdentifier, containingTypeName);
		}
		else if(accessor == PropertyAccessor.Set)
		{
			if (result.Accessors == PropertyAccessor.GetAndSet)
			{
				memberIdentifier++;
			}

			ExplicitIndexerExpectationsExtensionsIndexerBuilder.BuildSetter(writer, information, result, memberIdentifier, containingTypeName);
		}
	}
}