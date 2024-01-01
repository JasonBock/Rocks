using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class ExplicitIndexerExpectationsIndexerBuilderV4
{
	private static void BuildGetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, string expectationsFullyQualifiedName)
	{
		var propertyGetMethod = property.GetMethod!;

		var mockTypeName = property.MockType.FullyQualifiedName;
		var callbackDelegateTypeName = propertyGetMethod.RequiresProjectedDelegate ?
			propertyGetMethod.ReturnType.IsRefLikeType ?
				MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
			DelegateBuilder.Build(propertyGetMethod.Parameters, property.Type);
		var propertyReturnValue = propertyGetMethod.ReturnType.IsRefLikeType ?
			callbackDelegateTypeName : propertyGetMethod.ReturnType.FullyQualifiedName;
		var returnValue = propertyGetMethod.ReturnType.IsPointer ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, property.MockType, AdornmentType.Property, false)}<{mockTypeName}, {callbackDelegateTypeName}>" :
			$"global::Rocks.AdornmentsV4<{expectationsFullyQualifiedName}.Handler{memberIdentifier}, {callbackDelegateTypeName}, {propertyReturnValue}>";
		var instanceParameters = string.Join(", ", propertyGetMethod.Parameters.Select(_ =>
		{
			return $"global::Rocks.Argument<{_.Type.FullyQualifiedName}> @{_.Name}";
		}));

		writer.WriteLines(
			$$"""
			internal {{returnValue}} This({{instanceParameters}})
			{
			""");
		writer.Indent++;

		foreach (var parameter in property.Parameters)
		{
			writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{parameter.Name});");
		}

		writer.WriteLine();
		writer.WriteLines(
			$$"""
			var handler = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}
			{
			""");
		writer.Indent++;

		var handlerNamingContext = HandlerVariableNamingContextV4.Create();

		foreach (var parameter in property.Parameters)
		{
			if (parameter.HasExplicitDefaultValue)
			{
				writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = @{parameter.Name}.Transform({parameter.ExplicitDefaultValue}),");
			}
			else
			{
				writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = @{parameter.Name},");
			}
		}

		writer.Indent--;
		writer.WriteLines(
			$$"""
			};

			this.Expectations.handlers{{memberIdentifier}}.Add(handler);
			return new(handler);
			""");

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildSetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, string expectationsFullyQualifiedName)
	{
		var propertySetMethod = property.SetMethod!;

		var mockTypeName = property.MockType.FullyQualifiedName;
		var callbackDelegateTypeName = propertySetMethod.RequiresProjectedDelegate ?
			propertySetMethod.ReturnType.IsRefLikeType ?
				MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(propertySetMethod, property.MockType) :
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertySetMethod, property.MockType) :
			DelegateBuilder.Build(propertySetMethod.Parameters, property.Type);
		var returnValue = propertySetMethod.ReturnType.IsPointer ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, property.MockType, AdornmentType.Property, false)}<{mockTypeName}, {callbackDelegateTypeName}>" :
			$"global::Rocks.AdornmentsV4<{expectationsFullyQualifiedName}.Handler{memberIdentifier}, {callbackDelegateTypeName}>";
		var instanceParameters = string.Join(", ", propertySetMethod.Parameters.Select(_ =>
		{
			return $"global::Rocks.Argument<{_.Type.FullyQualifiedName}> @{_.Name}";
		}));

		writer.WriteLines(
			$$"""
			internal {{returnValue}} This({{instanceParameters}})
			{
			""");
		writer.Indent++;

		foreach (var parameter in property.Parameters)
		{
			writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{parameter.Name});");
		}

		writer.WriteLine();
		writer.WriteLines(
			$$"""
			var handler = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}
			{
			""");
		writer.Indent++;

		var handlerNamingContext = HandlerVariableNamingContextV4.Create();

		foreach (var parameter in property.Parameters)
		{
			if (parameter.HasExplicitDefaultValue)
			{
				writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = @{parameter.Name}.Transform({parameter.ExplicitDefaultValue}),");
			}
			else
			{
				writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = @{parameter.Name},");
			}
		}

		writer.Indent--;
		writer.WriteLines(
			$$"""
			};

			this.Expectations.handlers{{memberIdentifier}}.Add(handler);
			return new(handler);
			""");

		writer.Indent--;
		writer.WriteLine("}");
	}

	internal static void Build(IndentedTextWriter writer, PropertyModel property,  
		PropertyAccessor accessor, string expectationsFullyQualifiedName)
	{
		var memberIdentifier = property.MemberIdentifier;

		if (accessor == PropertyAccessor.Get && property.GetCanBeSeenByContainingAssembly)
		{
			ExplicitIndexerExpectationsIndexerBuilderV4.BuildGetter(writer, property, memberIdentifier, expectationsFullyQualifiedName);
		}
		else if(accessor == PropertyAccessor.Set || accessor == PropertyAccessor.Init)
		{
			if ((property.Accessors == PropertyAccessor.GetAndSet || property.Accessors == PropertyAccessor.GetAndInit) &&
				(property.SetCanBeSeenByContainingAssembly || property.InitCanBeSeenByContainingAssembly))
			{
				memberIdentifier++;
			}

			ExplicitIndexerExpectationsIndexerBuilderV4.BuildSetter(writer, property, memberIdentifier, expectationsFullyQualifiedName);
		}
	}
}