using Rocks.Models;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal sealed class VariableNamingContextV4
{
	private readonly Dictionary<string, string> variables = [];
	private readonly ImmutableArray<string> names;

	internal VariableNamingContextV4(MethodModel method) =>
		this.names = method.Parameters.Select(_ => _.Name).ToImmutableArray();

	internal VariableNamingContextV4(ImmutableArray<ParameterModel> parameters) =>
		this.names = parameters.Select(_ => _.Name).ToImmutableArray();

	internal VariableNamingContextV4(ImmutableArray<string> names) =>
		this.names = names;

	internal string this[string variableName]
	{
		get
		{
			if (!this.names.Contains(variableName))
			{
				if(!this.variables.ContainsKey(variableName))
				{
					this.variables[variableName] = variableName;
				}

				return variableName;
			}
			else
			{
				var uniqueName = variableName;
				var id = 1;

				while (this.names.Contains(uniqueName) ||
					this.variables.ContainsKey(uniqueName))
				{
					uniqueName = $"{variableName}{id++}";
				}

				this.variables.Add(variableName, uniqueName);
				return uniqueName;
			}
		}
	}
}