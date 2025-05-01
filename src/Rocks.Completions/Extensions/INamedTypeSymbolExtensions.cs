using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rocks.Completions.Extensions;

internal static class INamedTypeSymbolExtensions
{
	internal static NameSyntax GetFullName(this INamedTypeSymbol self)
	{
		static SimpleNameSyntax GetName(INamedTypeSymbol symbol)
		{
			if (symbol.TypeParameters.Length == 0)
			{
				return SyntaxFactory.IdentifierName(symbol.Name);
			}

			var nodesOrTokens = new List<SyntaxNodeOrToken>();

			for (var i = 0; i < symbol.TypeParameters.Length; i++)
			{
				if (i == symbol.TypeParameters.Length - 1)
				{
					nodesOrTokens.Add(SyntaxFactory.OmittedTypeArgument());
				}
				else
				{
					nodesOrTokens.Add(SyntaxFactory.OmittedTypeArgument());
					nodesOrTokens.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
				}
			}

			return SyntaxFactory.GenericName(
				SyntaxFactory.Identifier(symbol.Name))
				.WithTypeArgumentList(
					SyntaxFactory.TypeArgumentList(
						SyntaxFactory.SeparatedList<TypeSyntax>(nodesOrTokens)));
		}

		// See how many parts there are to the namespace
		if (self.ContainingNamespace.IsGlobalNamespace)
		{
			return GetName(self);
		}
		else
		{
			if (self.ContainingNamespace.ToDisplayParts().Length == 1)
			{
				return SyntaxFactory.QualifiedName(
					SyntaxFactory.IdentifierName(self.ContainingNamespace.Name), GetName(self));
			}

			var namespaceParts = self.ContainingNamespace.ToDisplayString().Split('.');

			QualifiedNameSyntax? qualifiedName = null;

			for (var i = 0; i < namespaceParts.Length - 1; i++)
			{
				if (i == 0)
				{
					qualifiedName = SyntaxFactory.QualifiedName(
						SyntaxFactory.IdentifierName(namespaceParts[i]),
						SyntaxFactory.IdentifierName(namespaceParts[i + 1]));
				}
				else
				{
					qualifiedName = SyntaxFactory.QualifiedName(
						qualifiedName!, SyntaxFactory.IdentifierName(namespaceParts[i + 1]));
				}
			}

			return SyntaxFactory.QualifiedName(
				qualifiedName!, GetName(self));
		}
	}
}
