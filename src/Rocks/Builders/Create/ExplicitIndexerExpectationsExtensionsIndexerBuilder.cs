using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class ExplicitIndexerExpectationsExtensionsIndexerBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, Models.PropertyModel property, uint memberIdentifier, string containingTypeName)
	{
		var propertyGetMethod = property.GetMethod!;
		var namingContext = new VariableNamingContext(propertyGetMethod);
		var mockTypeName = property.MockType.FullyQualifiedName;
		var thisParameter = $"this global::Rocks.Expectations.ExplicitIndexerGetterExpectations<{mockTypeName}, {containingTypeName}> @{namingContext["self"]}";

		var delegateTypeName = propertyGetMethod.RequiresProjectedDelegate ?
			propertyGetMethod.ReturnType.IsRefLikeType ?
				MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
			DelegateBuilder.Build(property.Parameters, property.Type);
		var propertyReturnValue = propertyGetMethod.ReturnType.IsRefLikeType ?
			delegateTypeName : propertyGetMethod.ReturnType.FullyQualifiedName;
		var adornmentsType = propertyGetMethod.RequiresProjectedDelegate ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, property.MockType, AdornmentType.Indexer, true)}<{mockTypeName}, {delegateTypeName}>" :
			$"global::Rocks.IndexerAdornments<{mockTypeName}, {delegateTypeName}, {propertyReturnValue}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		var instanceParameters = string.Join(", ", thisParameter,
			string.Join(", ", propertyGetMethod.Parameters.Select(_ =>
			{
				return $"global::Rocks.Argument<{_.Type.FullyQualifiedName}> @{_.Name}";
			})));

		writer.WriteLine($"internal static {returnValue} This({instanceParameters})");
		writer.WriteLine("{");
		writer.Indent++;

		foreach (var parameter in propertyGetMethod.Parameters)
		{
			writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{parameter.Name});");
		}

		var parameters = string.Join(", ", propertyGetMethod.Parameters.Select(
			_ => _.HasExplicitDefaultValue && property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? 
				$"@{_.Name}.Transform({_.ExplicitDefaultValue})" : $"@{_.Name}"));
		var addMethod = property.Type.IsPointer ?
			MockProjectedTypesAdornmentsBuilder.GetProjectedAddExtensionMethodFullyQualifiedName(property.Type, property.MockType) : 
			$"Add<{propertyReturnValue}>";
		writer.WriteLine($"return {newAdornments}(@{namingContext["self"]}.{addMethod}({memberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>({propertyGetMethod.Parameters.Length}) {{ {parameters} }}));");

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildSetter(IndentedTextWriter writer, Models.PropertyModel property, uint memberIdentifier, string containingTypeName, PropertyAccessor accessor)
	{
		var propertySetMethod = property.SetMethod!;
		var namingContext = new VariableNamingContext(propertySetMethod);
		var mockTypeName = property.MockType.FullyQualifiedName;
		var accessorName = accessor == PropertyAccessor.Set ? "Setter" : "Initializer";
		var thisParameter = $"this global::Rocks.Expectations.ExplicitIndexer{accessorName}Expectations<{mockTypeName}, {containingTypeName}> @{namingContext["self"]}";

		var delegateTypeName = propertySetMethod.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertySetMethod, property.MockType) :
			DelegateBuilder.Build(propertySetMethod.Parameters);
		var adornmentsType = propertySetMethod.RequiresProjectedDelegate ?
			$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, property.MockType, AdornmentType.Indexer, true)}<{mockTypeName}, {delegateTypeName}>" :
			$"global::Rocks.IndexerAdornments<{mockTypeName}, {delegateTypeName}>";
		var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

		var instanceParameters = string.Join(", ", thisParameter,
			string.Join(", ", propertySetMethod.Parameters.Select(_ =>
			{
				return $"global::Rocks.Argument<{_.Type.FullyQualifiedName}> @{_.Name}";
			})));

		writer.WriteLine($"internal static {returnValue} This({instanceParameters})");
		writer.WriteLine("{");
		writer.Indent++;

		foreach (var parameter in propertySetMethod.Parameters)
		{
			writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{parameter.Name});");
		}

		var parameters = string.Join(", ", propertySetMethod.Parameters.Select(
			_ => _.HasExplicitDefaultValue && property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No ? 
				$"@{_.Name}.Transform({_.ExplicitDefaultValue})" : $"@{_.Name}"));
		writer.WriteLine($"return {newAdornments}({namingContext["self"]}.Add({memberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>({propertySetMethod.Parameters.Length}) {{ {parameters} }}));");

		writer.Indent--;
		writer.WriteLine("}");
	}

	internal static void Build(IndentedTextWriter writer, Models.PropertyModel property,  
		PropertyAccessor accessor, string containingTypeName)
	{
		var memberIdentifier = property.MemberIdentifier;

		if (accessor == PropertyAccessor.Get && property.GetCanBeSeenByContainingAssembly)
		{
			ExplicitIndexerExpectationsExtensionsIndexerBuilder.BuildGetter(writer, property, memberIdentifier, containingTypeName);
		}
		else if(accessor == PropertyAccessor.Set || accessor == PropertyAccessor.Init)
		{
			if ((property.Accessors == PropertyAccessor.GetAndSet || property.Accessors == PropertyAccessor.GetAndInit) &&
				(property.SetCanBeSeenByContainingAssembly || property.InitCanBeSeenByContainingAssembly))
			{
				memberIdentifier++;
			}

			ExplicitIndexerExpectationsExtensionsIndexerBuilder.BuildSetter(writer, property, memberIdentifier, containingTypeName, accessor);
		}
	}
}