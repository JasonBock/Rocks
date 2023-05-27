using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks.Models;

internal record ParameterModel
{
	internal ParameterModel(IParameterSymbol parameter, Compilation compilation)
	{
		this.Name = parameter.Name;
		this.RefKind = parameter.RefKind;
		this.RequiresNullableAnnotation = parameter.RequiresForcedNullableAnnotation();
		this.IsParams = parameter.IsParams;
		this.Type = new TypeReferenceModel(parameter.Type, compilation);

		this.HasExplicitDefaultValue = parameter.HasExplicitDefaultValue;

		if (this.HasExplicitDefaultValue)
		{
			this.ExplicitDefaultValue = parameter.ExplicitDefaultValue.GetDefaultValue(parameter.Type);
		}

		this.AttributesDescription = parameter.GetAttributes().GetDescription(compilation);
	}

   internal string AttributesDescription { get; }
	internal bool HasExplicitDefaultValue { get; }
	internal string? ExplicitDefaultValue { get; }
	internal string Name { get; }

	// TODO: Should I make my own RefKind? Not sure about that...
	internal RefKind RefKind { get; }
	internal bool RequiresNullableAnnotation { get; }
	internal bool IsParams { get; }
   internal TypeReferenceModel Type { get; }
}