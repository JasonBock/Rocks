using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Rocks.Builders;
using Rocks.Builders.Create;
using Rocks.Configuration;
using Rocks.Extensions;
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

		static (SyntaxNode, ITypeSymbol)? TransformTargets(GeneratorSyntaxContext context, CancellationToken token)
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
					// Return the generic type argument along with the node, as that's the target we need to find.
					return (node, invocationSymbol!.TypeArguments[0]);
				}
			}

			return null;
		}

		var provider = context.SyntaxProvider
			.CreateSyntaxProvider(IsSyntaxTargetForGeneration, TransformTargets)
			.Where(static _ => _ is not null);
		var compilationNodes = context.CompilationProvider.Combine(provider.Collect());
		var output = context.AnalyzerConfigOptionsProvider.Combine(compilationNodes);
		context.RegisterSourceOutput(output,
			(context, source) => CreateOutput(source.Right.Left, source.Right.Right, source.Left, context));
	}

	private static void CreateOutput(Compilation compilation, ImmutableArray<(SyntaxNode, ITypeSymbol)?> symbols,
		AnalyzerConfigOptionsProvider options, SourceProductionContext context)
	{
		if (symbols.Length > 0)
		{
			var targets = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);

			foreach (var (node, target) in symbols.Distinct().Cast<(SyntaxNode, ITypeSymbol)>())
			{
				if (!target.ContainsDiagnostics() && targets.Add(target))
				{
					var tree = node.SyntaxTree;
					var model = compilation.GetSemanticModel(tree);
					var configurationValues = new ConfigurationValues(options, tree);
					var (diagnostics, name, text) = RockCreateGenerator.GenerateMapping(
						target, compilation.Assembly, model, configurationValues, compilation);

					foreach (var diagnostic in diagnostics)
					{
						context.ReportDiagnostic(diagnostic);
					}

					if (name is not null && text is not null)
					{
						context.AddSource(name, text);
					}
				}
			}
		}
	}

	private static (ImmutableArray<Diagnostic> diagnostics, string? name, SourceText? text) GenerateMapping(
		ITypeSymbol typeToMock, IAssemblySymbol containingAssemblySymbol, SemanticModel model,
		ConfigurationValues configurationValues, Compilation compilation)
	{
		var information = new MockInformation(typeToMock, containingAssemblySymbol, model,
			configurationValues, BuildType.Create);

		if (information.TypeToMock is not null)
		{
			var builder = new RockCreateBuilder(information, configurationValues, compilation);
			return (builder.Diagnostics, builder.Name, builder.Text);
		}
		else
		{
			return (information.Diagnostics, null, null);
		}
	}
}