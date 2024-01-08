using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class IndexerExpectationsIndexerBuilderV4
{
	private static void BuildGetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, string expectationsFullyQualifiedName)
	{
		static void BuildGetterImplementation(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, bool isGeneratedWithDefaults, string expectationsFullyQualifiedName)
		{
			var propertyGetMethod = property.GetMethod!;
			var namingContext = new VariableNamingContext(propertyGetMethod);
			var needsGenerationWithDefaults = false;

			var callbackDelegateTypeName = propertyGetMethod.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilderV4.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
				DelegateBuilder.Build(propertyGetMethod.Parameters, property.Type);

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

			var instanceParameters = string.Join(", ", propertyGetMethod.Parameters.Select(_ =>
				{
					if (_.Type.IsEsoteric)
					{
						var argName = ProjectedArgTypeBuilderV4.GetProjectedFullyQualifiedName(_.Type, _.MockType);
						return $"{argName} @{_.Name}";
					}
					else
					{
						var requiresNullable = _.RequiresNullableAnnotation ? "?" : string.Empty;

						if (isGeneratedWithDefaults)
						{
							if (_.HasExplicitDefaultValue)
							{
								return $"{_.Type.FullyQualifiedName}{requiresNullable} @{_.Name} = {_.ExplicitDefaultValue}";
							}
							else
							{
								return _.IsParams ?
									$"params {_.Type.FullyQualifiedName}{requiresNullable} @{_.Name}" :
									$"global::Rocks.Argument<{_.Type.FullyQualifiedName}{requiresNullable}> @{_.Name}";
							}
						}

						if (!isGeneratedWithDefaults)
						{
							// Only set this flag if we're currently not generating with defaults.
							needsGenerationWithDefaults |= _.HasExplicitDefaultValue;
						}

						return $"global::Rocks.Argument<{_.Type.FullyQualifiedName}{requiresNullable}> @{_.Name}";
					}
				}));

			if (isGeneratedWithDefaults)
			{
				var parameterValues = string.Join(", ", propertyGetMethod.Parameters.Select(
					p => p.HasExplicitDefaultValue || p.IsParams ?
						$"global::Rocks.Arg.Is(@{p.Name})" : $"@{p.Name}"));
				writer.WriteLine($"internal {adornmentsType} This({instanceParameters}) =>");
				writer.Indent++;
				writer.WriteLine($"this.This({parameterValues});");
				writer.Indent--;
			}
			else
			{
				writer.WriteLine($"internal {adornmentsType} This({instanceParameters})");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var parameter in propertyGetMethod.Parameters)
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

				foreach (var parameter in propertyGetMethod.Parameters)
				{
					if (parameter.HasExplicitDefaultValue && property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
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

			if (needsGenerationWithDefaults)
			{
				BuildGetterImplementation(writer, property, memberIdentifier, true, expectationsFullyQualifiedName);
			}
		}

		BuildGetterImplementation(writer, property, memberIdentifier, false, expectationsFullyQualifiedName);
	}

	private static void BuildSetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, string expectationsFullyQualifiedName)
	{
		static void BuildSetterImplementation(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, bool isGeneratedWithDefaults, string expectationsFullyQualifiedName)
		{
			var propertySetMethod = property.SetMethod!;
			var namingContext = new VariableNamingContext(propertySetMethod);

			var lastParameter = propertySetMethod.Parameters[propertySetMethod.Parameters.Length - 1];
			var lastParameterRequiresNullable = lastParameter.RequiresNullableAnnotation ? "?" : string.Empty;
			var valueParameterArgument =
				lastParameter.Type.IsEsoteric ?
					ProjectedArgTypeBuilderV4.GetProjectedFullyQualifiedName(lastParameter.Type, property.MockType) :
					$"global::Rocks.Argument<{lastParameter.Type.FullyQualifiedName}{lastParameterRequiresNullable}>";
			var valueParameter = $"{valueParameterArgument} @{lastParameter.Name}";

			var needsGenerationWithDefaults = false;

			var callbackDelegateTypeName = propertySetMethod.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilderV4.GetProjectedCallbackDelegateFullyQualifiedName(propertySetMethod, property.MockType) :
				DelegateBuilder.Build(propertySetMethod.Parameters);
			var adornmentsType = $"global::Rocks.AdornmentsV4<{expectationsFullyQualifiedName}.Handler{memberIdentifier}, {callbackDelegateTypeName}>";

			// We need to put the value parameter immediately after "self"
			// and then skip the value parameter by taking only the non-value parameters.
			var instanceParameters = string.Join(", ", valueParameter,
				string.Join(", ", propertySetMethod.Parameters.Take(propertySetMethod.Parameters.Length - 1).Select(_ =>
				{
					if (_.Type.IsEsoteric)
					{
						var argName = ProjectedArgTypeBuilderV4.GetProjectedFullyQualifiedName(_.Type, _.MockType);
						return $"{argName} @{_.Name}";
					}
					else
					{
						var requiresNullable = _.RequiresNullableAnnotation ? "?" : string.Empty;

						if (isGeneratedWithDefaults)
						{
							if (_.HasExplicitDefaultValue)
							{
								return $"{_.Type.FullyQualifiedName}{requiresNullable} @{_.Name} = {_.ExplicitDefaultValue}";
							}
							else
							{
								return _.IsParams ?
									$"params {_.Type.FullyQualifiedName}{requiresNullable} @{_.Name}" :
									$"global::Rocks.Argument<{_.Type.FullyQualifiedName}{requiresNullable}> @{_.Name}";
							}
						}

						if (!isGeneratedWithDefaults)
						{
							// Only set this flag if we're currently not generating with defaults.
							needsGenerationWithDefaults |= _.HasExplicitDefaultValue;
						}

						return $"global::Rocks.Argument<{_.Type.FullyQualifiedName}{requiresNullable}> @{_.Name}";
					}
				})));

			if (isGeneratedWithDefaults)
			{
				// We need to put the value parameter first
				// and then skip the value parameter by taking only the non-value parameters.
				var parameterValues = string.Join(", ", $"@{propertySetMethod.Parameters[propertySetMethod.Parameters.Length - 1].Name}",
					string.Join(", ", propertySetMethod.Parameters.Take(propertySetMethod.Parameters.Length - 1).Select(
						p => p.HasExplicitDefaultValue || p.IsParams ?
							$"global::Rocks.Arg.Is(@{p.Name})" : $"@{p.Name}")));
				writer.WriteLine($"internal {adornmentsType} This({instanceParameters}) =>");
				writer.Indent++;
				writer.WriteLine($"this.This({parameterValues});");
				writer.Indent--;
			}
			else
			{
				writer.WriteLine($"internal {adornmentsType} This({instanceParameters})");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (var parameter in propertySetMethod.Parameters)
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

				foreach (var parameter in propertySetMethod.Parameters)
				{
					if (parameter.HasExplicitDefaultValue && property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
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

			if (needsGenerationWithDefaults)
			{
				BuildSetterImplementation(writer, property, memberIdentifier, true, expectationsFullyQualifiedName);
			}
		}

		BuildSetterImplementation(writer, property, memberIdentifier, false, expectationsFullyQualifiedName);
	}

	internal static void Build(IndentedTextWriter writer, PropertyModel property, PropertyAccessor accessor, string expectationsFullyQualifiedName)
	{
		var memberIdentifier = property.MemberIdentifier;

		if (accessor == PropertyAccessor.Get && property.GetCanBeSeenByContainingAssembly)
		{
			IndexerExpectationsIndexerBuilderV4.BuildGetter(writer, property, memberIdentifier, expectationsFullyQualifiedName);
		}
		else if ((accessor == PropertyAccessor.Set && property.SetCanBeSeenByContainingAssembly) ||
			(accessor == PropertyAccessor.Init && property.InitCanBeSeenByContainingAssembly))
		{
			if ((property.Accessors == PropertyAccessor.GetAndSet || property.Accessors == PropertyAccessor.GetAndInit) &&
				property.GetCanBeSeenByContainingAssembly)
			{
				memberIdentifier++;
			}

			IndexerExpectationsIndexerBuilderV4.BuildSetter(writer, property, memberIdentifier, expectationsFullyQualifiedName);
		}
	}
}