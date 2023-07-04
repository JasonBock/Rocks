using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks.Models;

internal sealed record ParameterModel
{
	internal ParameterModel(IParameterSymbol parameter, TypeReferenceModel mockType, Compilation compilation, 
		bool isBasedOnTypeParameter = false)
	{
		this.MockType = mockType;
		this.Name = parameter.Name;
		this.RefKind = parameter.RefKind;
		this.RequiresNullableAnnotation = parameter.RequiresForcedNullableAnnotation();
		this.IsParams = parameter.IsParams;

		this.Type = new TypeReferenceModel(parameter.Type, compilation, isBasedOnTypeParameter);

		this.HasExplicitDefaultValue = parameter.HasExplicitDefaultValue;

		if (this.HasExplicitDefaultValue)
		{
			this.ExplicitDefaultValue = parameter.ExplicitDefaultValue.GetDefaultValue(parameter.Type);
		}

		this.AttributesDescription = parameter.GetAttributes().GetDescription(compilation);
	}

   internal string AttributesDescription { get; }
	internal string? ExplicitDefaultValue { get; }
	internal bool HasExplicitDefaultValue { get; }
	internal bool IsParams { get; }
	internal TypeReferenceModel MockType { get; }
   internal string Name { get; }
	internal RefKind RefKind { get; }
	internal bool RequiresNullableAnnotation { get; }
   internal TypeReferenceModel Type { get; }
}