using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Models;

namespace Rocks.Tests.Models;

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

		(var type, var constructor, var compilation) = ConstructorModelTests.GetSymbolsCompilation(code);
		var mockType = new TypeReferenceModel(type, compilation);
		var model = new ConstructorModel(constructor, mockType, compilation);

		Assert.Multiple(() =>
		{
			Assert.That(model.MockType, Is.SameAs(mockType));
			Assert.That(model.Parameters, Has.Length.EqualTo(1));
			Assert.That(model.Parameters[0].Name, Is.EqualTo("value"));
		});
	}

	private static (ITypeSymbol, IMethodSymbol, Compilation) GetSymbolsCompilation(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TypeDeclarationSyntax>().Single();
		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<ConstructorDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(typeSyntax)!, model.GetDeclaredSymbol(methodSyntax)!, compilation);
	}
}