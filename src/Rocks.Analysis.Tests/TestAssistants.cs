using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using NuGet.Frameworks;
using Rocks.Analysis.Extensions;
using Rocks.Analysis.Tests.Extensions;
using System.Collections.Immutable;

namespace Rocks.Analysis.Tests;

internal static class TestAssistants
{
	internal static async Task RunAnalyzerAsync<TAnalyzer>(string code,
		IEnumerable<DiagnosticResult> expectedDiagnostics,
		OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary,
		IEnumerable<MetadataReference>? additionalReferences = null)
		where TAnalyzer : DiagnosticAnalyzer, new()
	{
		var test = new AnalyzerTest<TAnalyzer>()
		{
			ReferenceAssemblies = TestAssistants.GetNet10(),
			TestState =
			{
				Sources = { code },
				OutputKind = outputKind,
			},
		};

		test.TestState.AdditionalReferences.Add(typeof(TAnalyzer).Assembly);
		test.TestState.AdditionalReferences.Add(typeof(RockAttribute).Assembly);

		if (additionalReferences is not null)
		{
			test.TestState.AdditionalReferences.AddRange(additionalReferences);
		}

		test.TestState.ExpectedDiagnostics.AddRange(expectedDiagnostics);
		await test.RunAsync();
	}

	internal static async Task RunSuppressorAsync<TDiagnosticSuppressor, TDiagnosticAnalyzer>(string code,
		IEnumerable<DiagnosticResult> expectedDiagnostics,
		OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary,
		IEnumerable<MetadataReference>? additionalReferences = null)
		where TDiagnosticSuppressor : DiagnosticSuppressor, new()
		where TDiagnosticAnalyzer : DiagnosticAnalyzer, new()
	{
		var test = new DiagnosticSuppressorVerifier<TDiagnosticSuppressor, TDiagnosticAnalyzer>()
		{
			ReferenceAssemblies = TestAssistants.GetNet10(),
			TestState =
			{
				Sources = { code },
				OutputKind = outputKind
			},
		};

		test.ExpectedDiagnostics.AddRange(expectedDiagnostics);

		test.TestState.AdditionalReferences.Add(typeof(TDiagnosticSuppressor).Assembly);
		test.TestState.AdditionalReferences.Add(typeof(RockAttribute).Assembly);

		var analyzer = new TDiagnosticAnalyzer();
		var diagnosticOptions = analyzer.SupportedDiagnostics
			.ToImmutableDictionary(
				static descriptor => descriptor.Id,
				static descriptor => descriptor.DefaultSeverity.ToReportDiagnostic());

		test.SolutionTransforms.Add((solution, projectId) =>
		{
			if (additionalReferences is not null)
			{
				solution = solution.AddMetadataReferences(projectId, additionalReferences);
			}

			var project = solution.GetProject(projectId)!;

			var compilationOptions = (CSharpCompilationOptions)project.CompilationOptions!;

			compilationOptions = compilationOptions
				.WithGeneralDiagnosticOption(ReportDiagnostic.Warn)
				.WithSpecificDiagnosticOptions(diagnosticOptions)
				.WithNullableContextOptions(NullableContextOptions.Enable);

			return solution.WithProjectCompilationOptions(projectId, compilationOptions);
		});

		test.DiagnosticVerifier = (diagnostic, result, verifier) =>
		{
			var expected = result.IsSuppressed.GetValueOrDefault();

			verifier.Equal(expected, diagnostic.IsSuppressed, 
				$"{nameof(Diagnostic)} {result} is expected to{(expected ? " be" : " not be")} suppressed.");
		};

		await test.RunAsync();
	}

	internal static async Task RunGeneratorAsync<TGenerator>(string code,
		IEnumerable<(string, string)> generatedSources,
		IEnumerable<DiagnosticResult> expectedDiagnostics,
		OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary,
		IEnumerable<MetadataReference>? additionalReferences = null,
		ReportDiagnostic generalDiagnosticOption = ReportDiagnostic.Default,
		List<string>? disabledDiagnostics = null)
		where TGenerator : IIncrementalGenerator, new()
	{
		var test = new IncrementalGeneratorTest<TGenerator>(generalDiagnosticOption)
		{
			ReferenceAssemblies = TestAssistants.GetNet10(),
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

		foreach (var (outputFileName, outputCode) in IncrementalGeneratorInitializationContextExtensions.GetOutputCode())
		{
			test.TestState.GeneratedSources.Add((typeof(RockGenerator), outputFileName, outputCode));
		}

		foreach (var (generatedFileName, generatedCode) in generatedSources)
		{
			test.TestState.GeneratedSources.Add((typeof(RockGenerator), generatedFileName, generatedCode));
		}

		test.TestState.AdditionalReferences.Add(typeof(TGenerator).Assembly);
		test.TestState.AdditionalReferences.Add(typeof(RockAttribute).Assembly);

		if (additionalReferences is not null)
		{
			test.TestState.AdditionalReferences.AddRange(additionalReferences);
		}

		test.TestState.ExpectedDiagnostics.AddRange(expectedDiagnostics);
		await test.RunAsync();
	}

	private static ReferenceAssemblies GetNet10()
	{
		// Always look here for the latest version of a particular runtime:
		// https://www.nuget.org/packages/Microsoft.NETCore.App.Ref
		if (!NuGetFramework.Parse("net10.0").IsPackageBased)
		{
			// The NuGet version provided at runtime does not recognize the 'net10.0' target framework
			throw new NotSupportedException("The 'net10.0' target framework is not supported by this version of NuGet.");
		}

		return new ReferenceAssemblies(
			 "net10.0",
			 new PackageIdentity(
				  "Microsoft.NETCore.App.Ref",
				  "10.0.0-rc.1.25451.107"),
			 Path.Combine("ref", "net10.0"));
	}
}