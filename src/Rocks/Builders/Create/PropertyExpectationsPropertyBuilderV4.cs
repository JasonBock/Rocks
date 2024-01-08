﻿using Microsoft.CodeAnalysis;
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
		var callbackDelegateTypeName = propertyGetMethod.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilderV4.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
			DelegateBuilder.Build(ImmutableArray<ParameterModel>.Empty, property.Type);

		string adornmentsType;

		if (property.Type.TypeKind == TypeKind.FunctionPointer ||
			property.Type.TypeKind == TypeKind.Pointer)
		{
			var projectedAdornmentTypeName = MockProjectedAdornmentsTypesBuilderV4.GetProjectedAdornmentsFullyQualifiedNameName(property.Type, property.MockType);
			adornmentsType = $"{projectedAdornmentTypeName}<{callbackDelegateTypeName}>";
		}
		else
		{
			var handlerTypeName = $"{expectationsFullyQualifiedName}.Handler{memberIdentifier}";
			var returnType =
				property.Type.IsRefLikeType ?
					MockProjectedDelegateBuilderV4.GetProjectedReturnValueDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
					property.Type.FullyQualifiedName;

			adornmentsType = $"global::Rocks.AdornmentsV4<{handlerTypeName}, {callbackDelegateTypeName}, {returnType}>";
		}

		writer.WriteLines(
			$$"""
			internal {{adornmentsType}} {{property.Name}}()
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
				ProjectedArgTypeBuilderV4.GetProjectedFullyQualifiedName(propertyParameterType, property.MockType) :
				$"global::Rocks.Argument<{propertyParameterType.FullyQualifiedName}>";
		var callbackDelegateTypeName = property.SetMethod!.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilderV4.GetProjectedCallbackDelegateFullyQualifiedName(property.SetMethod!, property.MockType) :
			DelegateBuilder.Build(property.SetMethod!.Parameters);
		var adornmentsType = $"global::Rocks.AdornmentsV4<{expectationsFullyQualifiedName}.Handler{memberIdentifier}, {callbackDelegateTypeName}>";

		writer.WriteLines(
			$$"""
			internal {{adornmentsType}} {{property.Name}}({{propertyParameterValue}} @value)
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