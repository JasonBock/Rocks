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
public sealed partial class AddRockAttributeRefactoring
	: CodeRefactoringProvider
{
	/// <inheritdoc />
	public override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
	{
		var document = context.Document;

		if (document.Project.Solution.Workspace.Kind == WorkspaceKind.MiscellaneousFiles) { return; }

		var span = context.Span;
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

			if (mockTypeSymbol.IsUnboundGenericType)
			{
				mockTypeSymbol = mockTypeSymbol.OriginalDefinition;
			}

			var newRoot = (CompilationUnitSyntax)root;

			// Figure out which document we should actually put the changes in.
			var options = context.TextDocument.Project.AnalyzerOptions
				.AnalyzerConfigOptionsProvider.GlobalOptions;

			if (options.TryGetValue("build_property.RocksAttributeFile", out var mockFile))
			{
				var fullMockFilePath = document.Project.FilePath is not null ?
					Path.Combine(Path.GetDirectoryName(document.Project.FilePath), mockFile) :
					mockFile;
				var mockDocument = document.Project.Documents.FirstOrDefault(_ => _.FilePath == fullMockFilePath);

				if (mockDocument is not null)
				{
					document = mockDocument;
					newRoot = (CompilationUnitSyntax)(await (await mockDocument.GetSemanticModelAsync(cancellationToken))!.SyntaxTree.GetRootAsync(cancellationToken));
				}
			}

			AddRockAttributeRefactoring.AddRockAttribute(newRoot, mockTypeSymbol, context, document);
		}
	}

	private static void AddRockAttribute(
		CompilationUnitSyntax root, INamedTypeSymbol mockTypeSymbol,
		CodeRefactoringContext context, Document document)
	{
		static CompilationUnitSyntax CreateNewRoot(
			CompilationUnitSyntax root, bool addCreateType, bool addMakeType, INamedTypeSymbol mockTypeSymbol)
		{
			var buildTypeArgument = (addCreateType && addMakeType) ?
				SyntaxFactory.AttributeArgument(
					SyntaxFactory.BinaryExpression(
						SyntaxKind.BitwiseOrExpression,
						SyntaxFactory.MemberAccessExpression(
							SyntaxKind.SimpleMemberAccessExpression,
							SyntaxFactory.MemberAccessExpression(
								SyntaxKind.SimpleMemberAccessExpression,
								SyntaxFactory.MemberAccessExpression(
									SyntaxKind.SimpleMemberAccessExpression,
									SyntaxFactory.IdentifierName("Rocks"),
									SyntaxFactory.IdentifierName("Runtime")),
								SyntaxFactory.IdentifierName("BuildType")),
							SyntaxFactory.IdentifierName("Create")),
						SyntaxFactory.MemberAccessExpression(
							SyntaxKind.SimpleMemberAccessExpression,
							SyntaxFactory.MemberAccessExpression(
								SyntaxKind.SimpleMemberAccessExpression,
								SyntaxFactory.MemberAccessExpression(
									SyntaxKind.SimpleMemberAccessExpression,
									SyntaxFactory.IdentifierName("Rocks"),
									SyntaxFactory.IdentifierName("Runtime")),
								SyntaxFactory.IdentifierName("BuildType")),
							SyntaxFactory.IdentifierName("Make")))) :
				SyntaxFactory.AttributeArgument(
					SyntaxFactory.MemberAccessExpression(
						SyntaxKind.SimpleMemberAccessExpression,
						SyntaxFactory.MemberAccessExpression(
							SyntaxKind.SimpleMemberAccessExpression,
							SyntaxFactory.MemberAccessExpression(
								SyntaxKind.SimpleMemberAccessExpression,
								SyntaxFactory.IdentifierName("Rocks"),
								SyntaxFactory.IdentifierName("Runtime")),
							SyntaxFactory.IdentifierName("BuildType")),
						SyntaxFactory.IdentifierName(addCreateType ? "Create" : "Make")));

			// This creates:
			//
			// [assembly: Rocks.Runtime.Rock(typeof(MockTypeName), Rocks.Runtime.BuildType.Create]
			// 
			// The buildTypeArgument above figures out how to create
			// the correct values for BuildType
			// https://roslynquoter.azurewebsites.net/

			var attributeSyntax = SyntaxFactory.AttributeList(
				SyntaxFactory.SingletonSeparatedList(
					 SyntaxFactory.Attribute(
						  SyntaxFactory.QualifiedName(
								SyntaxFactory.QualifiedName(
									 SyntaxFactory.IdentifierName("Rocks"),
									 SyntaxFactory.IdentifierName("Runtime")),
								SyntaxFactory.IdentifierName("Rock")))
				.WithArgumentList(
					SyntaxFactory.AttributeArgumentList(
						SyntaxFactory.SeparatedList<AttributeArgumentSyntax>(
							new SyntaxNodeOrToken[]
							{
								SyntaxFactory.AttributeArgument(
									SyntaxFactory.TypeOfExpression(mockTypeSymbol.GetFullName())),
								SyntaxFactory.Token(SyntaxKind.CommaToken),
								buildTypeArgument
							})))))
				.WithTarget(
					SyntaxFactory.AttributeTargetSpecifier(
						SyntaxFactory.Token(SyntaxKind.AssemblyKeyword)));

			return root.AddAttributeLists(attributeSyntax);
		}

		context.RegisterRefactoring(
			CodeAction.Create("Add RockAttribute definition...",
			[
				CodeAction.Create("Create",
					token => Task.FromResult(document.WithSyntaxRoot(CreateNewRoot(root, true, false, mockTypeSymbol)))),
				CodeAction.Create("Make",
					token => Task.FromResult(document.WithSyntaxRoot(CreateNewRoot(root, false, true, mockTypeSymbol)))),
				CodeAction.Create("Create and Make",
					token => Task.FromResult(document.WithSyntaxRoot(CreateNewRoot(root, true, true, mockTypeSymbol)))),
			], false));
	}
}