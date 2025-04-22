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

// We need a VNC for Handler-based usage,
// so the factory method produces one for everyone.
internal static class HandlerVariableNamingContext
{
   internal static readonly string[] sourceArray = ["CallCount", "ExpectedCallCount", "Callback", "ReturnValue"];

   internal static VariablesNamingContext Create() =>
		new([.. sourceArray]);
}