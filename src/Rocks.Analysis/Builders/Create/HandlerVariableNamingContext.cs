namespace Rocks.Analysis.Builders.Create;

// We need a VNC for Handler-based usage,
// so the factory method produces one for everyone.
internal static class HandlerVariableNamingContext
{
	internal static readonly string[] sourceArray = ["CallCount", "ExpectedCallCount", "Callback", "ReturnValue", "Exception"];

	internal static VariablesNamingContext Create() =>
		new([.. sourceArray]);
}