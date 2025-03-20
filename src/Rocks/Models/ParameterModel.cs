using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks.Models;

internal sealed record ParameterModel
{
	internal ParameterModel(IParameterSymbol parameter, Compilation compilation,
		RequiresExplicitInterfaceImplementation requiresExplicitInterfaceImplementation = RequiresExplicitInterfaceImplementation.No)
	{
		this.Name = parameter.Name;
		this.RefKind = parameter.RefKind;
		this.RequiresNullableAnnotation = parameter.RequiresForcedNullableAnnotation();
		this.IsParams = parameter.IsParams;
		this.IsScoped = parameter.IsScoped();

		this.Type = new TypeReferenceModel(parameter.Type, compilation);

		this.HasExplicitDefaultValue = parameter.HasExplicitDefaultValue;

		if (this.HasExplicitDefaultValue)
		{
			this.ExplicitDefaultValue = parameter.ExplicitDefaultValue.GetDefaultValue(parameter.Type, compilation);
		}

		this.AttributesDescription = parameter.GetAttributes().GetDescription(
			compilation,
			requiresExplicitInterfaceImplementation: requiresExplicitInterfaceImplementation,
			isParameterWithOptionalValue: this.HasExplicitDefaultValue);
	}

	internal string AttributesDescription { get; }
	internal string? ExplicitDefaultValue { get; }
	internal bool HasExplicitDefaultValue { get; }
	internal bool IsParams { get; }
	internal bool IsScoped { get; }
	internal string Name { get; }
	internal RefKind RefKind { get; }
	internal bool RequiresNullableAnnotation { get; }
	internal TypeReferenceModel Type { get; }
}