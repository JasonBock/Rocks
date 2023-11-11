using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using NuGet.Frameworks;

namespace Rocks.Tests;

internal static class TestAssistants
{
	internal static async Task RunAsync<T>(string code,
		IEnumerable<(Type, string, string)> generatedSources,
		IEnumerable<DiagnosticResult> expectedDiagnostics,
		OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary,
		IEnumerable<MetadataReference>? additionalReferences = null,
		ReportDiagnostic generalDiagnosticOption = ReportDiagnostic.Default,
		List<string>? disabledDiagnostics = null)
		where T : IIncrementalGenerator, new()
	{
		var test = new CSharpIncrementalSourceGeneratorVerifier<T>.Test(generalDiagnosticOption)
		{
			ReferenceAssemblies = TestAssistants.GetNet80(), // ReferenceAssemblies.Net.Net60, // TestAssistants.GetNet60(), /* ReferenceAssemblies.Net.Net50, */
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

		test.TestState.AdditionalReferences.Add(typeof(T).Assembly);

		if(additionalReferences is not null)
		{
			test.TestState.AdditionalReferences.AddRange(additionalReferences);
		}

		test.TestState.ExpectedDiagnostics.AddRange(expectedDiagnostics);
		await test.RunAsync().ConfigureAwait(false);
	}

	private static ReferenceAssemblies GetNet70()
	{
		if (!NuGetFramework.Parse("net7.0").IsPackageBased)
		{
			// The NuGet version provided at runtime does not recognize the 'net7.0' target framework
			throw new NotSupportedException("The 'net7.0' target framework is not supported by this version of NuGet.");
		}

		return new ReferenceAssemblies(
			 "net7.0",
			 new PackageIdentity(
				  "Microsoft.NETCore.App.Ref",
				  "7.0.13"),
			 Path.Combine("ref", "net7.0"));
   }

	private static ReferenceAssemblies GetNet80()
	{
		if (!NuGetFramework.Parse("net8.0").IsPackageBased)
		{
			// The NuGet version provided at runtime does not recognize the 'net7.0' target framework
			throw new NotSupportedException("The 'net7.0' target framework is not supported by this version of NuGet.");
		}

		return new ReferenceAssemblies(
			 "net8.0",
			 new PackageIdentity(
				  "Microsoft.NETCore.App.Ref",
				  "8.0.0-rc.2.23479.6"),
			 Path.Combine("ref", "net8.0"));
	}
}