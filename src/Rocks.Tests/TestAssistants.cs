using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using NuGet.Frameworks;

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
			ReferenceAssemblies = TestAssistants.GetNet70(), // ReferenceAssemblies.Net.Net60, // TestAssistants.GetNet60(), /* ReferenceAssemblies.Net.Net50, */
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

	// TODO: Hopefully this is temporary and the 
	// test package will include Net60 and Net70 pre-set properties.
	private static ReferenceAssemblies GetNet60()
	{
		if (!NuGetFramework.Parse("net6.0").IsPackageBased)
		{
			// The NuGet version provided at runtime does not recognize the 'net5.0' target framework
			throw new NotSupportedException("The 'net6.0' target framework is not supported by this version of NuGet.");
		}

		return new ReferenceAssemblies(
			 "net6.0",
			 new PackageIdentity(
				  "Microsoft.NETCore.App.Ref",
				  "6.0.0"),
			 Path.Combine("ref", "net6.0"));
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
				  "7.0.0-rc.1.22426.10"),
			 Path.Combine("ref", "net7.0"));
	}
}