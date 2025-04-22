using Rocks.Analysis.Builders.Create;

namespace Rocks.Analysis.Models;

internal sealed record Constraints(string TypeName, EquatableArray<string> Values)
{
	public override string ToString() =>
		$"where {this.TypeName} : {string.Join(", ", this.Values)}";

	internal string ToString(TypeArgumentsNamingContext typeArgumentsNamingContext, MethodModel method)
	{
		var typeName = method.IsGenericMethod && method.TypeArguments.Any(m => m.FullyQualifiedName == this.TypeName) ?
			typeArgumentsNamingContext[this.TypeName] : this.TypeName;
		return $"where {typeName} : {string.Join(", ", this.Values)}";
	}
}