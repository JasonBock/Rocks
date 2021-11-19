using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Rocks.Builders;
using Rocks.Builders.Create;
using Rocks.Configuration;
using System.Collections.Immutable;

namespace Rocks;

[Generator]
internal sealed class RockCreateIncrementalGenerator
	: IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken token) =>
			// We're looking for invocations with the name "Create"
			// that were done on either "Rock" or "RockRepository" -
			// that's good enough for our purposes.
			node is InvocationExpressionSyntax invocationNode &&
				invocationNode.Expression is MemberAccessExpressionSyntax memberExpression &&
				memberExpression.Name.Identifier.Text == nameof(Rock.Create) &&
				memberExpression.Expression is IdentifierNameSyntax identifierName &&
				(identifierName.Identifier.Text == nameof(Rock) ||
					identifierName.Identifier.Text == nameof(RockRepository));

		static SyntaxNode? TransformTargets(GeneratorSyntaxContext context, CancellationToken token)
		{
			var node = (InvocationExpressionSyntax)context.Node;
			var model = context.SemanticModel;
			var nodeSymbol = model.GetSymbolInfo(node, token);
			var invocationSymbol = nodeSymbol.Symbol is IMethodSymbol symbol ? symbol :
				nodeSymbol.CandidateSymbols.Length > 0 ? (IMethodSymbol)nodeSymbol.CandidateSymbols[0] : null;

			// We just need to make sure the invocation node is actually
			// from our Rock or RockRepository type and is the Create method.
			if (invocationSymbol is not null)
			{
				var rockCreateSymbol = model.Compilation.GetTypeByMetadataName(typeof(Rock).FullName)!
					.GetMembers().Single(_ => _.Name == nameof(Rock.Create));
				var rockRepositoryCreateSymbol = model.Compilation.GetTypeByMetadataName(typeof(RockRepository).FullName)!
					.GetMembers().Single(_ => _.Name == nameof(RockRepository.Create));

				if (SymbolEqualityComparer.Default.Equals(rockCreateSymbol, invocationSymbol.ConstructedFrom) ||
					SymbolEqualityComparer.Default.Equals(rockRepositoryCreateSymbol, invocationSymbol.ConstructedFrom))
				{
					return node;
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

	private static void CreateOutput(Compilation compilation, ImmutableArray<SyntaxNode?> nodes,
		AnalyzerConfigOptionsProvider options, SourceProductionContext context)
	{
		if (nodes.Length > 0)
		{
			var targets = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);

			foreach (var node in nodes.Distinct())
			{
				var model = compilation.GetSemanticModel(node!.SyntaxTree);
				var target = RockCreateIncrementalGenerator.GetTarget(
					node!, model, context.CancellationToken);

				if (targets.Add(target))
				{
					var tree = node!.SyntaxTree;
					var configurationValues = new ConfigurationValues(options, tree);
					var (diagnostics, name, text) = RockCreateIncrementalGenerator.GenerateMapping(
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

		if (!information.Diagnostics.Any(_ => _.Severity == DiagnosticSeverity.Error))
		{
			var builder = new RockCreateBuilder(information, configurationValues, compilation);
			return (builder.Diagnostics, builder.Name, builder.Text);
		}
		else
		{
			return (information.Diagnostics, null, null);
		}
	}

	private static ITypeSymbol GetTarget(SyntaxNode node, SemanticModel model, CancellationToken token)
	{
		var nodeSymbol = model.GetSymbolInfo(node, token);
		var invocationSymbol = nodeSymbol.Symbol is IMethodSymbol symbol ? symbol :
			nodeSymbol.CandidateSymbols.Length > 0 ? (IMethodSymbol)nodeSymbol.CandidateSymbols[0] : null;
		return invocationSymbol!.TypeArguments[0];
	}
}

[Generator]
public sealed class RockCreateGenerator
 : ISourceGenerator
{
	private static (ImmutableArray<Diagnostic> diagnostics, string? name, SourceText? text) GenerateMapping(
		ITypeSymbol typeToMock, IAssemblySymbol containingAssemblySymbol, SemanticModel model,
		ConfigurationValues configurationValues, Compilation compilation)
	{
		var information = new MockInformation(typeToMock, containingAssemblySymbol, model,
			configurationValues, BuildType.Create);

		if (!information.Diagnostics.Any(_ => _.Severity == DiagnosticSeverity.Error))
		{
			var builder = new RockCreateBuilder(information, configurationValues, compilation);
			return (builder.Diagnostics, builder.Name, builder.Text);
		}
		else
		{
			return (information.Diagnostics, null, null);
		}
	}

	public void Execute(GeneratorExecutionContext context)
	{
		if (context.SyntaxContextReceiver is RockCreateReceiver receiver &&
			receiver.Targets.Count > 0)
		{
			var compilation = context.Compilation;

			foreach (var targetPair in receiver.Targets)
			{
				var model = compilation.GetSemanticModel(targetPair.Value.SyntaxTree);
				var configurationValues = new ConfigurationValues(context.AnalyzerConfigOptions, targetPair.Value.SyntaxTree);
				var (diagnostics, name, text) = RockCreateGenerator.GenerateMapping(
					targetPair.Key, compilation.Assembly, model, configurationValues, compilation);

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

	public void Initialize(GeneratorInitializationContext context) =>
		context.RegisterForSyntaxNotifications(() => new RockCreateReceiver());
}