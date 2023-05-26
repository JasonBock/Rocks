using Microsoft.CodeAnalysis;

namespace Rocks.Models;

internal record ParameterModel
{
	internal ParameterModel(IParameterSymbol parameter)
	{
		this.Name = parameter.Name;
	}

   internal string Name { get; }
}