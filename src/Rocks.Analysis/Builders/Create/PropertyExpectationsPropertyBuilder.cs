using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class PropertyExpectationsPropertyBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, TypeMockModel type, PropertyModel property, uint memberIdentifier, 
		string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		var propertyGetMethod = property.GetMethod!;
		var callbackDelegateTypeName = propertyGetMethod.RequiresProjectedDelegate ?
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
			internal {{expectationsFullyQualifiedName}}.Adornments.AdornmentsForHandler{{memberIdentifier}} Gets()
			{
				global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);
				var handler = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}();
				if (this.parent.handlers{{memberIdentifier}} is null) { this.parent.handlers{{memberIdentifier}} = new(handler); }
				else { this.parent.handlers{{memberIdentifier}}.Add(handler); }
				return new(handler);
			}
			""");
	}

	private static void BuildSetter(IndentedTextWriter writer, TypeMockModel type, PropertyModel property, uint memberIdentifier, 
		string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		var propertyParameterType = property.SetMethod!.Parameters[0].Type;
		var propertyParameterValue = ProjectionBuilder.BuildArgument(
			propertyParameterType, new TypeArgumentsNamingContext(), property.SetMethod!.Parameters[0].RequiresNullableAnnotation);

		var callbackDelegateTypeName = property.SetMethod!.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(property.SetMethod!, type.Type, expectationsFullyQualifiedName, memberIdentifier) :
			DelegateBuilder.Build(property.SetMethod!);
		var adornmentsType = $"global::Rocks.Adornments<AdornmentsForHandler{memberIdentifier}, {expectationsFullyQualifiedName}.Handler{memberIdentifier}, {callbackDelegateTypeName}>";
		adornmentsFQNsPipeline(new(adornmentsType, string.Empty, string.Empty, property.SetMethod!, memberIdentifier));

		var name = property.Accessors == PropertyAccessor.Set || property.Accessors == PropertyAccessor.GetAndSet ?
			"Sets" : "Inits";

		writer.WriteLines(
			$$"""
			internal {{expectationsFullyQualifiedName}}.Adornments.AdornmentsForHandler{{memberIdentifier}} {{name}}({{propertyParameterValue}} @value)
			{
				global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);
				global::System.ArgumentNullException.ThrowIfNull(@value);

				var handler = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}
				{
					value = @value,
				};

				if (this.parent.handlers{{memberIdentifier}} is null) { this.parent.handlers{{memberIdentifier}} = new(handler); }
				else { this.parent.handlers{{memberIdentifier}}.Add(handler); }
				return new(handler);
			}
			""");
	}

	internal static void Build(IndentedTextWriter writer, TypeMockModel type, PropertyModel property,
		string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		var memberIdentifier = property.MemberIdentifier;
		var wasGetGenerated = false;

		if ((property.Accessors == PropertyAccessor.Get || property.Accessors == PropertyAccessor.GetAndSet || property.Accessors == PropertyAccessor.GetAndInit) && 
			property.GetCanBeSeenByContainingAssembly)
		{
			PropertyExpectationsPropertyBuilder.BuildGetter(writer, type, property, memberIdentifier, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
			wasGetGenerated = true;
		}
	
		if (((property.Accessors == PropertyAccessor.Set || property.Accessors == PropertyAccessor.GetAndSet) && property.SetCanBeSeenByContainingAssembly) ||
			((property.Accessors == PropertyAccessor.Init || property.Accessors == PropertyAccessor.GetAndInit) && property.InitCanBeSeenByContainingAssembly))
		{
			if (wasGetGenerated)
			{
				memberIdentifier++;
			}

			PropertyExpectationsPropertyBuilder.BuildSetter(writer, type, property, memberIdentifier, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
		}
	}
}