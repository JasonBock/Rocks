using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocks.Completions.Extensions;
using System.Composition;

namespace Rocks.Completions;

/// <summary>
/// Suggests refactorings to create the
/// <c>RockAttribute</c> declaration for the type
/// at the current code location (if possible).
/// </summary>
[ExportCodeRefactoringProvider(LanguageNames.CSharp,
	Name = nameof(AddRockAttributeRefactoring))]
[Shared]
public sealed class AddRockAttributeRefactoring
	: CodeRefactoringProvider
{
	/// <inheritdoc />
	public override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
	{
		var document = context.Document;

		if (document.Project.Solution.Workspace.Kind == WorkspaceKind.MiscellaneousFiles) { return; }

		var span = context.Span;

		//if (!span.IsEmpty) { return; }

		var cancellationToken = context.CancellationToken;
		var model = await document.GetSemanticModelAsync(cancellationToken);

		if (model is null) { return; }

		var root = await model.SyntaxTree.GetRootAsync(cancellationToken);
		var node = root.FindNode(span);
		var mockTypeSymbol = node.FindParentSymbol(model, cancellationToken);

		if (mockTypeSymbol is not null)
		{
			// TODO: This code exists in Rocks.Analysis. We don't want to reference that 
			// project explicitly. We could create a shared project, but the stuff within
			// MockModel is quite extensive. For now, we're not going to check if this is a valid
			// type to mock. When the attribute is created, the check will kick in anyway.

			//var mockModel = MockModel.Create(
			//	node, mockTypeSymbol, null, new ModelContext(model), BuildType.Create, true);

			//if (mockModel.Information is not null)

			var newRoot = (CompilationUnitSyntax)root;

			if (!newRoot.HasUsing("Rocks.Runtime"))
			{
				newRoot = newRoot.AddUsings(
					SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Rocks.Runtime")));
			}

			// TODO: May need more here for the namespace.
			if (!mockTypeSymbol.ContainingNamespace.IsGlobalNamespace &&
				!root.HasUsing(mockTypeSymbol.ContainingNamespace.Name))
			{
				newRoot = newRoot.AddUsings(
					SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(mockTypeSymbol.ContainingNamespace.Name)));
			}

			// TODO: We COULD also create a [RockPartialAttribute] declaration,
			// but that requires being tied to a partial class, so that would have to be created as well,
			// and what name would we give it? and where would it go?
			var attributeSyntax = SyntaxFactory.ParseSyntaxTree(
				$"""
				[assembly: Rock(typeof({mockTypeSymbol.Name}), BuildType.Create)]
				""")
				.GetRoot().DescendantNodes().OfType<AttributeListSyntax>().Single()
				.WithTrailingTrivia(
					SyntaxFactory.EndOfLine(Environment.NewLine),
					SyntaxFactory.EndOfLine(Environment.NewLine));

			newRoot = newRoot.AddAttributeLists(attributeSyntax);

			context.RegisterRefactoring(CodeAction.Create(
				"Add RockAttribute definition", token => Task.FromResult(document.WithSyntaxRoot(newRoot))));
		}
	}
}

