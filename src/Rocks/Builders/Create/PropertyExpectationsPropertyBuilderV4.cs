using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class PropertyExpectationsPropertyBuilderV4
{
	private static void BuildGetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, string expectationsFullyQualifiedName)
	{
		var propertyGetMethod = property.GetMethod!;

		var mockTypeName = property.MockType.FullyQualifiedName;
		var callbackDelegateTypeName = propertyGetMethod.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
			DelegateBuilder.Build(ImmutableArray<ParameterModel>.Empty, property.Type);
		var propertyReturnValue = propertyGetMethod.ReturnType.IsRefLikeType ?
			MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
			propertyGetMethod.ReturnType.FullyQualifiedName;
		var returnValue = propertyGetMethod.ReturnType.IsPointer ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, property.MockType, AdornmentType.Property, false)}<{mockTypeName}, {callbackDelegateTypeName}>" :
			$"global::Rocks.AdornmentsV4<{expectationsFullyQualifiedName}.Handler{memberIdentifier}, {callbackDelegateTypeName}, {propertyReturnValue}>";

		writer.WriteLines(
			$$"""
			internal {{returnValue}} {{property.Name}}()
			{
				var handler = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}();
				this.Expectations.handlers{{memberIdentifier}}.Add(handler);
				return new(handler);
			}
			""");
	}

	private static void BuildSetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, string expectationsFullyQualifiedName)
	{
		var propertyParameterType = property.SetMethod!.Parameters[0].Type;
		var propertyParameterValue =
			propertyParameterType.IsEsoteric ?
				propertyParameterType.IsPointer ?
					PointerArgTypeBuilder.GetProjectedFullyQualifiedName(propertyParameterType, property.MockType) :
					RefLikeArgTypeBuilder.GetProjectedFullyQualifiedName(propertyParameterType, property.MockType) :
			propertyParameterType.FullyQualifiedName;
		var mockTypeName = property.MockType.FullyQualifiedName;
		var callbackDelegateTypeName = property.SetMethod!.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(property.SetMethod!, property.MockType) :
			DelegateBuilder.Build(property.SetMethod!.Parameters);
		var returnValue = propertyParameterType.IsPointer ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, property.MockType, AdornmentType.Property, false)}<{mockTypeName}, {callbackDelegateTypeName}>" :
			$"global::Rocks.AdornmentsV4<{expectationsFullyQualifiedName}.Handler{memberIdentifier}, {callbackDelegateTypeName}>";

		writer.WriteLines(
			$$"""
			internal {{returnValue}} {{property.Name}}(global::Rocks.Argument<{{propertyParameterValue}}> @value)
			{
				var handler = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}
				{
					value = @value,
				};

				this.Expectations.handlers{{memberIdentifier}}.Add(handler);
				return new(handler);
			}
			""");
	}

	internal static void Build(IndentedTextWriter writer, PropertyModel property, PropertyAccessor accessor, string expectationsFullyQualifiedName)
	{
		var memberIdentifier = property.MemberIdentifier;

		if (accessor == PropertyAccessor.Get && property.GetCanBeSeenByContainingAssembly)
		{
			PropertyExpectationsPropertyBuilderV4.BuildGetter(writer, property, memberIdentifier, expectationsFullyQualifiedName);
		}
		else if ((accessor == PropertyAccessor.Set && property.SetCanBeSeenByContainingAssembly) ||
			(accessor == PropertyAccessor.Init && property.InitCanBeSeenByContainingAssembly))
		{
			if ((property.Accessors == PropertyAccessor.GetAndSet || property.Accessors == PropertyAccessor.GetAndInit) &&
				property.GetCanBeSeenByContainingAssembly)
			{
				memberIdentifier++;
			}

			PropertyExpectationsPropertyBuilderV4.BuildSetter(writer, property, memberIdentifier, expectationsFullyQualifiedName);
		}
	}
}