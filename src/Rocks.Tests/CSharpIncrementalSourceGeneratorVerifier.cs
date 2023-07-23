using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Rocks.Tests
{
	// All of this code was grabbed from Refit
	// (https://github.com/reactiveui/refit/pull/1216/files)
	// based on a suggestion from
	// sharwell - https://discord.com/channels/732297728826277939/732297994699014164/910258213532876861
	// If the .NET Roslyn testing packages get updated to have something like this in the future
	// I'll remove these helpers.
	public static partial class CSharpIncrementalSourceGeneratorVerifier<TIncrementalGenerator>
		where TIncrementalGenerator : IIncrementalGenerator, new()
	{
		internal sealed class Test : CSharpSourceGeneratorTest<EmptySourceGeneratorProvider, NUnitVerifier>
		{
			public Test(ReportDiagnostic generalDiagnosticOption = ReportDiagnostic.Default) =>
				this.SolutionTransforms.Add((solution, projectId) =>
				{
					if (solution is null)
					{
						throw new ArgumentNullException(nameof(solution));
					}

					if (projectId is null)
					{
						throw new ArgumentNullException(nameof(projectId));
					}

					var compilationOptions = solution.GetProject(projectId)!.CompilationOptions!;
					compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(
						compilationOptions.SpecificDiagnosticOptions.SetItems(CSharpVerifierHelper.NullableWarnings))
						.WithGeneralDiagnosticOption(generalDiagnosticOption);
					solution = solution.WithProjectCompilationOptions(projectId, compilationOptions);

					return solution;
				});

			protected override IEnumerable<ISourceGenerator> GetSourceGenerators()
			{
				yield return new TIncrementalGenerator().AsSourceGenerator();
			}

			protected override ParseOptions CreateParseOptions()
			{
				var parseOptions = (CSharpParseOptions)base.CreateParseOptions();
				// TODO: Does this need to be preview anymore?
				return parseOptions.WithLanguageVersion(LanguageVersion.Preview);
			}
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
			internal static ImmutableDictionary<string, ReportDiagnostic> NullableWarnings { get; } = GetNullableWarningsFromCompiler();

			static ImmutableDictionary<string, ReportDiagnostic> GetNullableWarningsFromCompiler()
			{
				string[] args = { "/warnaserror:nullable" };
				var commandLineArguments = CSharpCommandLineParser.Default.Parse(
					args, baseDirectory: Environment.CurrentDirectory, sdkDirectory: Environment.CurrentDirectory);
				return commandLineArguments.CompilationOptions.SpecificDiagnosticOptions;
			}
		}
	}
}