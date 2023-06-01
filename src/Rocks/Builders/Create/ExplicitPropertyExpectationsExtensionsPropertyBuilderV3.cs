using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal static class ExplicitPropertyExpectationsExtensionsPropertyBuilderV3
{
	private static void BuildGetter(IndentedTextWriter writer, Models.PropertyModel property, uint memberIdentifier, string containingTypeName)
	{
		var propertyGetMethod = property.GetMethod!;

		var mockTypeName = property.MockType.FullyQualifiedName;
		var thisParameter = $"this global::Rocks.Expectations.ExplicitPropertyGetterExpectations<{mockTypeName}, {containingTypeName}> @self";

		var delegateTypeName = propertyGetMethod.RequiresProjectedDelegate ?
			propertyGetMethod.ReturnType.IsRefLikeType ?
				MockProjectedDelegateBuilderV3.GetProjectedReturnValueDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
				MockProjectedDelegateBuilderV3.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
			DelegateBuilderV3.Build(ImmutableArray<ParameterModel>.Empty, property.Type);
		var propertyReturnValue = propertyGetMethod.ReturnType.IsRefLikeType ?
			delegateTypeName : propertyGetMethod.ReturnType.FullyQualifiedName;
		var adornmentsType = property.Type.IsEsoteric ?
			$"{MockProjectedTypesAdornmentsBuilderV3.GetProjectedAdornmentFullyQualifiedNameName(property.Type, property.MockType, AdornmentType.Property, true)}<{mockTypeName}, {delegateTypeName}>" :
			$"global::Rocks.PropertyAdornments<{mockTypeName}, {delegateTypeName}, {propertyReturnValue}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		writer.WriteLine($"internal static {returnValue} {property.Name}({thisParameter}) =>");
		writer.Indent++;

		var addMethod = property.Type.IsPointer ?
			MockProjectedTypesAdornmentsBuilderV3.GetProjectedAddExtensionMethodFullyQualifiedName(property.Type, property.MockType) : 
			$"Add<{propertyReturnValue}>";

		writer.WriteLine($"{newAdornments}(@self.{addMethod}({memberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>()));");
		writer.Indent--;
	}

	private static void BuildSetter(IndentedTextWriter writer, Models.PropertyModel property, uint memberIdentifier, string containingTypeName, PropertyAccessor accessor)
	{
		var propertyParameterValue = property.SetMethod!.Parameters[0].Type.IncludeGenericsName;
		var accessorName = accessor == PropertyAccessor.Set ? "Setter" : "Initializer";
		var mockTypeName = property.MockType.FullyQualifiedName;
		var thisParameter = $"this global::Rocks.Expectations.ExplicitProperty{accessorName}Expectations<{mockTypeName}, {containingTypeName}> @self";

		var delegateTypeName = property.SetMethod!.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilderV3.GetProjectedCallbackDelegateFullyQualifiedName(property.SetMethod!, property.MockType) :
			DelegateBuilderV3.Build(property.SetMethod!.Parameters);
		var adornmentsType = property.SetMethod!.RequiresProjectedDelegate ?
			$"{MockProjectedTypesAdornmentsBuilderV3.GetProjectedAdornmentFullyQualifiedNameName(property.Type, property.MockType, AdornmentType.Property, true)}<{mockTypeName}, {delegateTypeName}>" :
			$"global::Rocks.PropertyAdornments<{mockTypeName}, {delegateTypeName}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		writer.WriteLine($"internal static {returnValue} {property.Name}({thisParameter}, global::Rocks.Argument<{propertyParameterValue}> value) =>");
		writer.Indent++;

		writer.WriteLine($"{newAdornments}(@self.Add({memberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>(1) {{ value }}));");
		writer.Indent--;
	}

	internal static void Build(IndentedTextWriter writer, Models.PropertyModel property,
		PropertyAccessor accessor, string containingTypeName)
	{
		var memberIdentifier = property.MemberIdentifier;

		if (accessor == PropertyAccessor.Get && property.GetCanBeSeenByContainingAssembly)
		{
			ExplicitPropertyExpectationsExtensionsPropertyBuilderV3.BuildGetter(writer, property, memberIdentifier, containingTypeName);
		}
		else if(accessor == PropertyAccessor.Set || accessor == PropertyAccessor.Init)
		{
			if ((property.Accessors == PropertyAccessor.GetAndSet || property.Accessors == PropertyAccessor.GetAndInit) &&
				(property.SetCanBeSeenByContainingAssembly || property.InitCanBeSeenByContainingAssembly))
			{
				memberIdentifier++;
			}

			ExplicitPropertyExpectationsExtensionsPropertyBuilderV3.BuildSetter(writer, property, memberIdentifier, containingTypeName, accessor);
		}
	}
}