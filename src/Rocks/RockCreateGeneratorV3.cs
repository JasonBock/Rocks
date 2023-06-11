using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocks.Builders;
using Rocks.Builders.Create;
using Rocks.Models;
using System.Collections.Immutable;

namespace Rocks;

[Generator]
internal sealed class RockCreateGeneratorV3
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
					return MockModel.Create(invocationSymbol!.TypeArguments[0], model, BuildType.Create, true);
				}
			}

			return null;
		}

		var provider = context.SyntaxProvider
			.CreateSyntaxProvider(IsSyntaxTargetForGeneration, TransformTargets)
			.Where(static _ => _ is not null);
		context.RegisterSourceOutput(provider.Collect(),
			(context, source) => CreateOutput(source, context));
	}

	private static void CreateOutput(ImmutableArray<MockModel?> mocks, SourceProductionContext context)
	{
		var targets = new HashSet<TypeMockModel>();

		foreach (var mock in mocks)
		{
			foreach (var diagnostic in mock!.Diagnostics)
			{
				context.ReportDiagnostic(diagnostic);
			}

			if (mock.Type is not null && targets.Add(mock.Type))
			{
				var builder = new RockCreateBuilderV3(mock.Type);
				context.AddSource(builder.Name, builder.Text);
			}
		}
	}
}