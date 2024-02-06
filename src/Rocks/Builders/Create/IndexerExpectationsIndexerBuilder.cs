﻿using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using Rocks.Models;
using System.CodeDom.Compiler;

namespace Rocks.Builders.Create;

internal static class IndexerExpectationsIndexerBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, string expectationsFullyQualifiedName,
		Action<string, string, string> adornmentsFQNsPipeline)
	{
		static void BuildGetterImplementation(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, bool isGeneratedWithDefaults, 
			string expectationsFullyQualifiedName, Action<string, string, string> adornmentsFQNsPipeline)
		{
			var propertyGetMethod = property.GetMethod!;
			var namingContext = new VariableNamingContext(propertyGetMethod);
			var needsGenerationWithDefaults = false;

			var callbackDelegateTypeName = propertyGetMethod.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, property.MockType) :
				DelegateBuilder.Build(propertyGetMethod.Parameters, property.Type);

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

			var instanceParameters = string.Join(", ", propertyGetMethod.Parameters.Select(_ =>
				{
					if (_.Type.IsEsoteric)
					{
						var argName = 
							_.Type.IsRefLikeType ?
								RefLikeArgTypeBuilder.GetProjectedFullyQualifiedName(_.Type, _.MockType) :
								PointerArgTypeBuilder.GetProjectedFullyQualifiedName(_.Type, _.MockType);
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
				writer.WriteLines(
					$$"""
					internal {{adornmentsType}} This({{instanceParameters}}) =>
						this.This({{parameterValues}});
					""");
			}
			else
			{
				var handlerContext = new VariableNamingContext(property.Parameters);
				writer.WriteLine($"internal {adornmentsType} This({instanceParameters})");
				writer.WriteLine("{");
				writer.Indent++;
				writer.WriteLine("global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);");

				foreach (var parameter in propertyGetMethod.Parameters)
				{
					writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{parameter.Name});");
				}

				writer.WriteLine();
				writer.WriteLines(
					$$"""
					var @{{handlerContext["handler"]}} = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}
					{
					""");
				writer.Indent++;

				var handlerNamingContext = HandlerVariableNamingContext.Create();

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

					if (this.Expectations.handlers{{memberIdentifier}} is null ) { this.Expectations.handlers{{memberIdentifier}} = new(@{{handlerContext["handler"]}}); }
					else { this.Expectations.handlers{{memberIdentifier}}.Add(@{{handlerContext["handler"]}}); }
					return new(@{{handlerContext["handler"]}});
					""");

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (needsGenerationWithDefaults)
			{
				BuildGetterImplementation(writer, property, memberIdentifier, true, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
			}
		}

		BuildGetterImplementation(writer, property, memberIdentifier, false, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
	}

	private static void BuildSetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, 
		string expectationsFullyQualifiedName, Action<string, string, string> adornmentsFQNsPipeline)
	{
		static void BuildSetterImplementation(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, bool isGeneratedWithDefaults, 
			string expectationsFullyQualifiedName, Action<string, string, string> adornmentsFQNsPipeline)
		{
			var propertySetMethod = property.SetMethod!;
			var namingContext = new VariableNamingContext(propertySetMethod);

			var lastParameter = propertySetMethod.Parameters[propertySetMethod.Parameters.Length - 1];
			var lastParameterRequiresNullable = lastParameter.RequiresNullableAnnotation ? "?" : string.Empty;
			var valueParameterArgument =
				lastParameter.Type.IsEsoteric ?
					lastParameter.Type.IsRefLikeType ?
						RefLikeArgTypeBuilder.GetProjectedFullyQualifiedName(lastParameter.Type, property.MockType) :
						PointerArgTypeBuilder.GetProjectedFullyQualifiedName(lastParameter.Type, property.MockType) :
					$"global::Rocks.Argument<{lastParameter.Type.FullyQualifiedName}{lastParameterRequiresNullable}>";
			var valueParameter = $"{valueParameterArgument} @{lastParameter.Name}";

			var needsGenerationWithDefaults = false;

			var callbackDelegateTypeName = propertySetMethod.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertySetMethod, property.MockType) :
				DelegateBuilder.Build(propertySetMethod.Parameters);
			var adornmentsType = $"global::Rocks.Adornments<{expectationsFullyQualifiedName}.Handler{memberIdentifier}, {callbackDelegateTypeName}>";
			adornmentsFQNsPipeline(adornmentsType, string.Empty, string.Empty);

			// We need to put the value parameter immediately after "self"
			// and then skip the value parameter by taking only the non-value parameters.
			var instanceParameters = string.Join(", ", valueParameter,
				string.Join(", ", propertySetMethod.Parameters.Take(propertySetMethod.Parameters.Length - 1).Select(_ =>
				{
					if (_.Type.IsEsoteric)
					{
						var argName =
							_.Type.IsRefLikeType ?
								RefLikeArgTypeBuilder.GetProjectedFullyQualifiedName(_.Type, _.MockType) :
								PointerArgTypeBuilder.GetProjectedFullyQualifiedName(_.Type, _.MockType);
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
				writer.WriteLines(
					$$"""
					internal {{adornmentsType}} This({{instanceParameters}}) =>
						this.This({{parameterValues}});
					""");
			}
			else
			{
				var handlerContext = new VariableNamingContext(property.Parameters);
				writer.WriteLine($"internal {adornmentsType} This({instanceParameters})");
				writer.WriteLine("{");
				writer.Indent++;
				writer.WriteLine("global::Rocks.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);");

				foreach (var parameter in propertySetMethod.Parameters)
				{
					writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{parameter.Name});");
				}

				writer.WriteLine();
				writer.WriteLines(
					$$"""
					var @{{handlerContext["handler"]}} = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}
					{
					""");
				writer.Indent++;

				var handlerNamingContext = HandlerVariableNamingContext.Create();

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

					if (this.Expectations.handlers{{memberIdentifier}} is null ) { this.Expectations.handlers{{memberIdentifier}} = new(@{{handlerContext["handler"]}}); }
					else { this.Expectations.handlers{{memberIdentifier}}.Add(@{{handlerContext["handler"]}}); }
					return new(@{{handlerContext["handler"]}});
					""");

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (needsGenerationWithDefaults)
			{
				BuildSetterImplementation(writer, property, memberIdentifier, true, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
			}
		}

		BuildSetterImplementation(writer, property, memberIdentifier, false, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
	}

	internal static void Build(IndentedTextWriter writer, PropertyModel property, PropertyAccessor accessor, string expectationsFullyQualifiedName,
		Action<string, string, string> adornmentsFQNsPipeline)
	{
		var memberIdentifier = property.MemberIdentifier;

		if (accessor == PropertyAccessor.Get && property.GetCanBeSeenByContainingAssembly)
		{
			IndexerExpectationsIndexerBuilder.BuildGetter(writer, property, memberIdentifier, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
		}
		else if ((accessor == PropertyAccessor.Set && property.SetCanBeSeenByContainingAssembly) ||
			(accessor == PropertyAccessor.Init && property.InitCanBeSeenByContainingAssembly))
		{
			if ((property.Accessors == PropertyAccessor.GetAndSet || property.Accessors == PropertyAccessor.GetAndInit) &&
				property.GetCanBeSeenByContainingAssembly)
			{
				memberIdentifier++;
			}

			IndexerExpectationsIndexerBuilder.BuildSetter(writer, property, memberIdentifier, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
		}
	}
}