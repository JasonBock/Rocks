using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal sealed class VariableNamingContext
{
	private readonly ImmutableArray<IParameterSymbol> parameters;
	private readonly Dictionary<string, string> variables = new();

	internal VariableNamingContext(IMethodSymbol method) =>
		this.parameters = method.Parameters;

	internal VariableNamingContext(ImmutableArray<IParameterSymbol> parameters) =>
		this.parameters = parameters;

	internal string this[string variableName]
	{
		get
		{
			if (!this.variables.ContainsKey(variableName))
			{
				var uniqueName = variableName;
				var id = 1;

				while (this.parameters.Any(_ => _.Name == uniqueName) ||
					this.variables.ContainsKey(uniqueName))
				{
					uniqueName = $"{variableName}{id++}";
				}

				this.variables.Add(variableName, uniqueName);
			}

			return this.variables[variableName];
		}
	}
}