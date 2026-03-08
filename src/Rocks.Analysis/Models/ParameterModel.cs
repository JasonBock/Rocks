using Microsoft.CodeAnalysis;
using Rocks.Analysis.Builders.Create;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Models;

internal sealed record ParameterModel
{
	internal ParameterModel(IParameterSymbol parameter, ModelContext modelContext,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation = RequiresExplicitInterfaceImplementation.No)
	{
		this.Name = parameter.Name;
		this.RefKind = parameter.RefKind;
		this.RequiresNullableAnnotation = parameter.RequiresForcedNullableAnnotation();
		this.IsParams = parameter.IsParams;
		this.IsScoped = parameter.IsScoped();

		this.Type = modelContext.CreateTypeReference(parameter.Type);

		this.HasExplicitDefaultValue = parameter.HasExplicitDefaultValue;

		if (this.HasExplicitDefaultValue)
		{
			this.ExplicitDefaultValue = parameter.ExplicitDefaultValue.GetDefaultValue(
				parameter.Type, modelContext.SemanticModel.Compilation);
		}

		this.AttributesDescription = parameter.GetAttributes().GetDescription(
			modelContext.SemanticModel.Compilation,
			requiresExplicitInterfaceImplementation: requiresExplicitInterfaceImplementation,
			isParameterWithOptionalValue: this.HasExplicitDefaultValue);
	}

	internal (string expectationParameter, bool needsGenerationWithDefaults) GetExpectationParameter(
		bool isGeneratedWithDefaults, ParameterModel lastParameter, TypeArgumentsNamingContext typeArgumentsNamingContext)
	{
		var argumentTypeName = ProjectionBuilder.BuildArgument(
			this.Type, typeArgumentsNamingContext, this.RequiresNullableAnnotation);

		if (this.Type.IsPointer)
		{
			return ($"{argumentTypeName} @{this.Name}", false);
		}
		else
		{
			var requiresNullable = this.RequiresNullableAnnotation ? "?" : string.Empty;
			var needsGenerationWithDefaults = false;

			if (isGeneratedWithDefaults)
			{
				var generatedWithDefaultsName = this.HasExplicitDefaultValue ?
					this.GetOptionalExpectationParameter(lastParameter, this.Type.FullyQualifiedName, requiresNullable) :
					this.IsParams ?
						this.Type.IsRefLikeType ?
							$"global::Rocks.RefStructArgument<{this.Type.FullyQualifiedName}> @{this.Name}" :
							$"params {this.Type.FullyQualifiedName}{requiresNullable} @{this.Name}" :
						$"{argumentTypeName} @{this.Name}";
				return (generatedWithDefaultsName, false);
			}

			if (!isGeneratedWithDefaults)
			{
				// Only set this flag if we're currently not generating with defaults.
				needsGenerationWithDefaults = this.HasExplicitDefaultValue ||
					(this.IsParams && !this.Type.IsRefLikeType);
			}

			return ($"{argumentTypeName} @{this.Name}", needsGenerationWithDefaults);
		}
	}

	private string GetOptionalExpectationParameter(ParameterModel lastParameter, string typeName, string requiresNullable) =>
		lastParameter.IsParams && lastParameter.Type.IsRefLikeType ?
			$"[global::System.Runtime.InteropServices.Optional, global::System.Runtime.InteropServices.DefaultParameterValue({this.ExplicitDefaultValue})] {typeName}{requiresNullable} @{this.Name}" :
			this.AttributesDescription.Contains("Optional") ?
				$"[global::System.Runtime.InteropServices.Optional, global::System.Runtime.InteropServices.DefaultParameterValue({this.ExplicitDefaultValue})] {typeName}{requiresNullable} @{this.Name}" :
				$"{typeName}{requiresNullable} @{this.Name} = {this.ExplicitDefaultValue}";

	internal string AttributesDescription { get; }
	internal string? ExplicitDefaultValue { get; }
	internal bool HasExplicitDefaultValue { get; }
	internal bool IsParams { get; }
	internal bool IsScoped { get; }
	internal string Name { get; }
	internal RefKind RefKind { get; }
	internal bool RequiresNullableAnnotation { get; }
	internal ITypeReferenceModel Type { get; }
}