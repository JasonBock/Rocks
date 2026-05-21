using Microsoft.CodeAnalysis;
using Rocks.Analysis.Extensions;
using Rocks.Analysis.Models;
using System.CodeDom.Compiler;

namespace Rocks.Analysis.Builders.Create;

internal static class IndexerExpectationsIndexerBuilder
{
	private static IndexerConstructorDefaultValues? BuildGetter(IndentedTextWriter writer, TypeMockModel mockType, PropertyModel property, 
		uint memberIdentifier, string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		// isGeneratedWithDefaults really means, "do we need to generate an overload
		// for the given method because it has optional and/or params parameters".
		static IndexerConstructorDefaultValues? BuildGetterImplementation(IndentedTextWriter writer, TypeMockModel mockType, PropertyModel property,
			uint memberIdentifier, bool isGeneratedWithDefaults,
			string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
		{
			var propertyGetMethod = property.GetMethod!;
			var namingContext = new VariablesNamingContext(propertyGetMethod);
			var needsGenerationWithDefaults = false;

			var callbackDelegateTypeName = propertyGetMethod.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertyGetMethod, expectationsFullyQualifiedName, memberIdentifier) :
				DelegateBuilder.Build(propertyGetMethod);

			var handlerTypeName = $"{expectationsFullyQualifiedName}.Handler{memberIdentifier}";
			var adornmentsTypeName = $"thisGetsAdornments{propertyGetMethod.Hash}";

			string adornmentsType;

			if (property.Type.IsPointer)
			{
				adornmentsType = $"global::Rocks.Adornments<{adornmentsTypeName}, {handlerTypeName}, {callbackDelegateTypeName}>";
			}
			else
			{
				var returnType =
					property.Type.IsRefLikeType || property.Type.AllowsRefLikeType ?
						$"global::System.Func<{property.Type.FullyQualifiedName}>" :
						property.Type.FullyQualifiedName;

				adornmentsType = $"global::Rocks.Adornments<{adornmentsTypeName}, {handlerTypeName}, {callbackDelegateTypeName}, {returnType}>";
			}

			adornmentsFQNsPipeline(new(adornmentsTypeName, adornmentsType, string.Empty, string.Empty, propertyGetMethod, memberIdentifier));

			var instanceParameters = string.Join(", ", propertyGetMethod.Parameters.Take(propertyGetMethod.Parameters.Length - 1).Select(parameter =>
			{
				var (expectationParameter, needs) = parameter.GetExpectationParameter(
					isGeneratedWithDefaults, propertyGetMethod.Parameters[propertyGetMethod.Parameters.Length - 1], new TypeArgumentsNamingContext());
				needsGenerationWithDefaults |= needs;
				return expectationParameter;
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
				writer.WriteLine($"{mockType.Accessibility} {expectationsFullyQualifiedName}.Adornments.{adornmentsTypeName} Gets()");
				writer.WriteLine("{");
				writer.Indent++;
				writer.WriteLines(
					$$"""
					global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);

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

					if (this.parent.handlers{{memberIdentifier}} is null) { this.parent.handlers{{memberIdentifier}} = new(1); }
					this.parent.handlers{{memberIdentifier}}.Add(@{{handlerContext["handler"]}});
					return new(@{{handlerContext["handler"]}}, this.parent);
					""");

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (needsGenerationWithDefaults)
			{
				return BuildGetterImplementation(writer, mockType, property, memberIdentifier, true, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
			}

			return null;
		}

		return BuildGetterImplementation(writer, mockType, property, memberIdentifier, false, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
	}

	private static IndexerConstructorDefaultValues? BuildSetter(IndentedTextWriter writer, TypeMockModel mockType, PropertyModel property, 
		uint memberIdentifier, string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		static IndexerConstructorDefaultValues? BuildSetterImplementation(IndentedTextWriter writer, TypeMockModel mockType, PropertyModel property, 
			uint memberIdentifier, bool isGeneratedWithDefaults,
			string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
		{
			var propertySetMethod = property.SetMethod!;
			var namingContext = new VariablesNamingContext(propertySetMethod);
			var typeArgumentsNamingContext = new TypeArgumentsNamingContext();

			var lastParameter = propertySetMethod.Parameters[propertySetMethod.Parameters.Length - 1];
			var valueParameterArgument = ProjectionBuilder.BuildArgument(
				lastParameter.Type, typeArgumentsNamingContext, lastParameter.RequiresNullableAnnotation);
			var valueParameter = $"{valueParameterArgument} @{lastParameter.Name}";

			var needsGenerationWithDefaults = false;

			var callbackDelegateTypeName = propertySetMethod.RequiresProjectedDelegate ?
				MockProjectedDelegateBuilder.GetProjectedCallbackDelegateFullyQualifiedName(propertySetMethod, expectationsFullyQualifiedName, memberIdentifier) :
				DelegateBuilder.Build(propertySetMethod);
			var name = property.Accessors == PropertyAccessor.Set || property.Accessors == PropertyAccessor.GetAndSet ?
				"Sets" : "Inits";
			var adornmentsTypeName = $"this{name}Adornments{propertySetMethod.Hash}";
			var adornmentsType = $"global::Rocks.Adornments<{adornmentsTypeName}, {expectationsFullyQualifiedName}.Handler{memberIdentifier}, {callbackDelegateTypeName}>";
			adornmentsFQNsPipeline(new(adornmentsTypeName, adornmentsType, string.Empty, string.Empty, property.SetMethod!, memberIdentifier));

			// We need to put the value parameter immediately after "self"
			// and then skip the value parameter by taking only the non-value parameters.
			var instanceParameters = string.Join(", ", propertySetMethod.Parameters.Take(propertySetMethod.Parameters.Length - 1).Select(parameter =>
			{
				var (expectationParameter, needs) = parameter.GetExpectationParameter(
					isGeneratedWithDefaults, propertySetMethod.Parameters[propertySetMethod.Parameters.Length - 2], typeArgumentsNamingContext);
				needsGenerationWithDefaults |= needs;
				return expectationParameter;
			}));

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
				writer.WriteLines(
					$$"""
					{{mockType.Accessibility}} {{expectationsFullyQualifiedName}}.Adornments.{{adornmentsTypeName}} {{name}}({{valueParameter}})
					{
					""");
				writer.Indent++;

				writer.WriteLines(
					$$"""
					global::Rocks.Exceptions.ExpectationException.ThrowIf(this.parent.WasInstanceInvoked);
					global::System.ArgumentNullException.ThrowIfNull(@{{lastParameter.Name}});

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

					if (this.parent.handlers{{memberIdentifier}} is null) { this.parent.handlers{{memberIdentifier}} = new(1); }
					this.parent.handlers{{memberIdentifier}}.Add(@{{handlerContext["handler"]}});
					return new(@{{handlerContext["handler"]}}, this.parent);
					""");

				writer.Indent--;
				writer.WriteLine("}");
			}

			if (needsGenerationWithDefaults)
			{
				return BuildSetterImplementation(writer, mockType, property, memberIdentifier, true, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
			}

			return null;
		}

		return BuildSetterImplementation(writer, mockType, property, memberIdentifier, false, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
	}

	internal static IndexerConstructorDefaultValues? Build(IndentedTextWriter writer, TypeMockModel mockType, PropertyModel property,
		string expectationsFullyQualifiedName, Action<AdornmentsPipeline> adornmentsFQNsPipeline)
	{
		var memberIdentifier = property.MemberIdentifier;
		var wasGetGenerated = false;

		IndexerConstructorDefaultValues? constructorValues = null;

		if ((property.Accessors == PropertyAccessor.Get || property.Accessors == PropertyAccessor.GetAndSet || property.Accessors == PropertyAccessor.GetAndInit) &&
			property.GetCanBeSeenByContainingAssembly)
		{
			constructorValues = BuildGetter(writer, mockType, property, memberIdentifier, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
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

			constructorValues = BuildSetter(writer, mockType, property, memberIdentifier, expectationsFullyQualifiedName, adornmentsFQNsPipeline);
		}

		return constructorValues;
	}
}