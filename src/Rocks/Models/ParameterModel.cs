using Microsoft.CodeAnalysis;
using Rocks.Extensions;

namespace Rocks.Models;

internal record ParameterModel
{
	internal ParameterModel(IParameterSymbol parameter)
	{
		this.Name = parameter.Name;
		this.RefKind = parameter.RefKind;
		this.RequiresNullableAnnotation = parameter.RequiresForcedNullableAnnotation();
		this.IsParams = parameter.IsParams;
		this.TypeFullyQualifiedName = parameter.Type.GetFullyQualifiedName();
		this.IsPointer = parameter.Type.IsPointer();

		this.HasExplicitDefaultValue = parameter.HasExplicitDefaultValue;

		if (this.HasExplicitDefaultValue)
		{
			this.ExplicitDefaultValue = parameter.ExplicitDefaultValue.GetDefaultValue(parameter.Type);
		}
	}

	internal bool IsPointer { get; }
	internal bool HasExplicitDefaultValue { get; }
	internal string? ExplicitDefaultValue { get; }
	internal string Name { get; }

	// TODO: Should I make my own RefKind? Not sure about that...
	internal RefKind RefKind { get; }
	internal bool RequiresNullableAnnotation { get; }
	internal bool IsParams { get; }
	internal string TypeFullyQualifiedName { get; }
}