using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class ExplicitPropertyExpectationsExtensionsPropertyBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, string containingTypeName)
	{
		var property = result.Value;
		var propertyGetMethod = property.GetMethod!;

		var thisParameter = $"this global::Rocks.Expectations.ExplicitPropertyGetterExpectations<{result.MockType.GetFullyQualifiedName()}, {containingTypeName}> @self";
		var mockTypeName = result.MockType.GetName();

		var delegateTypeName = propertyGetMethod.RequiresProjectedDelegate() ?
			propertyGetMethod.ReturnType.IsRefLikeType ?
				MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(propertyGetMethod, result.MockType) :
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, result.MockType) :
			DelegateBuilder.Build(ImmutableArray<IParameterSymbol>.Empty, property.Type);
		var propertyReturnValue = propertyGetMethod.ReturnType.IsRefLikeType ?
			delegateTypeName : propertyGetMethod.ReturnType.GetFullyQualifiedName();
		var adornmentsType = property.Type.IsEsoteric() ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, result.MockType, AdornmentType.Property, true)}<{mockTypeName}, {delegateTypeName}>" :
			$"global::Rocks.PropertyAdornments<{mockTypeName}, {delegateTypeName}, {propertyReturnValue}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		writer.WriteLine($"internal static {returnValue} {property.Name}({thisParameter}) =>");
		writer.Indent++;

		var addMethod = property.Type.IsPointer() ?
			MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodFullyQualifiedName(property.Type, result.MockType) : 
			$"Add<{propertyReturnValue}>";

		writer.WriteLine($"{newAdornments}(@self.{addMethod}({memberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>()));");
		writer.Indent--;
	}

	private static void BuildSetter(IndentedTextWriter writer, PropertyMockableResult result, uint memberIdentifier, string containingTypeName)
	{
		var property = result.Value;
		var propertyParameterValue = property.SetMethod!.Parameters[0].Type.GetName();
		var thisParameter = $"this global::Rocks.Expectations.ExplicitPropertySetterExpectations<{result.MockType.GetName()}, {containingTypeName}> @self";
		var mockTypeName = result.MockType.GetName();

		var delegateTypeName = property.SetMethod!.RequiresProjectedDelegate() ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(property.SetMethod!, result.MockType) :
			DelegateBuilder.Build(property.SetMethod!.Parameters);
		var adornmentsType = property.SetMethod!.RequiresProjectedDelegate() ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, result.MockType, AdornmentType.Property, true)}<{mockTypeName}, {delegateTypeName}>" :
			$"global::Rocks.PropertyAdornments<{mockTypeName}, {delegateTypeName}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		// TODO: This doesn't seem right, the getter has an "add" qualified for projected names.
		writer.WriteLine($"internal static {returnValue} {property.Name}({thisParameter}, global::Rocks.Argument<{propertyParameterValue}> value) =>");
		writer.Indent++;

		writer.WriteLine($"{newAdornments}(@self.Add({memberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) {{ value }}));");
		writer.Indent--;
	}

	// TODO: This isn't good. I'm passing in a PropertyAccessor value to state 
	// if I should be doing a "get", "set", or "init", but then I also look at the 
	// property's accessor value for the member identifier increment. This
	// doesn't feel "right".
	internal static void Build(IndentedTextWriter writer, PropertyMockableResult result, IAssemblySymbol typeToMockContainingAssembly,
		PropertyAccessor accessor, string containingTypeName)
	{
		var memberIdentifier = result.MemberIdentifier;

		if (accessor == PropertyAccessor.Get &&
			result.Value.GetMethod!.CanBeSeenByContainingAssembly(typeToMockContainingAssembly))
		{
			ExplicitPropertyExpectationsExtensionsPropertyBuilder.BuildGetter(writer, result, memberIdentifier, containingTypeName);
		}
		else if(accessor == PropertyAccessor.Set)
		{
			if (result.Accessors == PropertyAccessor.GetAndSet &&
				result.Value.SetMethod!.CanBeSeenByContainingAssembly(typeToMockContainingAssembly))
			{
				memberIdentifier++;
			}

			ExplicitPropertyExpectationsExtensionsPropertyBuilder.BuildSetter(writer, result, memberIdentifier, containingTypeName);
		}
	}
}