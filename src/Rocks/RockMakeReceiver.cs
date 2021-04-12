﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocks.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Rocks
{
	public sealed class RockMakeReceiver
		: ISyntaxContextReceiver
	{
		public Dictionary<ITypeSymbol, SyntaxNode> Targets { get; } = new(SymbolEqualityComparer.Default);

		public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
		{
			var node = context.Node;
			var model = context.SemanticModel;

			if (node is InvocationExpressionSyntax)
			{
				var rockMakeSymbol = model.Compilation.GetTypeByMetadataName(typeof(Rock).FullName)!
					.GetMembers().Single(_ => _.Name == nameof(Rock.Make));
				var nodeSymbol = model.GetSymbolInfo(node);

				var invocationSymbol = nodeSymbol.Symbol is IMethodSymbol symbol ? symbol :
					nodeSymbol.CandidateSymbols.Length > 0 ? (IMethodSymbol)nodeSymbol.CandidateSymbols[0] : null;

				if (invocationSymbol is not null)
				{
					if (SymbolEqualityComparer.Default.Equals(rockMakeSymbol, invocationSymbol.ConstructedFrom))
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
}