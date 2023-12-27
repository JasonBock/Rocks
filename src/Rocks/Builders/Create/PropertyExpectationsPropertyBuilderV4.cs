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

		writer.WriteLine($"internal {returnValue} {property.Name}()");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLines(
			$$"""
			var handler = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}
			{
				Callback = null,
				CallCount = 0,
				ExpectedCallCount = 1,
				ReturnValue = null
			};

			this.Expectations.handlers{{memberIdentifier}}.Add(handler);
			return new(handler);
			""");
		writer.Indent--;
		writer.WriteLine("}");
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
		var delegateTypeName = property.SetMethod!.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(property.SetMethod!, property.MockType) :
			DelegateBuilder.Build(property.SetMethod!.Parameters);
		var returnValue = propertyParameterType.IsPointer ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, property.MockType, AdornmentType.Property, false)}<{mockTypeName}, {delegateTypeName}>" :
			$"global::Rocks.AdornmentsV4<{mockTypeName}, {delegateTypeName}>";

		writer.WriteLine($"internal {returnValue} {property.Name}(global::Rocks.Argument<{propertyParameterValue}> @value)");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLines(
			$$"""
			var handler = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}
			{
				value = @value,
				Callback = null,
				CallCount = 0,
				ExpectedCallCount = 1,
				ReturnValue = null
			};

			this.Expectations.handlers{{memberIdentifier}}.Add(handler);
			return new(handler);
			""");
		writer.Indent--;
		writer.WriteLine("}");
	}

	internal static void Build(IndentedTextWriter writer, PropertyModel result, PropertyAccessor accessor, string expectationsFullyQualifiedName)
	{
		var memberIdentifier = result.MemberIdentifier;

		if (accessor == PropertyAccessor.Get && result.GetCanBeSeenByContainingAssembly)
		{
			PropertyExpectationsPropertyBuilderV4.BuildGetter(writer, result, memberIdentifier, expectationsFullyQualifiedName);
		}
		else if((accessor == PropertyAccessor.Set || accessor == PropertyAccessor.Init) &&
			(result.SetCanBeSeenByContainingAssembly || result.InitCanBeSeenByContainingAssembly))
		{
			if (result.Accessors == PropertyAccessor.GetAndSet ||
				result.Accessors == PropertyAccessor.GetAndInit)
			{
				memberIdentifier++;
			}

			PropertyExpectationsPropertyBuilderV4.BuildSetter(writer, result, memberIdentifier, expectationsFullyQualifiedName);
		}
	}
}