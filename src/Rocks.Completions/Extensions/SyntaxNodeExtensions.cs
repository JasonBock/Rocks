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
				typeof(ClassDeclarationSyntax),
				typeof(InterfaceDeclarationSyntax),
				typeof(RecordDeclarationSyntax),
				typeof(ParameterSyntax),
				typeof(ObjectCreationExpressionSyntax),
			];

		var parent = self;

		while (parent is not null)
		{
			var syntaxNodeTypeMatch = syntaxNodeTypes.FirstOrDefault(
				t => parent.GetType() == t);

			if (syntaxNodeTypeMatch is not null)
			{
				var parentSymbol = parent switch
				{
					IdentifierNameSyntax identifierNameSyntax =>
						model.GetSymbolInfo(identifierNameSyntax, cancellationToken).Symbol,
					ClassDeclarationSyntax classDeclarationSyntax =>
						model.GetDeclaredSymbol(classDeclarationSyntax, cancellationToken),
					InterfaceDeclarationSyntax interfaceDeclarationSyntax =>
						model.GetDeclaredSymbol(interfaceDeclarationSyntax, cancellationToken),
					RecordDeclarationSyntax recordDeclarationSyntax =>
						model.GetDeclaredSymbol(recordDeclarationSyntax, cancellationToken),
					ParameterSyntax parameterSyntax =>
						((IParameterSymbol)model.GetDeclaredSymbol(parameterSyntax, cancellationToken)!).Type,
					ObjectCreationExpressionSyntax objectCreationExpressionSyntax =>
						((IMethodSymbol)model.GetSymbolInfo(objectCreationExpressionSyntax, cancellationToken).Symbol!).ContainingType,
					_ => null
				};

				if (parentSymbol is INamedTypeSymbol namedTypeSymbol)
				{
					return namedTypeSymbol;
				}
			}

			parent = parent.Parent;
		}

		return null;
	}

	internal static bool HasUsing(this SyntaxNode self, string qualifiedName)
	{
		if (self is null) { throw new ArgumentNullException(nameof(self)); }

		var root = self;

		while (true)
		{
			if (root.Parent is not null)
			{
				root = root.Parent;
			}
			else
			{
				break;
			}
		}

		var usingNodes = root.DescendantNodes(_ => true).OfType<UsingDirectiveSyntax>();

		foreach (var usingNode in usingNodes)
		{
			if (usingNode.Name!.ToFullString().Contains(qualifiedName))
			{
				return true;
			}
		}

		return false;
	}
}