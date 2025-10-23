using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rocks.Completions.Extensions;

internal static class SyntaxNodeExtensions
{
	internal static INamedTypeSymbol? FindParentSymbol(this SyntaxNode self,
		SemanticModel model, CancellationToken cancellationToken)
	{
		Type[] syntaxNodeTypes =
			[
				typeof(IdentifierNameSyntax),
				typeof(ParameterSyntax),
				typeof(ObjectCreationExpressionSyntax),
				typeof(BaseTypeSyntax),
				typeof(PredefinedTypeSyntax),
				typeof(AttributeSyntax),
				typeof(ClassDeclarationSyntax),
				typeof(InterfaceDeclarationSyntax),
				typeof(RecordDeclarationSyntax)
			];

		var parent = self;

		while (parent is not null)
		{
			var syntaxNodeTypeMatch = syntaxNodeTypes.FirstOrDefault(
				t => t.IsAssignableFrom(parent.GetType()));

			if (syntaxNodeTypeMatch is not null)
			{
				ISymbol? parentSymbol = null;

				if (parent is IdentifierNameSyntax identifierNameSyntax)
				{
					var identifierNameSymbol = model.GetSymbolInfo(identifierNameSyntax, cancellationToken);

					if (identifierNameSymbol.Symbol is not null)
					{
						parentSymbol = identifierNameSymbol.Symbol;
					}
					else if (identifierNameSymbol.CandidateSymbols.Length > 0)
					{
						parentSymbol = identifierNameSymbol.CandidateSymbols[0];
					}
				}
				else if (parent is ParameterSyntax parameterSyntax)
				{
					var parameterSymbol = model.GetDeclaredSymbol(parameterSyntax, cancellationToken) as IParameterSymbol;

					if (parameterSymbol is not null)
					{
						parentSymbol = parameterSymbol.Type;
					}
				}
				else if (parent is ObjectCreationExpressionSyntax objectCreationExpressionSyntax)
				{
					var objectCreationSymbol = model.GetSymbolInfo(objectCreationExpressionSyntax, cancellationToken).Symbol as IMethodSymbol;

					if (objectCreationSymbol is not null)
					{
						parentSymbol = objectCreationSymbol.ContainingType;
					}
				}
				else if (parent is BaseTypeSyntax baseTypeSyntax)
				{
					parentSymbol = model.GetSymbolInfo(baseTypeSyntax.Type, cancellationToken).Symbol;
				}
				else if (parent is PredefinedTypeSyntax predefinedTypeSyntax)
				{
					parentSymbol = model.GetSymbolInfo(predefinedTypeSyntax, cancellationToken).Symbol;
				}
				else if (parent is AttributeSyntax attributeSyntax)
				{
					parentSymbol = model.GetSymbolInfo(attributeSyntax, cancellationToken).Symbol;
				}
				else
				{
					parentSymbol = parent switch
					{
						ClassDeclarationSyntax classDeclarationSyntax =>
							model.GetDeclaredSymbol(classDeclarationSyntax, cancellationToken),
						InterfaceDeclarationSyntax interfaceDeclarationSyntax =>
							model.GetDeclaredSymbol(interfaceDeclarationSyntax, cancellationToken),
						RecordDeclarationSyntax recordDeclarationSyntax =>
							model.GetDeclaredSymbol(recordDeclarationSyntax, cancellationToken),
						_ => null
					};
				}

				if (parentSymbol is INamedTypeSymbol namedTypeSymbol)
				{
					return namedTypeSymbol;
				}
				else if (parentSymbol is IMethodSymbol methodSymbol)
				{
					return methodSymbol.ContainingType;
				}
				else if (parentSymbol is ILocalSymbol localSymbol && localSymbol.Type is INamedTypeSymbol localNamedTypeSymbol)
				{
					return localNamedTypeSymbol;
				}
			}

			parent = parent.Parent;
		}

		return null;
	}
}