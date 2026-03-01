using System.Collections.Immutable;

namespace Rocks.Analysis.Builders.Create;

/*
The purpose of this type is to provide a way for
the code generation phase to say, "I want a variable named "value"",
and produce a name that will not collide with other named "things"
currently in scope.

For example, if you wanted a variable named "value"
and the method parameters are called "a", "b", and "c",
then "value" doesn't collide with any of them,
and the indexer getter will return "value".
However, if one of the parameters is named "value",
then the getter will return "value2".
*/
internal abstract class NamingContext
{
	private readonly Dictionary<string, string> variables;
	private readonly ImmutableHashSet<string> names;

	protected NamingContext() =>
		(this.names, this.variables) = ([], []);

	protected NamingContext(ImmutableHashSet<string> names)
		: base() => (this.names, this.variables) = (names, []);

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

	internal int NameCount => this.names.Count;
}