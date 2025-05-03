using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rocks.Completions.Extensions;

internal static class SemanticModelExtensions
{
	internal static bool HasRockAttributeDefinition(this SemanticModel self, INamedTypeSymbol mockTypeSymbol)
	{
		var assemblyAttributeListSyntaxNodes = self.SyntaxTree.GetRoot()
			.DescendantNodes(_ => true)
			.OfType<AttributeListSyntax>()
			.Where(_ => _.DescendantTokens(_ => true)
				.Any(_ => _.RawKind == (int)SyntaxKind.AssemblyKeyword))
			.ToArray();

		if (assemblyAttributeListSyntaxNodes.Length > 0)
		{
			foreach (var assemblyAttributeList in assemblyAttributeListSyntaxNodes)
			{
				foreach (var attributeSyntax in assemblyAttributeList.Attributes)
				{
					var attributeCtor = self.GetSymbolInfo(attributeSyntax).Symbol as IMethodSymbol;

					if (attributeCtor is not null &&
						attributeCtor.ContainingType.Name == "RockAttribute" &&
						attributeCtor.ContainingType.ContainingNamespace.ToDisplayString() == "Rocks.Runtime" &&
						attributeCtor.ContainingType.ContainingAssembly.ToDisplayString().StartsWith("Rocks.Runtime") &&
						attributeCtor.Parameters.Length == 2)
					{
						// self.GetSymbolInfo((attributeSyntax.ArgumentList.Arguments[0].Expression as TypeOfExpressionSyntax).Type).Symbol is INamedTypeSymbol

						if (attributeSyntax.ArgumentList?.Arguments[0].Expression is TypeOfExpressionSyntax attributeTypeOf)
						{
							if (self.GetSymbolInfo(attributeTypeOf.Type).Symbol is INamedTypeSymbol attributeTypeOfSymbol &&
								SymbolEqualityComparer.Default.Equals(attributeTypeOfSymbol, mockTypeSymbol))
							{
								return true;
							}
						}
					}
				}
			}
		}

		return false;
	}
}
