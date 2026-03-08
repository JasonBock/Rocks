using Microsoft.CodeAnalysis;
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

	internal string GetOptionalParameter(ParameterModel lastParameter, string typeName)
	{
		var requiresNullable = this.RequiresNullableAnnotation ? "?" : string.Empty;

		return lastParameter.IsParams && lastParameter.Type.IsRefLikeType ?
			$"[global::System.Runtime.InteropServices.Optional, global::System.Runtime.InteropServices.DefaultParameterValue({this.ExplicitDefaultValue})] {typeName}{requiresNullable} @{this.Name}" :
			this.AttributesDescription.Contains("Optional") ?
				$"[global::System.Runtime.InteropServices.Optional, global::System.Runtime.InteropServices.DefaultParameterValue({this.ExplicitDefaultValue})] {typeName}{requiresNullable} @{this.Name}" :
				$"{typeName}{requiresNullable} @{this.Name} = {this.ExplicitDefaultValue}";
	}

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