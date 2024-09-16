using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class PropertyExpectationsPropertyBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, TypeMockModel type, PropertyModel property, uint memberIdentifier, string expectationsFullyQualifiedName, 
		Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		var propertyGetMethod = property.GetMethod!;
		var callbackDelegateTypeName = propertyGetMethod.NeedsProjection ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(property.GetMethod!, type.Type, expectationsFullyQualifiedName, memberIdentifier) :
			DelegateBuilder.Build(propertyGetMethod);

		var handlerTypeName = $"{expectationsFullyQualifiedName}.Handler{memberIdentifier}";

		string adornmentsType;

		if (property.Type.IsPointer)
		{
			adornmentsType = $"global::Rocks.Adornments<AdornmentsForHandler{memberIdentifier}, {handlerTypeName}, {callbackDelegateTypeName}>";
		}
		else
		{
			var returnType =
				property.Type.IsRefLikeType || property.Type.AllowsRefLikeType ?
					$"global::System.Func<{property.Type.FullyQualifiedName}>" :
					property.Type.FullyQualifiedName;

			adornmentsType = $"global::Rocks.Adornments<AdornmentsForHandler{memberIdentifier}, {handlerTypeName}, {callbackDelegateTypeName}, {returnType}>";
		}

		adornmentsFQNsPipeline(new(adornmentsType, string.Empty, string.Empty, property.GetMethod!, memberIdentifier));

		writer.WriteLines(
			$$"""
			internal {{expectationsFullyQualifiedName}}.Adornments.AdornmentsForHandler{{memberIdentifier}} {{property.Name}}()
			{
				global::Rocks.Exceptions.ExpectationException.ThrowIf(this.{{type.ExpectationsPropertyName}}.WasInstanceInvoked);
				var handler = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}();
				if (this.{{type.ExpectationsPropertyName}}.handlers{{memberIdentifier}} is null) { this.{{type.ExpectationsPropertyName}}.handlers{{memberIdentifier}} = new(handler); }
				else { this.{{type.ExpectationsPropertyName}}.handlers{{memberIdentifier}}.Add(handler); }
				return new(handler);
			}
			""");
	}

	private static void BuildSetter(IndentedTextWriter writer, TypeMockModel type, PropertyModel property, uint memberIdentifier, string expectationsFullyQualifiedName, 
		Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		var propertyParameterType = property.SetMethod!.Parameters[0].Type;
		var propertyParameterValue =
			propertyParameterType.IsPointer ?
				$"global::Rocks.Projections.{propertyParameterType.PointerNames}Argument<{propertyParameterType.PointedAt!.FullyQualifiedName}>" :
				propertyParameterType.NeedsProjection ?
					$"global::Rocks.Projections.{propertyParameterType.Name}Argument" :
					propertyParameterType.IsRefLikeType || propertyParameterType.AllowsRefLikeType ?
						$"global::Rocks.RefStructArgument<{propertyParameterType.FullyQualifiedName}>" :
						$"global::Rocks.Argument<{propertyParameterType.FullyQualifiedName}>";
		var callbackDelegateTypeName = property.SetMethod!.NeedsProjection ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(property.SetMethod!, type.Type, expectationsFullyQualifiedName, memberIdentifier) :
			DelegateBuilder.Build(property.SetMethod!);
		var adornmentsType = $"global::Rocks.Adornments<AdornmentsForHandler{memberIdentifier}, {expectationsFullyQualifiedName}.Handler{memberIdentifier}, {callbackDelegateTypeName}>";
		adornmentsFQNsPipeline(new(adornmentsType, string.Empty, string.Empty, property.SetMethod!, memberIdentifier));

		writer.WriteLines(
			$$"""
			internal {{expectationsFullyQualifiedName}}.Adornments.AdornmentsForHandler{{memberIdentifier}} {{property.Name}}({{propertyParameterValue}} @value)
			{
				global::Rocks.Exceptions.ExpectationException.ThrowIf(this.{{type.ExpectationsPropertyName}}.WasInstanceInvoked);
				global::System.ArgumentNullException.ThrowIfNull(@value);

				var handler = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}
				{
					value = @value,
				};

				if (this.{{type.ExpectationsPropertyName}}.handlers{{memberIdentifier}} is null) { this.{{type.ExpectationsPropertyName}}.handlers{{memberIdentifier}} = new(handler); }
				else { this.{{type.ExpectationsPropertyName}}.handlers{{memberIdentifier}}.Add(handler); }
				return new(handler);
			}
			""");
	}

	internal static void Build(IndentedTextWriter writer, TypeMockModel type, PropertyModel property, PropertyAccessor accessor, string expectationsFullyQualifiedName,
		Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		var memberIdentifier = property.MemberIdentifier;

		if (accessor == PropertyAccessor.Get && property.GetCanBeSeenByContainingAssembly)
		{
			PropertyExpectationsPropertyBuilder.BuildGetter(writer, type, property, memberIdentifier, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
		}
		else if ((accessor == PropertyAccessor.Set && property.SetCanBeSeenByContainingAssembly) ||
			(accessor == PropertyAccessor.Init && property.InitCanBeSeenByContainingAssembly))
		{
			if ((property.Accessors == PropertyAccessor.GetAndSet || property.Accessors == PropertyAccessor.GetAndInit) &&
				property.GetCanBeSeenByContainingAssembly)
			{
				memberIdentifier++;
			}

			PropertyExpectationsPropertyBuilder.BuildSetter(writer, type, property, memberIdentifier, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
		}
	}
}