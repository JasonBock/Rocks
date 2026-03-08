using Rocks.Analysis.Models;
using System.Collections.Immutable;

namespace Rocks.Analysis.Builders.Create;

internal sealed class VariablesNamingContext
	: NamingContext
{
	internal VariablesNamingContext()
		: base() { }

	internal VariablesNamingContext(MethodModel method)
		: this(method.Parameters)
	{ }

	internal VariablesNamingContext(ImmutableArray<ParameterModel> parameters)
		: base([.. parameters.Select(_ => _.Name)]) { }

	internal VariablesNamingContext(ImmutableHashSet<string> names)
		: base(names) { }
}