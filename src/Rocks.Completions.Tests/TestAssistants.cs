﻿using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.Testing;
using NuGet.Frameworks;

namespace Rocks.Completions.Tests;

internal static class TestAssistants
{
	internal static async Task RunRefactoringAsync<TCodeRefactoring>(
		IEnumerable<(string, string)> sources,
		IEnumerable<(string, string)> fixedSources,
		int codeActionIndex, bool addBuildProperty,
		IEnumerable<DiagnosticResult> expectedSourceDiagnostics,
		IEnumerable<DiagnosticResult> expectedFixedDiagnostics)
		where TCodeRefactoring : CodeRefactoringProvider, new()
	{
		var test = new CodeRefactoringTest<TCodeRefactoring>
		{
			CodeActionIndex = codeActionIndex,
			ReferenceAssemblies = TestAssistants.GetNet90(),
		};

		foreach (var source in sources)
		{
			test.TestState.Sources.Add(source);
		}

		foreach (var fixedSource in fixedSources)
		{
			test.FixedState.Sources.Add(fixedSource);
		}

		if (addBuildProperty)
		{
			test.TestState.AnalyzerConfigFiles.Add(
				(@"c:\users\example\src\SomeProject",
				"""
				is_global = true

				build_property.RocksAttributeFile = MockDefinitions.cs
				"""));
		}

		test.TestState.ExpectedDiagnostics.AddRange(expectedSourceDiagnostics);
		test.FixedState.ExpectedDiagnostics.AddRange(expectedFixedDiagnostics);
		test.TestState.AdditionalReferences.Add(typeof(TCodeRefactoring).Assembly);
		test.TestState.AdditionalReferences.Add(typeof(RockAttribute).Assembly);

		await test.RunAsync();
	}

	private static ReferenceAssemblies GetNet90()
	{
		// Always look here for the latest version of a particular runtime:
		// https://www.nuget.org/packages/Microsoft.NETCore.App.Ref
		if (!NuGetFramework.Parse("net9.0").IsPackageBased)
		{
			// The NuGet version provided at runtime does not recognize the 'net9.0' target framework
			throw new NotSupportedException("The 'net9.0' target framework is not supported by this version of NuGet.");
		}

		return new ReferenceAssemblies(
			 "net9.0",
			 new PackageIdentity(
				  "Microsoft.NETCore.App.Ref",
				  "9.0.4"),
			 Path.Combine("ref", "net9.0"));
	}
}