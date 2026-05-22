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
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, expectationsFullyQualifiedName, memberIdentifier) :
			DelegateBuilder.Build(propertyGetMethod);

		var handlerTypeName = $"{expectationsFullyQualifiedName}.Handler{memberIdentifier}";
		var adornmentsTypeName = $"{property.Name}GetsAdornments{propertyGetMethod.Hash}";

		string adornmentsType;

		if (property.Type.IsPointer)
		{
			adornmentsType = $"global::Rocks.Adornments<{adornmentsTypeName}, {handlerTypeName}, {callbackDelegateTypeName}>";
		}
		else
		{
			var returnType =
				property.Type.IsRefLikeType || property.Type.AllowsRefLikeType ?
					$"global::System.Func<{property.Type.FullyQualifiedName}>" :
					property.Type.FullyQualifiedName;

			adornmentsType = $"global::Rocks.Adornments<{adornmentsTypeName}, {handlerTypeName}, {callbackDelegateTypeName}, {returnType}>";
		}

		adornmentsFQNsPipeline(new(adornmentsTypeName, adornmentsType, string.Empty, string.Empty, propertyGetMethod, memberIdentifier));

		writer.WriteLines(
			$$"""
			internal {{expectationsFullyQualifiedName}}.Adornments.{{adornmentsTypeName}} Gets()
			{
				global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);
				var handler = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}();
				this.parent.handlers{{memberIdentifier}} = handler;
				return new(handler, this.parent);
			}
			""");
	}

	private static void BuildSetter(IndentedTextWriter writer, TypeMockModel type, PropertyModel property, uint memberIdentifier,
		string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		var propertySetMethod = property.SetMethod!;
		var propertyParameterType = propertySetMethod.Parameters[0].Type;
		var propertyParameterValue = ProjectionBuilder.BuildArgument(
			propertyParameterType, new TypeArgumentsNamingContext(), property.SetMethod!.Parameters[0].RequiresNullableAnnotation);

		var callbackDelegateTypeName = propertySetMethod.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertySetMethod, expectationsFullyQualifiedName, memberIdentifier) :
			DelegateBuilder.Build(propertySetMethod);
		var name = property.Accessors == PropertyAccessor.Set || property.Accessors == PropertyAccessor.GetAndSet ?
			"Sets" : "Inits";
		var adornmentsTypeName = $"{property.Name}{name}Adornments{propertySetMethod.Hash}";
		var adornmentsType = $"global::Rocks.Adornments<{adornmentsTypeName}, {expectationsFullyQualifiedName}.Handler{memberIdentifier}, {callbackDelegateTypeName}>";
		adornmentsFQNsPipeline(new(adornmentsTypeName, adornmentsType, string.Empty, string.Empty, propertySetMethod, memberIdentifier));

		writer.WriteLines(
			$$"""
			internal {{expectationsFullyQualifiedName}}.Adornments.{{adornmentsTypeName}} {{name}}({{propertyParameterValue}} @value)
			{
				global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);
				global::System.ArgumentNullException.ThrowIfNull(@value);

				var handler = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}
				{
					value = @value,
				};

				if (this.parent.handlers{{memberIdentifier}} is null) { this.parent.handlers{{memberIdentifier}} = new(1); }
				this.parent.handlers{{memberIdentifier}}.Add(handler);
				return new(handler, this.parent);
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
			BuildGetter(writer, type, property, memberIdentifier, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
			wasGetGenerated = true;
		}

		if (((property.Accessors == PropertyAccessor.Set || property.Accessors == PropertyAccessor.GetAndSet) && property.SetCanBeSeenByContainingAssembly) ||
			((property.Accessors == PropertyAccessor.Init || property.Accessors == PropertyAccessor.GetAndInit) && property.InitCanBeSeenByContainingAssembly))
		{
			if (wasGetGenerated)
			{
				memberIdentifier++;
			}

			BuildSetter(writer, type, property, memberIdentifier, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
		}
	}
}