using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Models;

namespace Rocks.Analysis.Tests.Models;

public static class ConstructorModelTests
{
	[Test]
	public static async Task CreateAsync()
	{
		var code =
			"""
			public class Target
			{
				public Target(string value) { }
			}
			""";

		(var type, var constructor, var modelContext) = await ConstructorModelTests.GetSymbolsCompilationAsync(code);
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
	public static async Task CreateWhenSetsRequiredMembersExistsAsync()
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

		(var type, var constructor, var modelContext) = await ConstructorModelTests.GetSymbolsCompilationAsync(code);
		var mockType = modelContext.CreateTypeReference(type);
		var model = new ConstructorModel(constructor, modelContext);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(model.RequiresSetsRequiredMembersAttribute, Is.True);
			Assert.That(model.Parameters, Has.Length.EqualTo(1));
			Assert.That(model.Parameters[0].Name, Is.EqualTo("value"));
		}
	}

	private static async Task<(ITypeSymbol, IMethodSymbol, ModelContext)> GetSymbolsCompilationAsync(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);
		var root = await syntaxTree.GetRootAsync();

		var typeSyntax = root.DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		var methodSyntax = root.DescendantNodes(_ => true)
			.OfType<ConstructorDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(typeSyntax)!, model.GetDeclaredSymbol(methodSyntax)!, new(model));
	}
}