using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Rocks.Tests;

internal sealed class IncrementalGeneratorTest<TIncrementalGenerator>
	: CSharpSourceGeneratorTest<EmptySourceGeneratorProvider, DefaultVerifier>
	where TIncrementalGenerator : IIncrementalGenerator, new()
{
	public IncrementalGeneratorTest(ReportDiagnostic generalDiagnosticOption = ReportDiagnostic.Default) =>
		this.SolutionTransforms.Add((solution, projectId) =>
		{
			ArgumentNullException.ThrowIfNull(solution);
			ArgumentNullException.ThrowIfNull(projectId);

			var compilationOptions = solution.GetProject(projectId)!.CompilationOptions!;
			compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(
				compilationOptions.SpecificDiagnosticOptions.SetItems(CSharpVerifierHelper.CompilationOptions))
				.WithGeneralDiagnosticOption(generalDiagnosticOption);
			solution = solution.WithProjectCompilationOptions(projectId, compilationOptions);

			return solution;
		});

	protected override ParseOptions CreateParseOptions()
	{
		var parseOptions = (CSharpParseOptions)base.CreateParseOptions();
		return parseOptions.WithLanguageVersion(LanguageVersion.Latest);
	}

	static class CSharpVerifierHelper
	{
		/// <summary>
		/// By default, the compiler reports diagnostics for nullable reference types at
		/// <see cref="DiagnosticSeverity.Warning"/>, and the analyzer test framework defaults to only validating
		/// diagnostics at <see cref="DiagnosticSeverity.Error"/>. This map contains all compiler diagnostic IDs
		/// related to nullability mapped to <see cref="ReportDiagnostic.Error"/>, which is then used to enable all
		/// of these warnings for default validation during analyzer and code fix tests.
		/// </summary>
		internal static ImmutableDictionary<string, ReportDiagnostic> CompilationOptions { get; } = GetCompilationOptionsFromCompiler();

		static ImmutableDictionary<string, ReportDiagnostic> GetCompilationOptionsFromCompiler()
		{
			var args = new[] { "/warnaserror:nullable" };
			var commandLineArguments = CSharpCommandLineParser.Default.Parse(
				args, baseDirectory: Environment.CurrentDirectory, sdkDirectory: Environment.CurrentDirectory);
			return commandLineArguments.CompilationOptions.SpecificDiagnosticOptions;
		}
	}
}