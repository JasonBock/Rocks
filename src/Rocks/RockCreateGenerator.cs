using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocks.Builders;
using Rocks.Builders.Create;
using Rocks.Models;
using System.Collections.Immutable;

namespace Rocks;

[Generator]
internal sealed class RockCreateGenerator
	: IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken token) =>
			// We're looking for invocations with the name "Create" -
			// that's good enough for our purposes.
			// Note that we can't check the Expression on memberExpression
			// for "RockRepository" because it will be an invocation on an instance
			// and we can't figure out at this point if it's done on a RockRepository type.
			node is InvocationExpressionSyntax invocationNode &&
				invocationNode.Expression is MemberAccessExpressionSyntax memberExpression &&
				memberExpression.Name.Identifier.Text == nameof(Rock.Create);

		static MockModel? TransformTargets(GeneratorSyntaxContext context, CancellationToken token)
		{
			var node = (InvocationExpressionSyntax)context.Node;
			var model = context.SemanticModel;
			var nodeSymbol = model.GetSymbolInfo(node, token);
			var invocationSymbol = nodeSymbol.Symbol is IMethodSymbol symbol ? symbol :
				nodeSymbol.CandidateSymbols.Length > 0 ? (IMethodSymbol)nodeSymbol.CandidateSymbols[0] : null;

			if (invocationSymbol is not null)
			{
				var rockCreateSymbol = model.Compilation.GetTypeByMetadataName(typeof(Rock).FullName)!
					.GetMembers().Single(_ => _.Name == nameof(Rock.Create));
				var rockRepositoryCreateSymbol = model.Compilation.GetTypeByMetadataName(typeof(RockRepository).FullName)!
					.GetMembers().Single(_ => _.Name == nameof(RockRepository.Create));

				if (SymbolEqualityComparer.Default.Equals(rockCreateSymbol, invocationSymbol.ConstructedFrom) ||
					SymbolEqualityComparer.Default.Equals(rockRepositoryCreateSymbol, invocationSymbol.ConstructedFrom))
				{
					return MockModel.Create(node, invocationSymbol!.TypeArguments[0], model, BuildType.Create, true);
				}
			}

			return null;
		}

		var compilationOption = context.CompilationProvider.Select((compilation, _) =>
			compilation.Options.SpecificDiagnosticOptions.TryGetValue("ROCK10", out var rock10Value) ?
				rock10Value : new ReportDiagnostic?());

		var optionsProvider = context.AnalyzerConfigOptionsProvider.Select((provider, _) =>
			provider.GlobalOptions.TryGetValue("ROCK10", out var rock10Value) ?
				Enum.TryParse<DiagnosticSeverity>(rock10Value, out var rock10Severity) ?
					rock10Severity : new DiagnosticSeverity?() :
					new DiagnosticSeverity?());

		var coProvider = compilationOption.Combine(optionsProvider);

		var syntaxProvider = context.SyntaxProvider
			.CreateSyntaxProvider(IsSyntaxTargetForGeneration, TransformTargets)
			.Where(static _ => _ is not null)
			.WithTrackingName("RockCreate");

		var combinedProvider = syntaxProvider.Combine(coProvider);

		context.RegisterSourceOutput(combinedProvider.Collect(),
			(context, source) => CreateOutput(source, context));
	}

	private static void CreateOutput(ImmutableArray<(MockModel? Left, (ReportDiagnostic? Left, DiagnosticSeverity? Right) Right)> outputs, SourceProductionContext context)
	{
		var targets = new HashSet<TypeMockModel>();

		foreach (var output in outputs)
		{
			var mock = output.Left!;

			foreach (var diagnostic in mock.Diagnostics)
			{
				context.ReportDiagnostic(diagnostic);
			}

			if (mock.Type is not null && targets.Add(mock.Type))
			{
				var builder = new RockCreateBuilder(mock.Type);
				context.AddSource(builder.Name, builder.Text);
			}
		}
	}
}