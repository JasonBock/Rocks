Syntax nodes to look for:

* ClassDeclarationSyntax
* InterfaceDeclarationSyntax
* RecordDeclarationSyntax
* ParameterSyntax
* ObjectCreateExpressionSyntax

This is the code I need to keep around:

```c#
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;s
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocks.Extensions;
using Rocks.Models;
using System.Composition;

namespace Rocks;

/// <summary>
/// 
/// </summary>
[ExportCodeRefactoringProvider(LanguageNames.CSharp,
	Name = nameof(AddRockAttributeRefactoring))]
[Shared]
public sealed class AddRockAttributeRefactoring
	: CodeRefactoringProvider
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
	{
		var document = context.Document;

		if (document.Project.Solution.Workspace.Kind == WorkspaceKind.MiscellaneousFiles) { return; }

		var span = context.Span;

		if (!span.IsEmpty) { return; }

		var cancellationToken = context.CancellationToken;
		var model = await document.GetSemanticModelAsync(cancellationToken);

		if (model is null) { return; }

		var root = await model.SyntaxTree.GetRootAsync(cancellationToken);
		var node = root.FindNode(span);
		var mockTypeSymbol = node.FindParentSymbol(model, cancellationToken);

		if (mockTypeSymbol is not null)
		{
			var mockModel = MockModel.Create(
				node, mockTypeSymbol, null, new ModelContext(model), BuildType.Create, true);

			if (mockModel.Information is not null)
			{
				var newRoot = (CompilationUnitSyntax)root;

				if (!newRoot.HasUsing("Rocks"))
				{
					newRoot = newRoot.AddUsings(
						SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Rocks")));
				}

				// TODO: May need more here for the namespace.
				if (!root.HasUsing(mockTypeSymbol.ContainingNamespace.Name))
				{
					newRoot = newRoot.AddUsings(
						SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(mockTypeSymbol.ContainingNamespace.Name)));
				}

				var attributeSyntax = SyntaxFactory.ParseSyntaxTree(
					$"[assembly: Rock(typeof({mockTypeSymbol.Name}), BuildType.Create]")
					.GetRoot().DescendantNodes().OfType<AttributeListSyntax>().Single();
				newRoot = newRoot.AddAttributeLists(attributeSyntax);

				context.RegisterRefactoring(CodeAction.Create(
					"Add RockAttribute definition", token => Task.FromResult(document.WithSyntaxRoot(newRoot))));
			}
		}
	}
}
```

Also:

```c#
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading;

namespace Rocks.Extensions;

internal static class SyntaxNodeExtensions
{
	internal static INamedTypeSymbol? FindParentSymbol(this SyntaxNode self,
		SemanticModel model, CancellationToken cancellationToken)
	{
		Type[] syntaxNodeTypes = 
			[
				typeof(ClassDeclarationSyntax),
				typeof(InterfaceDeclarationSyntax),
				typeof(RecordDeclarationSyntax),
				typeof(ParameterSyntax),
				typeof(ObjectCreationExpressionSyntax),
			];

		var parent = self.Parent;

		while (parent is not null)
		{
			var syntaxNodeTypeMatch = syntaxNodeTypes.FirstOrDefault(
				t => parent.GetType() == t);

			if (syntaxNodeTypeMatch is not null)
			{
				var parentSymbol = parent switch
				{
					ClassDeclarationSyntax classDeclarationSyntax => 
						model.GetDeclaredSymbol(classDeclarationSyntax, cancellationToken),
					InterfaceDeclarationSyntax interfaceDeclarationSyntax =>
						model.GetDeclaredSymbol(interfaceDeclarationSyntax, cancellationToken),
					RecordDeclarationSyntax recordDeclarationSyntax =>
						model.GetDeclaredSymbol(recordDeclarationSyntax, cancellationToken),
					ParameterSyntax parameterSyntax => 
						((IParameterSymbol)model.GetSymbolInfo(parameterSyntax, cancellationToken).Symbol!).Type,
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

	internal static T? FindParent<T>(this SyntaxNode self)
		where T : SyntaxNode
	{
		var parent = self.Parent;

		while (parent is not T && parent is not null)
		{
			parent = parent.Parent;
		}

		return (T)parent!;
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
```