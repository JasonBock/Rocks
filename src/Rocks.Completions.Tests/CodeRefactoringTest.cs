using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;

namespace Rocks.Completions.Tests;

internal sealed class CodeRefactoringTest<TCodeRefactoring>
	: CSharpCodeRefactoringTest<TCodeRefactoring, DefaultVerifier>
	where TCodeRefactoring : CodeRefactoringProvider, new()
{ }