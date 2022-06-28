using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class PropertyExpectationsExtensionsPropertyBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier)
	{
		var property = result.Value;
		var propertyGetMethod = property.GetMethod!;

		var mockTypeName = result.MockType.GetName();
		var thisParameter = $"this {WellKnownNames.Property}{WellKnownNames.Getter}{WellKnownNames.Expectations}<{mockTypeName}> self";
		var callbackDelegateTypeName = propertyGetMethod.RequiresProjectedDelegate() ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateName(propertyGetMethod) :
			DelegateBuilder.Build(ImmutableArray<IParameterSymbol>.Empty, property.Type);
		var propertyReturnValue = propertyGetMethod.ReturnType.IsRefLikeType ?
			MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateName(propertyGetMethod) : 
			propertyGetMethod.ReturnType.GetReferenceableName();
		var adornmentsType = propertyGetMethod.ReturnType.IsPointer() ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(property.Type, AdornmentType.Property, false)}<{mockTypeName}, {callbackDelegateTypeName}>" :
			$"{WellKnownNames.Property}{WellKnownNames.Adornments}<{mockTypeName}, {callbackDelegateTypeName}, {propertyReturnValue}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		writer.WriteLine($"internal static {returnValue} {property.Name}({thisParameter}) =>");
		writer.Indent++;

		var addMethod = property.Type.IsPointer() ?
			MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodName(property.Type) : 
			$"Add<{propertyReturnValue}>";

		writer.WriteLine($"{newAdornments}(self.{addMethod}({memberIdentifier}, new List<{nameof(Argument)}>()));");
		writer.Indent--;
	}

	private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier)
	{
		var property = result.Value;
		var propertyParameterType = property.SetMethod!.Parameters[0].Type;
		var propertyParameterValue = 
			propertyParameterType.IsEsoteric() ?
				propertyParameterType.IsPointer() ?
					PointerArgTypeBuilder.GetProjectedName(propertyParameterType) :
					RefLikeArgTypeBuilder.GetProjectedName(propertyParameterType) :
			propertyParameterType.GetReferenceableName();
		var mockTypeName = result.MockType.GetName();
		var thisParameter = $"this {WellKnownNames.Property}{WellKnownNames.Setter}{WellKnownNames.Expectations}<{mockTypeName}> self";
		var delegateTypeName = property.SetMethod!.RequiresProjectedDelegate() ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateName(property.SetMethod!) :
			DelegateBuilder.Build(property.SetMethod!.Parameters);
		var adornmentsType = propertyParameterType.IsPointer() ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentName(property.Type, AdornmentType.Property, false)}<{mockTypeName}, {delegateTypeName}>" :
			$"{WellKnownNames.Property}{WellKnownNames.Adornments}<{mockTypeName}, {delegateTypeName}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		writer.WriteLine($"internal static {returnValue} {property.Name}({thisParameter}, {nameof(Argument)}<{propertyParameterValue}> value) =>");
		writer.Indent++;

		writer.WriteLine($"{newAdornments}(self.Add({memberIdentifier}, new List<{nameof(Argument)}>(1) {{ value }}));");
		writer.Indent--;
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
			PropertyExpectationsExtensionsPropertyBuilder.BuildGetter(writer, result, memberIdentifier);
		}
		else if(accessor == PropertyAccessor.Set)
		{
			if (result.Accessors == PropertyAccessor.GetAndSet)
			{
				memberIdentifier++;
			}

			PropertyExpectationsExtensionsPropertyBuilder.BuildSetter(writer, result, memberIdentifier);
		}
	}
}