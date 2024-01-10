using Rocks.Models;
using System.Collections.Immutable;

namespace Rocks.Builders.Create;

internal sealed class VariableNamingContext
{
	private readonly Dictionary<string, string> variables = [];
	private readonly ImmutableHashSet<string> names;

	internal VariableNamingContext(MethodModel method)
		: this(method.Parameters) 
	{ }

	internal VariableNamingContext(ImmutableArray<ParameterModel> parameters) =>
		this.names = parameters.Select(_ => _.Name).ToImmutableHashSet();

	internal VariableNamingContext(ImmutableHashSet<string> names) =>
		this.names = names;

	internal string this[string variableName]
	{
		get
		{
			if (!this.names.Contains(variableName))
			{
				if (!this.variables.ContainsKey(variableName))
				{
					this.variables[variableName] = variableName;
				}

				return variableName;
			}
			else if (this.variables.TryGetValue(variableName, out var value))
			{
				return value;
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

// We need a VNC for Handler-based usage,
// so the factory method produces one for everyone.
internal static class HandlerVariableNamingContext
{
	internal static VariableNamingContext Create() =>
		new(new[] { "CallCount", "ExpectedCallCount", "Callback", "ReturnValue" }.ToImmutableHashSet());
}