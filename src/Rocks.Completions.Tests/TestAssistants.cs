using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.Testing;
using NuGet.Frameworks;
using Rocks.Runtime;

namespace Rocks.Completions.Tests;

internal static class TestAssistants
{
	internal static async Task RunRefactoringAsync<TCodeRefactoring>(
		string source, string fixedSource, int codeActionIndex)
		where TCodeRefactoring : CodeRefactoringProvider, new()
	{
		var test = new CodeRefactoringTest<TCodeRefactoring>
		{
			CodeActionIndex = codeActionIndex,
			ReferenceAssemblies = TestAssistants.GetNet90(),
			TestState =
			{
				Sources = { source }
			},
			FixedState =
			{
				Sources = { fixedSource }
			}
		};

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