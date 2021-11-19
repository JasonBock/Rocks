using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocks.Extensions;

namespace Rocks;

public sealed class RockCreateReceiver
	 : ISyntaxContextReceiver
{
	public Dictionary<ITypeSymbol, SyntaxNode> Targets { get; } = new(SymbolEqualityComparer.Default);

	public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
	{
		var node = context.Node;
		var model = context.SemanticModel;

		if (node is InvocationExpressionSyntax)
		{
			var rockCreateSymbol = model.Compilation.GetTypeByMetadataName(typeof(Rock).FullName)!
				.GetMembers().Single(_ => _.Name == nameof(Rock.Create));
			var rockRepositoryCreateSymbol = model.Compilation.GetTypeByMetadataName(typeof(RockRepository).FullName)!
				.GetMembers().Single(_ => _.Name == nameof(RockRepository.Create));
			var nodeSymbol = model.GetSymbolInfo(node);

			var invocationSymbol = nodeSymbol.Symbol is IMethodSymbol symbol ? symbol :
				nodeSymbol.CandidateSymbols.Length > 0 ? (IMethodSymbol)nodeSymbol.CandidateSymbols[0] : null;

			if (invocationSymbol is not null)
			{
				if (SymbolEqualityComparer.Default.Equals(rockCreateSymbol, invocationSymbol.ConstructedFrom) ||
					SymbolEqualityComparer.Default.Equals(rockRepositoryCreateSymbol, invocationSymbol.ConstructedFrom))
				{
					var typeToMock = invocationSymbol.TypeArguments[0];

					if (!typeToMock.ContainsDiagnostics() && !this.Targets.ContainsKey(typeToMock))
					{
						this.Targets.Add(typeToMock, node);
					}
				}
			}
		}
	}
}