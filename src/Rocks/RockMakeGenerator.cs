using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Rocks.Builders;
using Rocks.Builders.Make;
using Rocks.Configuration;
using Rocks.Extensions;
using System.Collections.Immutable;

namespace Rocks;

[Generator]
internal sealed class RockMakeGenerator
	: IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken token) =>
			// We're looking for invocations with the name "Make" -
			// that's good enough for our purposes.
			node is InvocationExpressionSyntax invocationNode &&
				invocationNode.Expression is MemberAccessExpressionSyntax memberExpression &&
				memberExpression.Name.Identifier.Text == nameof(Rock.Make);

		static (SyntaxNode, ITypeSymbol)? TransformTargets(GeneratorSyntaxContext context, CancellationToken token)
		{
			var node = (InvocationExpressionSyntax)context.Node;
			var model = context.SemanticModel;
			var nodeSymbol = model.GetSymbolInfo(node, token);
			var invocationSymbol = nodeSymbol.Symbol is IMethodSymbol symbol ? symbol :
				nodeSymbol.CandidateSymbols.Length > 0 ? (IMethodSymbol)nodeSymbol.CandidateSymbols[0] : null;

			if (invocationSymbol is not null)
			{
				// We just need to make sure the invocation node is actually
				// from our Rock type and is the Make method.
				var rockMakeSymbol = model.Compilation.GetTypeByMetadataName(typeof(Rock).FullName)!
					.GetMembers().Single(_ => _.Name == nameof(Rock.Make));

				if (SymbolEqualityComparer.Default.Equals(rockMakeSymbol, invocationSymbol.ConstructedFrom))
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
					var (diagnostics, name, text) = RockMakeGenerator.GenerateMapping(
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
		// TODO: Not that it matters...because this should go away anyway, but...
		// why is this BuildType.Create?
		var information = new MockInformation(typeToMock, containingAssemblySymbol, model,
			configurationValues, BuildType.Create);

		if (!information.Diagnostics.Any(_ => _.Severity == DiagnosticSeverity.Error))
		{
			var builder = new RockMakeBuilder(information, configurationValues, compilation);
			return (builder.Diagnostics, builder.Name, builder.Text);
		}
		else
		{
			return (information.Diagnostics, null, null);
		}
	}
}