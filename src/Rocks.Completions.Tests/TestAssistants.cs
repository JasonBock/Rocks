using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;

namespace Rocks.Completions.Tests;

internal static class TestAssistants
{
	internal static async Task RunRefactoringAsync<TCodeRefactoring>(string source, string fixedSource)
		where TCodeRefactoring : CodeRefactoringProvider, new() => 
			await CSharpCodeRefactoringVerifier<TCodeRefactoring, DefaultVerifier>
				.VerifyRefactoringAsync(source, fixedSource);
}