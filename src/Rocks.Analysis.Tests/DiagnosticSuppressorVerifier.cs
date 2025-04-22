using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace Rocks.Analysis.Tests;

internal sealed class DiagnosticSuppressorVerifier<TDiagnosticSuppressor, TDiagnosticAnalyzer>
	: CSharpAnalyzerTest<TDiagnosticSuppressor, DefaultVerifier>
	where TDiagnosticSuppressor : DiagnosticSuppressor, new()
	where TDiagnosticAnalyzer : DiagnosticAnalyzer, new()
{
   protected override IEnumerable<DiagnosticAnalyzer> GetDiagnosticAnalyzers() => 
		base.GetDiagnosticAnalyzers().Concat([new TDiagnosticAnalyzer()]);
}