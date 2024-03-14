using Rocks.Builders.Create;

namespace Rocks.Models;

internal sealed record Constraints(string TypeName, EquatableArray<string> Values)
{
	public override string ToString() =>
		$"where {this.TypeName} : {string.Join(", ", this.Values)}";

	internal string ToString(VariableNamingContext typeArgumentsNamingContext, MethodModel method)
	{
		var typeName = method.IsGenericMethod && method.TypeArguments.Contains(this.TypeName) ?
			typeArgumentsNamingContext[this.TypeName] : this.TypeName;
		return $"where {typeName} : {string.Join(", ", this.Values)}";
	}
}