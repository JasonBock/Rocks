using Microsoft.CodeAnalysis;
using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class IndexerExpectationsIndexerBuilder
{
	private static void BuildGetter(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, string expectationsFullyQualifiedName,
		Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		static void BuildGetterImplementation(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, bool isGeneratedWithDefaults, 
			string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
		{
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
				adornmentsType = $"global::Rocks.Runtime.Adornments<AdornmentsForHandler{memberIdentifier}, {handlerTypeName}, {callbackDelegateTypeName}>";
			}
			else
			{
				var returnType =
					property.Type.IsRefLikeType || property.Type.AllowsRefLikeType ?
						$"global::System.Func<{property.Type.FullyQualifiedName}>" :
						property.Type.FullyQualifiedName;

				adornmentsType = $"global::Rocks.Runtime.Adornments<AdornmentsForHandler{memberIdentifier}, {handlerTypeName}, {callbackDelegateTypeName}, {returnType}>";
			}

			adornmentsFQNsPipeline(new(adornmentsType, string.Empty, string.Empty, property.GetMethod!, memberIdentifier));

			var instanceParameters = string.Join(", ", propertyGetMethod.Parameters.Select(_ =>
				{
					var argumentTypeName = ProjectionBuilder.BuildArgument(
						_.Type, new TypeArgumentsNamingContext(), _.RequiresNullableAnnotation);

					if (_.Type.IsPointer)
					{
						return $"{argumentTypeName} @{_.Name}";
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
									$"{argumentTypeName} @{_.Name}";
							}
						}

						if (!isGeneratedWithDefaults)
						{
							// Only set this flag if we're currently not generating with defaults.
							needsGenerationWithDefaults |= _.HasExplicitDefaultValue;
						}

						return $"{argumentTypeName} @{_.Name}";
					}
				}));

			if (isGeneratedWithDefaults)
			{
				var parameterValues = string.Join(", ", propertyGetMethod.Parameters.Select(
					p => p.HasExplicitDefaultValue || p.IsParams ?
						$"global::Rocks.Runtime.Arg.Is(@{p.Name})" : $"@{p.Name}"));
				writer.WriteLines(
					$$"""
					internal {{expectationsFullyQualifiedName}}.Adornments.AdornmentsForHandler{{memberIdentifier}} This({{instanceParameters}}) =>
						this.This({{parameterValues}});
					""");
			}
			else
			{
				var handlerContext = new VariablesNamingContext(property.Parameters);
				writer.WriteLine($"internal {expectationsFullyQualifiedName}.Adornments.AdornmentsForHandler{memberIdentifier} This({instanceParameters})");
				writer.WriteLine("{");
				writer.Indent++;
				writer.WriteLine("global::Rocks.Runtime.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);");

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

					if (this.Expectations.handlers{{memberIdentifier}} is null) { this.Expectations.handlers{{memberIdentifier}} = new(@{{handlerContext["handler"]}}); }
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
		string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		static void BuildSetterImplementation(IndentedTextWriter writer, PropertyModel property, uint memberIdentifier, bool isGeneratedWithDefaults, 
			string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
		{
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
			var adornmentsType = $"global::Rocks.Runtime.Adornments<AdornmentsForHandler{memberIdentifier}, {expectationsFullyQualifiedName}.Handler{memberIdentifier}, {callbackDelegateTypeName}>";
			adornmentsFQNsPipeline(new(adornmentsType, string.Empty, string.Empty, property.SetMethod!, memberIdentifier));

			// We need to put the value parameter immediately after "self"
			// and then skip the value parameter by taking only the non-value parameters.
			var instanceParameters = string.Join(", ", valueParameter,
				string.Join(", ", propertySetMethod.Parameters.Take(propertySetMethod.Parameters.Length - 1).Select(_ =>
				{
					var argumentTypeName = ProjectionBuilder.BuildArgument(
						_.Type, new TypeArgumentsNamingContext(), _.RequiresNullableAnnotation);

					if (_.Type.IsPointer)
					{
						return $"{argumentTypeName} @{_.Name}";
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
									$"{argumentTypeName} @{_.Name}";
							}
						}

						if (!isGeneratedWithDefaults)
						{
							// Only set this flag if we're currently not generating with defaults.
							needsGenerationWithDefaults |= _.HasExplicitDefaultValue;
						}

						return $"{argumentTypeName} @{_.Name}";
					}
				})));

			if (isGeneratedWithDefaults)
			{
				// We need to put the value parameter first
				// and then skip the value parameter by taking only the non-value parameters.
				var parameterValues = string.Join(", ", $"@{propertySetMethod.Parameters[propertySetMethod.Parameters.Length - 1].Name}",
					string.Join(", ", propertySetMethod.Parameters.Take(propertySetMethod.Parameters.Length - 1).Select(
						p => p.HasExplicitDefaultValue || p.IsParams ?
							$"global::Rocks.Runtime.Arg.Is(@{p.Name})" : $"@{p.Name}")));
				writer.WriteLines(
					$$"""
					internal {{expectationsFullyQualifiedName}}.Adornments.AdornmentsForHandler{{memberIdentifier}} This({{instanceParameters}}) =>
						this.This({{parameterValues}});
					""");
			}
			else
			{
				var handlerContext = new VariablesNamingContext(property.Parameters);
				writer.WriteLine($"internal {expectationsFullyQualifiedName}.Adornments.AdornmentsForHandler{memberIdentifier} This({instanceParameters})");
				writer.WriteLine("{");
				writer.Indent++;
				writer.WriteLine("global::Rocks.Runtime.Exceptions.ExpectationException.ThrowIf(this.Expectations.WasInstanceInvoked);");

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

					if (this.Expectations.handlers{{memberIdentifier}} is null) { this.Expectations.handlers{{memberIdentifier}} = new(@{{handlerContext["handler"]}}); }
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
		Action<AdornmentsPipeline> adornmentsFQNsPipeline)
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