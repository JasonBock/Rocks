using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class IndexerExpectationsExtensionsIndexerBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier)
	{
		static void BuildGetterImplementation(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, bool isGeneratedWithDefaults)
		{
			var propertyGetMethod = property.GetMethod!;
			var namingContext = new VariableNamingContext(propertyGetMethod);
			var mockTypeName = property.MockType.FullyQualifiedName;
			var thisParameter = $"this global::Rocks.Expectations.IndexerGetterExpectations<{mockTypeName}> @{namingContext["self"]}";
			var needsGenerationWithDefaults = false;

			var delegateTypeName = propertyGetMethod.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
				DelegateBuilder.Build(property.Parameters, property.Type);
			var propertyReturnValue = propertyGetMethod.ReturnType.IsRefLikeType ?
				MockProjectedDelegateBuilder.GetProjectedReturnValueDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
				propertyGetMethod.ReturnType.FullyQualifiedName;
			var adornmentsType = propertyGetMethod.ReturnType.IsPointer ?
				$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, property.MockType, AdornmentType.Indexer, false)}<{mockTypeName}, {delegateTypeName}>" :
				$"global::Rocks.IndexerAdornments<{mockTypeName}, {delegateTypeName}, {propertyReturnValue}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			var instanceParameters = string.Join(", ", thisParameter,
				string.Join(", ", propertyGetMethod.Parameters.Select(_ =>
				{
					var requiresNullable = _.RequiresNullableAnnotation ? "?" : string.Empty;

					if (isGeneratedWithDefaults && _.HasExplicitDefaultValue)
					{
						return $"{_.Type.FullyQualifiedName}{requiresNullable} @{_.Name} = {_.ExplicitDefaultValue}";
					}

					if (!isGeneratedWithDefaults)
					{
						// Only set this flag if we're currently not generating with defaults.
						needsGenerationWithDefaults |= _.HasExplicitDefaultValue;
					}

					return $"global::Rocks.Argument<{_.Type.FullyQualifiedName}{requiresNullable}> @{_.Name}";
				})));

			if (isGeneratedWithDefaults)
			{
				var parameterValues = string.Join(", ", propertyGetMethod.Parameters.Select(
					p => p.HasExplicitDefaultValue ? $"global::Rocks.Arg.Is(@{p.Name})" : $"@{p.Name}"));
				writer.WriteLine($"internal static {returnValue} This({instanceParameters}) =>");
				writer.Indent++;
				writer.WriteLine($"@{namingContext["self"]}.This({parameterValues});");
				writer.Indent--;
			}
			else
			{
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

			if (needsGenerationWithDefaults)
			{
				BuildGetterImplementation(writer, property, memberIdentifier, true);
			}
		}

		BuildGetterImplementation(writer, property, memberIdentifier, false);
	}

	private static void BuildSetter(IndentedTextWriter writer, Models.PropertyModel property, uint memberIdentifier, PropertyAccessor accessor)
	{
		static void BuildSetterImplementation(IndentedTextWriter writer, Models.PropertyModel property, uint memberIdentifier, PropertyAccessor accessor, bool isGeneratedWithDefaults)
		{
			var propertySetMethod = property.SetMethod!;
			var namingContext = new VariableNamingContext(propertySetMethod);
			var mockTypeName = property.MockType.FullyQualifiedName;
			var accessorName = accessor == PropertyAccessor.Set ? "Setter" : "Initializer";
			var thisParameter = $"this global::Rocks.Expectations.Indexer{accessorName}Expectations<{mockTypeName}> @{namingContext["self"]}";
			var needsGenerationWithDefaults = false;

			var delegateTypeName = propertySetMethod.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertySetMethod, property.MockType) :
				DelegateBuilder.Build(propertySetMethod.Parameters);
			var adornmentsType = propertySetMethod.RequiresProjectedDelegate ?
				$"{MockProjectedTypesAdornmentsBuilder.GetProjectedAdornmentFullyQualifiedNameName(property.Type, property.MockType, AdornmentType.Indexer, false)}<{mockTypeName}, {delegateTypeName}>" :
				$"global::Rocks.IndexerAdornments<{mockTypeName}, {delegateTypeName}>";
			var (returnValue, newAdornments) = (adornmentsType, $"new {adornmentsType}");

			var instanceParameters = string.Join(", ", thisParameter,
				string.Join(", ", propertySetMethod.Parameters.Select(_ =>
				{
					var requiresNullable = _.RequiresNullableAnnotation ? "?" : string.Empty;

					if (isGeneratedWithDefaults && _.HasExplicitDefaultValue)
					{
						return $"{_.Type.FullyQualifiedName}{requiresNullable} @{_.Name} = {_.ExplicitDefaultValue}";
					}

					if (!isGeneratedWithDefaults)
					{
						// Only set this flag if we're currently not generating with defaults.
						needsGenerationWithDefaults |= _.HasExplicitDefaultValue;
					}

					return $"global::Rocks.Argument<{_.Type.FullyQualifiedName}{requiresNullable}> @{_.Name}";
				})));

			if (isGeneratedWithDefaults)
			{
				var parameterValues = string.Join(", ", propertySetMethod.Parameters.Select(
					p => p.HasExplicitDefaultValue ? $"global::Rocks.Arg.Is(@{p.Name})" : $"@{p.Name}"));
				writer.WriteLine($"internal static {returnValue} This({instanceParameters}) =>");
				writer.Indent++;
				writer.WriteLine($"@{namingContext["self"]}.This({parameterValues});");
				writer.Indent--;
			}
			else
			{
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

			if (needsGenerationWithDefaults)
			{
				BuildSetterImplementation(writer, property, memberIdentifier, accessor, true);
			}
		}

		BuildSetterImplementation(writer, property, memberIdentifier, accessor, false);
	}

	internal static void Build(IndentedTextWriter writer, PropertyModel property, PropertyAccessor accessor)
	{
		var memberIdentifier = property.MemberIdentifier;

		if (accessor == PropertyAccessor.Get)
		{
			IndexerExpectationsExtensionsIndexerBuilder.BuildGetter(writer, property, memberIdentifier);
		}
		else if (accessor == PropertyAccessor.Set || accessor == PropertyAccessor.Init)
		{
			if (property.Accessors == PropertyAccessor.GetAndSet ||
				property.Accessors == PropertyAccessor.GetAndInit)
			{
				memberIdentifier++;
			}

			IndexerExpectationsExtensionsIndexerBuilder.BuildSetter(writer, property, memberIdentifier, accessor);
		}
	}
}