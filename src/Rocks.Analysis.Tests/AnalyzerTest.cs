using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace Rocks.Analysis.Tests;

internal sealed class AnalyzerTest<TAnalyzer>
	: CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>
	where TAnalyzer : DiagnosticAnalyzer, new()
{
   protected override ParseOptions CreateParseOptions()
	{
		var parseOptions = (CSharpParseOptions)base.CreateParseOptions();
		return parseOptions.WithLanguageVersion(LanguageVersion.Preview);
	}
}