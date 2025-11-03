using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Models;

namespace Rocks.Analysis.Tests.Models;

public static class ConstructorModelTests
{
	[Test]
	public static void Create()
	{
		var code =
			"""
			public class Target
			{
				public Target(string value) { }
			}
			""";

		(var type, var constructor, var modelContext) = ConstructorModelTests.GetSymbolsCompilation(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new ConstructorModel(constructor, modelContext);

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(model.RequiresSetsRequiredMembersAttribute, Is.False);
			Assert.That(model.Parameters, Has.Length.EqualTo(1));
			Assert.That(model.Parameters[0].Name, Is.EqualTo("value"));
		}
	}

	[Test]
	public static void CreateWhenSetsRequiredMembersExists()
	{
		var code =
			"""
			using System.Diagnostics.CodeAnalysis;
			
			public class Target
			{
				[SetsRequiredMembers]
				public Target(string value) { }
			}
			""";

		(var type, var constructor, var modelContext) = ConstructorModelTests.GetSymbolsCompilation(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new ConstructorModel(constructor, modelContext);

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(model.RequiresSetsRequiredMembersAttribute, Is.True);
			Assert.That(model.Parameters, Has.Length.EqualTo(1));
			Assert.That(model.Parameters[0].Name, Is.EqualTo("value"));
		}
	}

	private static (ITypeSymbol, IMethodSymbol, ModelContext) GetSymbolsCompilation(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<ConstructorDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(typeSyntax)!, model.GetDeclaredSymbol(methodSyntax)!, new(model));
	}
}