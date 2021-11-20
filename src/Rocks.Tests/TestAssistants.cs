using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;

namespace Rocks.Tests;

internal static class TestAssistants
{
	internal static async Task RunAsync<T>(string code,
		IEnumerable<(Type, string, string)> generatedSources,
		IEnumerable<DiagnosticResult> expectedDiagnostics,
		OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary)
		where T : IIncrementalGenerator, new()
	{
		var test = new CSharpIncrementalSourceGeneratorVerifier<T>.Test
		{
			ReferenceAssemblies = ReferenceAssemblies.Net.Net50,
			TestState =
			{
				Sources = { code },
				OutputKind = outputKind
			},
		};

		foreach (var generatedSource in generatedSources)
		{
			test.TestState.GeneratedSources.Add(generatedSource);
		}

		test.TestState.AdditionalReferences.Add(typeof(T).Assembly);
		test.TestState.ExpectedDiagnostics.AddRange(expectedDiagnostics);
		await test.RunAsync().ConfigureAwait(false);
	}
}