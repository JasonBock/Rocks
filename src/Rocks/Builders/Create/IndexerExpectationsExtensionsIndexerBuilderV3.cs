using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class IndexerExpectationsExtensionsIndexerBuilderV3
{
	private static void BuildGetter(IndentedTextWriter writer, Models.PropertyModel property, uint memberIdentifier)
	{
		var propertyGetMethod = property.GetMethod!;
		var namingContext = new VariableNamingContextV3(propertyGetMethod);
		var mockTypeName = property.MockType.FullyQualifiedName;
		var thisParameter = $"this global::Rocks.Expectations.IndexerGetterExpectations<{mockTypeName}> @{namingContext["self"]}";

		var delegateTypeName = propertyGetMethod.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilderV3.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
			DelegateBuilderV3.Build(property.Parameters, property.Type);
		var propertyReturnValue = propertyGetMethod.ReturnType.IsRefLikeType ?
			MockProjectedDelegateBuilderV3.GetProjectedReturnValueDelegateFullyQualifiedName(propertyGetMethod, property.MockType) : 
			propertyGetMethod.ReturnType.FullyQualifiedName;
		var adornmentsType = propertyGetMethod.ReturnType.IsPointer ?
			$"{MockProjectedTypesAdornmentsBuilderV3.GetProjectedAdornmentFullyQualifiedNameName(property.Type, property.MockType, AdornmentType.Indexer, false)}<{mockTypeName}, {delegateTypeName}>" :
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
			_ => _.HasExplicitDefaultValue ? $"@{_.Name}.Transform({_.ExplicitDefaultValue})" : $"@{_.Name}"));
		var addMethod = property.Type.IsPointer ?
			MockProjectedTypesAdornmentsBuilderV3.GetProjectedAddExtensionMethodFullyQualifiedName(property.Type, property.MockType) : 
			$"Add<{propertyReturnValue}>";
		writer.WriteLine($"return {newAdornments}(@{namingContext["self"]}.{addMethod}({memberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>({propertyGetMethod.Parameters.Length}) {{ {parameters} }}));");

		writer.Indent--;
		writer.WriteLine("}");
	}

	private static void BuildSetter(IndentedTextWriter writer, Models.PropertyModel property, uint memberIdentifier, PropertyAccessor accessor)
	{
		var propertySetMethod = property.SetMethod!;
		var namingContext = new VariableNamingContextV3(propertySetMethod);
		var mockTypeName = property.MockType.FullyQualifiedName;
		var accessorName = accessor == PropertyAccessor.Set ? "Setter" : "Initializer";
		var thisParameter = $"this global::Rocks.Expectations.Indexer{accessorName}Expectations<{mockTypeName}> @{namingContext["self"]}";
		var delegateTypeName = propertySetMethod.RequiresProjectedDelegate ?
			MockProjectedDelegateBuilderV3.GetProjectedCallbackDelegateFullyQualifiedName(propertySetMethod, property.MockType) :
			DelegateBuilderV3.Build(propertySetMethod.Parameters);
		var adornmentsType = propertySetMethod.RequiresProjectedDelegate ?
			$"{MockProjectedTypesAdornmentsBuilderV3.GetProjectedAdornmentFullyQualifiedNameName(property.Type, property.MockType, AdornmentType.Indexer, false)}<{mockTypeName}, {delegateTypeName}>" :
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
			_ => _.HasExplicitDefaultValue ? $"@{_.Name}.Transform({_.ExplicitDefaultValue})" : $"@{_.Name}"));
		writer.WriteLine($"return {newAdornments}({namingContext["self"]}.Add({memberIdentifier}, new global::System.Collections.Generic.List<global::Rocks.Argument>({propertySetMethod.Parameters.Length}) {{ {parameters} }}));");

		writer.Indent--;
		writer.WriteLine("}");
	}

	internal static void Build(IndentedTextWriter writer, Models.PropertyModel property, PropertyAccessor accessor)
	{
		var memberIdentifier = property.MemberIdentifier;

		if (accessor == PropertyAccessor.Get)
		{
			IndexerExpectationsExtensionsIndexerBuilderV3.BuildGetter(writer, property, memberIdentifier);
		}
		else if(accessor == PropertyAccessor.Set || accessor == PropertyAccessor.Init)
		{
			if (property.Accessors == PropertyAccessor.GetAndSet ||
				property.Accessors == PropertyAccessor.GetAndInit)
			{
				memberIdentifier++;
			}

			IndexerExpectationsExtensionsIndexerBuilderV3.BuildSetter(writer, property, memberIdentifier, accessor);
		}
	}
}