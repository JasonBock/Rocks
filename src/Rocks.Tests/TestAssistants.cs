using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using NuGet.Frameworks;

namespace Rocks.Tests;

internal static class TestAssistants
{
	internal static async Task RunAnalyzerAsync<TAnalyzer>(string code,
		IEnumerable<DiagnosticResult> expectedDiagnostics,
		OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary)
		where TAnalyzer: DiagnosticAnalyzer, new()
	{
		var test = new AnalyzerTest<TAnalyzer>()
		{
			ReferenceAssemblies = TestAssistants.GetNet80(),
			TestState =
			{
				Sources = { code },
				OutputKind = outputKind,
			},
		};

		test.TestState.AdditionalReferences.Add(typeof(TAnalyzer).Assembly);
		test.TestState.ExpectedDiagnostics.AddRange(expectedDiagnostics);
		await test.RunAsync();
	}

	internal static async Task RunGeneratorAsync<TGenerator>(string code,
		IEnumerable<(Type, string, string)> generatedSources,
		IEnumerable<DiagnosticResult> expectedDiagnostics,
		OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary,
		IEnumerable<MetadataReference>? additionalReferences = null,
		ReportDiagnostic generalDiagnosticOption = ReportDiagnostic.Default,
		List<string>? disabledDiagnostics = null)
		where TGenerator : IIncrementalGenerator, new()
	{
		var test = new IncrementalGeneratorTest<TGenerator>(generalDiagnosticOption)
		{
			ReferenceAssemblies = TestAssistants.GetNet80(),
			TestState =
			{
				Sources = { code },
				OutputKind = outputKind,
			},
		};

		if (disabledDiagnostics is not null)
		{
			test.DisabledDiagnostics.AddRange(disabledDiagnostics);
		}

		foreach (var generatedSource in generatedSources)
		{
			test.TestState.GeneratedSources.Add(generatedSource);
		}

		test.TestState.AdditionalReferences.Add(typeof(TGenerator).Assembly);

		if(additionalReferences is not null)
		{
			test.TestState.AdditionalReferences.AddRange(additionalReferences);
		}

		test.TestState.ExpectedDiagnostics.AddRange(expectedDiagnostics);
		await test.RunAsync();
	}

	private static ReferenceAssemblies GetNet80()
	{
		// ReferenceAssemblies.Net.Net60, // TestAssistants.GetNet60(), /* ReferenceAssemblies.Net.Net50, */
		if (!NuGetFramework.Parse("net8.0").IsPackageBased)
		{
			// The NuGet version provided at runtime does not recognize the 'net7.0' target framework
			throw new NotSupportedException("The 'net7.0' target framework is not supported by this version of NuGet.");
		}

		return new ReferenceAssemblies(
			 "net8.0",
			 new PackageIdentity(
				  "Microsoft.NETCore.App.Ref",
				  "8.0.0"),
			 Path.Combine("ref", "net8.0"));
	}
}