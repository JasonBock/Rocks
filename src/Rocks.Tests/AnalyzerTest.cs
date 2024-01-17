using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace Rocks.Tests;

internal sealed class AnalyzerTest<TAnalyzer>
	: CSharpAnalyzerTest<TAnalyzer, NUnitVerifier>
	where TAnalyzer : DiagnosticAnalyzer, new()
{
   protected override ParseOptions CreateParseOptions()
	{
		var parseOptions = (CSharpParseOptions)base.CreateParseOptions();
		return parseOptions.WithLanguageVersion(LanguageVersion.Latest);
	}
}