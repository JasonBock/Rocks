using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;

namespace Rocks.Completions.Tests;

internal sealed class CodeRefactoringVerifier<TCodeRefactoring>
	: CSharpCodeRefactoringVerifier<TCodeRefactoring, DefaultVerifier>
	where TCodeRefactoring : CodeRefactoringProvider, new()
{ }