using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class PropertyExpectationsPropertyBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, string expectationsFullyQualifiedName, 
		Action<string, string, string> adornmentsFQNsPipeline)
	{
		var propertyGetMethod = property.GetMethod!;
		var callbackDelegateTypeName = propertyGetMethod.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
			DelegateBuilder.Build(ImmutableArray<ParameterModel>.Empty, property.Type);

		string adornmentsType;

		if (property.Type.TypeKind == TypeKind.FunctionPointer ||
			property.Type.TypeKind == TypeKind.Pointer)
		{
			var projectedAdornmentTypeName = MockProjectedAdornmentsTypesBuilder.GetProjectedAdornmentsFullyQualifiedNameName(property.Type, property.MockType);
			adornmentsType = $"{projectedAdornmentTypeName}<{callbackDelegateTypeName}>";
		}
		else
		{
			var handlerTypeName = $"{expectationsFullyQualifiedName}.Handler{memberIdentifier}";
			var returnType =
				property.Type.IsRefLikeType ?
					MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
					property.Type.FullyQualifiedName;

			adornmentsType = $"global::Rocks.Adornments<{handlerTypeName}, {callbackDelegateTypeName}, {returnType}>";
		}

		adornmentsFQNsPipeline(adornmentsType, string.Empty, string.Empty);

		writer.WriteLines(
			$$"""
			internal {{adornmentsType}} {{property.Name}}()
			{
				global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
				if (this.Expectations.handlers{{memberIdentifier}} is null ) { this.Expectations.handlers{{memberIdentifier}} = new(); }
				var handler = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}();
				this.Expectations.handlers{{memberIdentifier}}.Add(handler);
				return new(handler);
			}
			""");
	}

	private static void BuildSetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, string expectationsFullyQualifiedName, 
		Action<string, string, string> adornmentsFQNsPipeline)
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
			DelegateBuilder.Build(property.SetMethod!.Parameters);
		var adornmentsType = $"global::Rocks.Adornments<{expectationsFullyQualifiedName}.Handler{memberIdentifier}, {callbackDelegateTypeName}>";
		adornmentsFQNsPipeline(adornmentsType, string.Empty, string.Empty);

		writer.WriteLines(
			$$"""
			internal {{adornmentsType}} {{property.Name}}({{propertyParameterValue}} @value)
			{
				global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);
				global::System.ArgumentNullException.ThrowIfNull(@value);

				var handler = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}
				{
					value = @value,
				};

				if (this.Expectations.handlers{{memberIdentifier}} is null ) { this.Expectations.handlers{{memberIdentifier}} = new(); }
				this.Expectations.handlers{{memberIdentifier}}.Add(handler);
				return new(handler);
			}
			""");
	}

	internal static void Build(IndentedTextWriter writer, PropertyModel property, PropertyAccessor accessor, string expectationsFullyQualifiedName,
		Action<string, string, string> adornmentsFQNsPipeline)
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