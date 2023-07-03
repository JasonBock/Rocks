using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class PropertyExpectationsExtensionsPropertyBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier)
	{
		var propertyGetMethod = property.GetMethod!;

		var mockTypeName = property.MockType.FullyQualifiedName;
		var thisParameter = $"this global::Rocks.Expectations.PropertyGetterExpectations<{mockTypeName}> @self";
		var callbackDelegateTypeName = propertyGetMethod.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
			DelegateBuilder.Build(ImmutableArray<ParameterModel>.Empty, property.Type);
		var propertyReturnValue = propertyGetMethod.ReturnType.IsRefLikeType ?
			MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(propertyGetMethod, property.MockType) : 
			propertyGetMethod.ReturnType.FullyQualifiedName;
		var adornmentsType = propertyGetMethod.ReturnType.IsPointer ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, property.MockType, AdornmentType.Property, false)}<{mockTypeName}, {callbackDelegateTypeName}>" :
			$"global::Rocks.PropertyAdornments<{mockTypeName}, {callbackDelegateTypeName}, {propertyReturnValue}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		var addMethod = property.Type.IsPointer ?
			MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodFullyQualifiedName(property.Type, property.MockType) :
			$"Add<{propertyReturnValue}>";

		writer.WriteLines(
			$$"""
			internal static {{returnValue}} {{property.Name}}({{thisParameter}}) =>
				{{newAdornments}}(@self.{{addMethod}}({{memberIdentifier}}, new global::System.Collections.Generic.List<global::Rocks.Argument>()));
			""");
	}

	private static void BuildSetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, PropertyAccessor accessor)
	{
		var propertyParameterType = property.SetMethod!.Parameters[0].Type;
		var propertyParameterValue = 
			propertyParameterType.IsEsoteric ?
				propertyParameterType.IsPointer ?
					PointerArgTypeBuilder.GetProjectedFullyQualifiedName(propertyParameterType, property.MockType) :
					RefLikeArgTypeBuilder.GetProjectedFullyQualifiedName(propertyParameterType, property.MockType) :
			propertyParameterType.FullyQualifiedName;
		var mockTypeName = property.MockType.FullyQualifiedName;
		var accessorName = accessor == PropertyAccessor.Set ? "Setter" : "Initializer";
		var thisParameter = $"this global::Rocks.Expectations.Property{accessorName}Expectations<{mockTypeName}> @self";
		var delegateTypeName = property.SetMethod!.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(property.SetMethod!, property.MockType) :
			DelegateBuilder.Build(property.SetMethod!.Parameters);
		var adornmentsType = propertyParameterType.IsPointer ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, property.MockType, AdornmentType.Property, false)}<{mockTypeName}, {delegateTypeName}>" :
			$"global::Rocks.PropertyAdornments<{mockTypeName}, {delegateTypeName}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		writer.WriteLines(
			$$"""
			internal static {{returnValue}} {{property.Name}}({{thisParameter}}, global::Rocks.Argument<{{propertyParameterValue}}> @value) =>
				{{newAdornments}}(@self.Add({{memberIdentifier}}, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) { @value }));
			""");
	}

	internal static void Build(IndentedTextWriter writer, PropertyModel result, PropertyAccessor accessor)
	{
		var memberIdentifier = result.MemberIdentifier;

		if (accessor == PropertyAccessor.Get && result.GetCanBeSeenByContainingAssembly)
		{
			PropertyExpectationsExtensionsPropertyBuilder.BuildGetter(writer, result, memberIdentifier);
		}
		else if((accessor == PropertyAccessor.Set || accessor == PropertyAccessor.Init) &&
			(result.SetCanBeSeenByContainingAssembly || result.InitCanBeSeenByContainingAssembly))
		{
			if (result.Accessors == PropertyAccessor.GetAndSet ||
				result.Accessors == PropertyAccessor.GetAndInit)
			{
				memberIdentifier++;
			}

			PropertyExpectationsExtensionsPropertyBuilder.BuildSetter(writer, result, memberIdentifier, accessor);
		}
	}
}