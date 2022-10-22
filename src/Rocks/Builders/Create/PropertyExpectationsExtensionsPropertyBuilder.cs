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

		var mockTypeName = result.MockType.GetReferenceableName();
		var thisParameter = $"this global::Rocks.Expectations.PropertyGetterExpectations<{mockTypeName}> @self";
		var callbackDelegateTypeName = propertyGetMethod.RequiresProjectedDelegate() ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, result.MockType) :
			DelegateBuilder.Build(ImmutableArray<IParameterSymbol>.Empty, property.Type);
		var propertyReturnValue = propertyGetMethod.ReturnType.IsRefLikeType ?
			MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(propertyGetMethod, result.MockType) : 
			propertyGetMethod.ReturnType.GetReferenceableName();
		var adornmentsType = propertyGetMethod.ReturnType.IsPointer() ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, result.MockType, AdornmentType.Property, false)}<{mockTypeName}, {callbackDelegateTypeName}>" :
			$"global::Rocks.PropertyAdornments<{mockTypeName}, {callbackDelegateTypeName}, {propertyReturnValue}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		writer.WriteLine($"internal static {returnValue} {property.Name}({thisParameter}) =>");
		writer.Indent++;

		var addMethod = property.Type.IsPointer() ?
			MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodFullyQualifiedName(property.Type, result.MockType) : 
			$"Add<{propertyReturnValue}>";

		writer.WriteLine($"{newAdornments}(@self.{addMethod}({memberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>()));");
		writer.Indent--;
	}

	private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier)
	{
		var property = result.Value;
		var propertyParameterType = property.SetMethod!.Parameters[0].Type;
		var propertyParameterValue = 
			propertyParameterType.IsEsoteric() ?
				propertyParameterType.IsPointer() ?
					PointerArgTypeBuilder.GetProjectedFullyQualifiedName(propertyParameterType, result.MockType) :
					RefLikeArgTypeBuilder.GetProjectedFullyQualifiedName(propertyParameterType, result.MockType) :
			propertyParameterType.GetReferenceableName();
		var mockTypeName = result.MockType.GetReferenceableName();
		var thisParameter = $"this global::Rocks.Expectations.PropertySetterExpectations<{mockTypeName}> @self";
		var delegateTypeName = property.SetMethod!.RequiresProjectedDelegate() ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(property.SetMethod!, result.MockType) :
			DelegateBuilder.Build(property.SetMethod!.Parameters);
		var adornmentsType = propertyParameterType.IsPointer() ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, result.MockType, AdornmentType.Property, false)}<{mockTypeName}, {delegateTypeName}>" :
			$"global::Rocks.PropertyAdornments<{mockTypeName}, {delegateTypeName}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		writer.WriteLine($"internal static {returnValue} {property.Name}({thisParameter}, global::Rocks.Argument<{propertyParameterValue}> @value) =>");
		writer.Indent++;

		writer.WriteLine($"{newAdornments}(@self.Add({memberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) {{ @value }}));");
		writer.Indent--;
	}

	// TODO: This isn't good. I'm passing in a PropertyAccessor value to state 
	// if I should be doing a "get", "set", or "init", but then I also look at the 
	// property's accessor value for the member identifier increment. This
	// doesn't feel "right".
	internal static void Build(IndentedTextWriter writer, PropertyMockableResult result, IAssemblySymbol typeToMockContainingAssembly, PropertyAccessor accessor)
	{
		var memberIdentifier = result.MemberIdentifier;

		if (accessor == PropertyAccessor.Get &&
			result.Value.GetMethod!.CanBeSeenByContainingAssembly(typeToMockContainingAssembly))
		{
			PropertyExpectationsExtensionsPropertyBuilder.BuildGetter(writer, result, memberIdentifier);
		}
		else if(accessor == PropertyAccessor.Set &&
			result.Value.SetMethod!.CanBeSeenByContainingAssembly(typeToMockContainingAssembly))
		{
			if (result.Accessors == PropertyAccessor.GetAndSet)
			{
				memberIdentifier++;
			}

			PropertyExpectationsExtensionsPropertyBuilder.BuildSetter(writer, result, memberIdentifier);
		}
	}
}