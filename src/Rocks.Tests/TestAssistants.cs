using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace Rocks.Tests;

internal static class TestAssistants
{
	internal static async Task RunIncrementalAsync<T>(string code,
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

	internal static async Task RunAsync<T>(string code,
		IEnumerable<(Type, string, string)> generatedSources,
		IEnumerable<DiagnosticResult> expectedDiagnostics,
		OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary)
		where T : ISourceGenerator, new()
	{
		var test = new CSharpSourceGeneratorTest<T, NUnitVerifier>
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