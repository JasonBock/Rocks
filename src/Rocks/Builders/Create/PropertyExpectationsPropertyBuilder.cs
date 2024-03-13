using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class PropertyExpectationsPropertyBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, string expectationsFullyQualifiedName, 
		Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		var propertyGetMethod = property.GetMethod!;
		var callbackDelegateTypeName = propertyGetMethod.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
			DelegateBuilder.Build(propertyGetMethod);

		string adornmentsType;

		if (property.Type.TypeKind == TypeKind.FunctionPointer ||
			property.Type.TypeKind == TypeKind.Pointer)
		{
			var projectedAdornmentTypeName = MockProjectedAdornmentsTypesBuilder.GetProjectedAdornmentsFullyQualifiedNameName(property.Type, property.MockType);
			adornmentsType = $"{projectedAdornmentTypeName}<AdornmentsForHandler{memberIdentifier}, {callbackDelegateTypeName}>";
		}
		else
		{
			var handlerTypeName = $"{expectationsFullyQualifiedName}.Handler{memberIdentifier}";
			var returnType =
				property.Type.IsRefLikeType ?
					MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
					property.Type.FullyQualifiedName;

			adornmentsType = $"global::Rocks.Adornments<AdornmentsForHandler{memberIdentifier}, {handlerTypeName}, {callbackDelegateTypeName}, {returnType}>";
		}

		adornmentsFQNsPipeline(new(adornmentsType, string.Empty, string.Empty, memberIdentifier));

		writer.WriteLines(
			$$"""
			internal {{expectationsFullyQualifiedName}}.Adornments.AdornmentsForHandler{{memberIdentifier}} {{property.Name}}()
			{
				global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
				var handler = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}();
				if (this.Expectations.handlers{{memberIdentifier}} is null ) { this.Expectations.handlers{{memberIdentifier}} = new(handler); }
				else { this.Expectations.handlers{{memberIdentifier}}.Add(handler); }
				return new(handler);
			}
			""");
	}

	private static void BuildSetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, string expectationsFullyQualifiedName, 
		Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		var propertyParameterType = property.SetMethod!.Parameters[0].Type;
		var propertyParameterValue =
			propertyParameterType.IsEsoteric ?
				propertyParameterType.IsRefLikeType ?
					RefLikeArgTypeBuilder.GetProjectedFullyQualifiedName(propertyParameterType, property.MockType) :
					PointerArgTypeBuilder.GetProjectedFullyQualifiedName(propertyParameterType, property.MockType) :
				$"global::Rocks.Argument<{propertyParameterType.FullyQualifiedName}>";
		var callbackDelegateTypeName = property.SetMethod!.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(property.SetMethod!, property.MockType) :
			DelegateBuilder.Build(property.SetMethod!);
		var adornmentsType = $"global::Rocks.Adornments<AdornmentsForHandler{memberIdentifier}, {expectationsFullyQualifiedName}.Handler{memberIdentifier}, {callbackDelegateTypeName}>";
		adornmentsFQNsPipeline(new(adornmentsType, string.Empty, string.Empty, memberIdentifier));

		writer.WriteLines(
			$$"""
			internal {{expectationsFullyQualifiedName}}.Adornments.AdornmentsForHandler{{memberIdentifier}} {{property.Name}}({{propertyParameterValue}} @value)
			{
				global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
				global::System.ArgumentNullException.ThrowIfNull(@value);

				var handler = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}
				{
					value = @value,
				};

				if (this.Expectations.handlers{{memberIdentifier}} is null ) { this.Expectations.handlers{{memberIdentifier}} = new(handler); }
				else { this.Expectations.handlers{{memberIdentifier}}.Add(handler); }
				return new(handler);
			}
			""");
	}

	internal static void Build(IndentedTextWriter writer, PropertyModel property, PropertyAccessor accessor, string expectationsFullyQualifiedName,
		Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		var memberIdentifier = property.MemberIdentifier;

		if (accessor == PropertyAccessor.Get && property.GetCanBeSeenByContainingAssembly)
		{
			PropertyExpectationsPropertyBuilder.BuildGetter(writer, property, memberIdentifier, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
		}
		else if ((accessor == PropertyAccessor.Set && property.SetCanBeSeenByContainingAssembly) ||
			(accessor == PropertyAccessor.Init && property.InitCanBeSeenByContainingAssembly))
		{
			if ((property.Accessors == PropertyAccessor.GetAndSet || property.Accessors == PropertyAccessor.GetAndInit) &&
				property.GetCanBeSeenByContainingAssembly)
			{
				memberIdentifier++;
			}

			PropertyExpectationsPropertyBuilder.BuildSetter(writer, property, memberIdentifier, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
		}
	}
}