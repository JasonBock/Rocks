using Microsoft.CodeAnalysis;
using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;
using System.Reflection.Metadata;

namespace Rocks.Analysis.Builders.Create;

internal static class IndexerExpectationsIndexerBuilder
{
	private static IndexerConstructorDefaultValues? BuildGetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier,
		string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		// isGeneratedWithDefaults really means, "do we need to generate an overload
		// for the given method because it has optional and/or params parameters".
		static IndexerConstructorDefaultValues? BuildGetterImplementation(IndentedTextWriter writer, PropertyModel property,
			uint memberIdentifier, bool isGeneratedWithDefaults,
			string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
		{
			static string GetOptionalParameter(ParameterModel parameter, ParameterModel lastParameter,
				string typeName, string requiresNullable) =>
					lastParameter.IsParams && lastParameter.Type.IsRefLikeType ?
						$"[global::System.Runtime.InteropServices.Optional, global::System.Runtime.InteropServices.DefaultParameterValue({parameter.ExplicitDefaultValue})] {typeName}{requiresNullable} @{parameter.Name}" :
						parameter.AttributesDescription.Contains("Optional") ?
							$"[global::System.Runtime.InteropServices.Optional, global::System.Runtime.InteropServices.DefaultParameterValue({parameter.ExplicitDefaultValue})] {typeName}{requiresNullable} @{parameter.Name}" :
							$"{typeName}{requiresNullable} @{parameter.Name} = {parameter.ExplicitDefaultValue}";

			var propertyGetMethod = property.GetMethod!;
			var namingContext = new VariablesNamingContext(propertyGetMethod);
			var needsGenerationWithDefaults = false;

			var callbackDelegateTypeName = propertyGetMethod.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, property.MockType, expectationsFullyQualifiedName, memberIdentifier) :
				DelegateBuilder.Build(propertyGetMethod);

			var handlerTypeName = $"{expectationsFullyQualifiedName}.Handler{memberIdentifier}";

			string adornmentsType;

			if (property.Type.IsPointer)
			{
				adornmentsType = $"global::Rocks.Adornments<AdornmentsForHandler{memberIdentifier}, {handlerTypeName}, {callbackDelegateTypeName}>";
			}
			else
			{
				var returnType =
					property.Type.IsRefLikeType || property.Type.AllowsRefLikeType ?
						$"global::System.Func<{property.Type.FullyQualifiedName}>" :
						property.Type.FullyQualifiedName;

				adornmentsType = $"global::Rocks.Adornments<AdornmentsForHandler{memberIdentifier}, {handlerTypeName}, {callbackDelegateTypeName}, {returnType}>";
			}

			adornmentsFQNsPipeline(new(adornmentsType, string.Empty, string.Empty, property.GetMethod!, memberIdentifier));

			var instanceParameters = string.Join(", ", propertyGetMethod.Parameters.Select(parameter =>
				{
					var argumentTypeName = ProjectionBuilder.BuildArgument(
						parameter.Type, new TypeArgumentsNamingContext(), parameter.RequiresNullableAnnotation);

					if (parameter.Type.IsPointer)
					{
						return $"{argumentTypeName} @{parameter.Name}";
					}
					else
					{
						var requiresNullable = parameter.RequiresNullableAnnotation ? "?" : string.Empty;

						if (isGeneratedWithDefaults)
						{
							return parameter.HasExplicitDefaultValue ?
								GetOptionalParameter(parameter, propertyGetMethod.Parameters[propertyGetMethod.Parameters.Length - 1], parameter.Type.FullyQualifiedName, requiresNullable) :
								parameter.IsParams ?
									parameter.Type.IsRefLikeType ?
										$"global::Rocks.RefStructArgument<{parameter.Type.FullyQualifiedName}> @{parameter.Name}" :
										$"params {parameter.Type.FullyQualifiedName}{requiresNullable} @{parameter.Name}" :
									$"{argumentTypeName} @{parameter.Name}";
						}

						if (!isGeneratedWithDefaults)
						{
							// Only set this flag if we're currently not generating with defaults.
							needsGenerationWithDefaults |= parameter.HasExplicitDefaultValue ||
								(parameter.IsParams && !parameter.Type.IsRefLikeType);
						}

						return $"{argumentTypeName} @{parameter.Name}";
					}
				}));


			if (isGeneratedWithDefaults)
			{
				var parameterValues = string.Join(", ", propertyGetMethod.Parameters.Select(
					parameter => parameter.HasExplicitDefaultValue || parameter.IsParams ?
						parameter.Type.IsRefLikeType ?
							$"@{parameter.Name}" :
							$"global::Rocks.Arg.Is(@{parameter.Name})" :
						$"@{parameter.Name}"));
				return new(instanceParameters, parameterValues);
			}
			else
			{
				var handlerContext = new VariablesNamingContext(property.Parameters);
				writer.WriteLine($"internal {expectationsFullyQualifiedName}.Adornments.AdornmentsForHandler{memberIdentifier} Gets()");
				writer.WriteLine("{");
				writer.Indent++;
				writer.WriteLine("global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);");
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
						writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = this.@{parameter.Name}.Transform({parameter.ExplicitDefaultValue}),");
					}
					else
					{
						writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = this.@{parameter.Name},");
					}
				}

				writer.Indent--;
				writer.WriteLines(
					$$"""
					};

					if (this.parent.handlers{{memberIdentifier}} is null) { this.parent.handlers{{memberIdentifier}} = new(@{{handlerContext["handler"]}}); }
					else { this.parent.handlers{{memberIdentifier}}.Add(@{{handlerContext["handler"]}}); }
					return new(@{{handlerContext["handler"]}});
					""");

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (needsGenerationWithDefaults)
			{
				return BuildGetterImplementation(writer, property, memberIdentifier, true, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
			}

			return null;
		}

		return BuildGetterImplementation(writer, property, memberIdentifier, false, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
	}

	private static IndexerConstructorDefaultValues? BuildSetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier,
		string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		static IndexerConstructorDefaultValues? BuildSetterImplementation(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, bool isGeneratedWithDefaults,
			string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
		{
			static string GetOptionalParameter(ParameterModel parameter, ParameterModel lastParameter,
				string typeName, string requiresNullable) =>
					lastParameter.IsParams && lastParameter.Type.IsRefLikeType ?
						$"[global::System.Runtime.InteropServices.Optional, global::System.Runtime.InteropServices.DefaultParameterValue({parameter.ExplicitDefaultValue})] {typeName}{requiresNullable} @{parameter.Name}" :
						parameter.AttributesDescription.Contains("Optional") ?
							$"[global::System.Runtime.InteropServices.Optional, global::System.Runtime.InteropServices.DefaultParameterValue({parameter.ExplicitDefaultValue})] {typeName}{requiresNullable} @{parameter.Name}" :
							$"{typeName}{requiresNullable} @{parameter.Name} = {parameter.ExplicitDefaultValue}";

			var propertySetMethod = property.SetMethod!;
			var namingContext = new VariablesNamingContext(propertySetMethod);

			var lastParameter = propertySetMethod.Parameters[propertySetMethod.Parameters.Length - 1];
			var valueParameterArgument = ProjectionBuilder.BuildArgument(
				lastParameter.Type, new TypeArgumentsNamingContext(), lastParameter.RequiresNullableAnnotation);
			var valueParameter = $"{valueParameterArgument} @{lastParameter.Name}";

			var needsGenerationWithDefaults = false;

			var callbackDelegateTypeName = propertySetMethod.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(property.SetMethod!, property.MockType, expectationsFullyQualifiedName, memberIdentifier) :
				DelegateBuilder.Build(propertySetMethod);
			var adornmentsType = $"global::Rocks.Adornments<AdornmentsForHandler{memberIdentifier}, {expectationsFullyQualifiedName}.Handler{memberIdentifier}, {callbackDelegateTypeName}>";
			adornmentsFQNsPipeline(new(adornmentsType, string.Empty, string.Empty, property.SetMethod!, memberIdentifier));

			// We need to put the value parameter immediately after "self"
			// and then skip the value parameter by taking only the non-value parameters.
			var instanceParameters = string.Join(", ", propertySetMethod.Parameters.Take(propertySetMethod.Parameters.Length - 1).Select(parameter =>
				{
					var argumentTypeName = ProjectionBuilder.BuildArgument(
						parameter.Type, new TypeArgumentsNamingContext(), parameter.RequiresNullableAnnotation);

					if (parameter.Type.IsPointer)
					{
						return $"{argumentTypeName} @{parameter.Name}";
					}
					else
					{
						var requiresNullable = parameter.RequiresNullableAnnotation ? "?" : string.Empty;

						if (isGeneratedWithDefaults)
						{
							return parameter.HasExplicitDefaultValue ?
								GetOptionalParameter(parameter, propertySetMethod.Parameters[propertySetMethod.Parameters.Length - 2], parameter.Type.FullyQualifiedName, requiresNullable) :
								parameter.IsParams ?
									parameter.Type.IsRefLikeType ?
										$"global::Rocks.RefStructArgument<{parameter.Type.FullyQualifiedName}> @{parameter.Name}" :
										$"params {parameter.Type.FullyQualifiedName}{requiresNullable} @{parameter.Name}" :
									$"{argumentTypeName} @{parameter.Name}";
						}

						if (!isGeneratedWithDefaults)
						{
							// Only set this flag if we're currently not generating with defaults.
							needsGenerationWithDefaults |= parameter.HasExplicitDefaultValue ||
								(parameter.IsParams && !parameter.Type.IsRefLikeType);
						}

						return $"{argumentTypeName} @{parameter.Name}";
					}
				}));

			var name = property.Accessors == PropertyAccessor.Set || property.Accessors == PropertyAccessor.GetAndSet ?
				"Sets" : "Inits";

			if (isGeneratedWithDefaults)
			{
				// We need to skip the value parameter by taking only the non-value parameters.
				var parameterValues = string.Join(", ", propertySetMethod.Parameters.Take(propertySetMethod.Parameters.Length - 1).Select(
					parameter => parameter.HasExplicitDefaultValue || parameter.IsParams ?
						parameter.Type.IsRefLikeType ?
							$"@{parameter.Name}" :
							$"global::Rocks.Arg.Is(@{parameter.Name})" :
						$"@{parameter.Name}"));
				return new(instanceParameters, parameterValues);
			}
			else
			{
				var handlerContext = new VariablesNamingContext(property.Parameters);
				writer.WriteLine($"internal {expectationsFullyQualifiedName}.Adornments.AdornmentsForHandler{memberIdentifier} {name}({valueParameter})");
				writer.WriteLine("{");
				writer.Indent++;
				writer.WriteLine("global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);");
				writer.WriteLine($"global::System.ArgumentNullException.ThrowIfNull(@{lastParameter.Name});");

				writer.WriteLine();
				writer.WriteLines(
					$$"""
					var @{{handlerContext["handler"]}} = new {{expectationsFullyQualifiedName}}.Handler{{memberIdentifier}}
					{
					""");
				writer.Indent++;

				var handlerNamingContext = HandlerVariableNamingContext.Create();

				for (var i = 0; i < propertySetMethod.Parameters.Length - 1; i++)
				{
					var parameter = propertySetMethod.Parameters[i];

					if (parameter.HasExplicitDefaultValue && property.RequiresExplicitInterfaceImplementation == RequiresExplicitInterfaceImplementation.No)
					{
						writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = this.@{parameter.Name}.Transform({parameter.ExplicitDefaultValue}),");
					}
					else
					{
						writer.WriteLine($"@{handlerNamingContext[parameter.Name]} = this.@{parameter.Name},");
					}
				}

				writer.WriteLine($"@value = @value,");

				writer.Indent--;
				writer.WriteLines(
					$$"""
					};

					if (this.parent.handlers{{memberIdentifier}} is null) { this.parent.handlers{{memberIdentifier}} = new(@{{handlerContext["handler"]}}); }
					else { this.parent.handlers{{memberIdentifier}}.Add(@{{handlerContext["handler"]}}); }
					return new(@{{handlerContext["handler"]}});
					""");

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (needsGenerationWithDefaults)
			{
				return BuildSetterImplementation(writer, property, memberIdentifier, true, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
			}

			return null;
		}

		return BuildSetterImplementation(writer, property, memberIdentifier, false, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
	}

	internal static IndexerConstructorDefaultValues? Build(IndentedTextWriter writer, PropertyModel property,
		string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		var memberIdentifier = property.MemberIdentifier;
		var wasGetGenerated = false;

		IndexerConstructorDefaultValues? constructorValues = null;

		if ((property.Accessors == PropertyAccessor.Get || property.Accessors == PropertyAccessor.GetAndSet || property.Accessors == PropertyAccessor.GetAndInit) &&
			property.GetCanBeSeenByContainingAssembly)
		{
			constructorValues = BuildGetter(writer, property, memberIdentifier, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
			wasGetGenerated = true;
		}

		if (property.Accessors == PropertyAccessor.GetAndSet || property.Accessors != PropertyAccessor.GetAndInit)
		{
			writer.WriteLine();
		}

		if (((property.Accessors == PropertyAccessor.Set || property.Accessors == PropertyAccessor.GetAndSet) && property.SetCanBeSeenByContainingAssembly) ||
			((property.Accessors == PropertyAccessor.Init || property.Accessors == PropertyAccessor.GetAndInit) && property.InitCanBeSeenByContainingAssembly))
		{
			if (wasGetGenerated)
			{
				memberIdentifier++;
			}

			constructorValues = BuildSetter(writer, property, memberIdentifier, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
		}

		return constructorValues;
	}
}