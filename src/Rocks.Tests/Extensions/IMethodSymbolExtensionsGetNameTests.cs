using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class IMethodSymbolExtensionsGetNameTests
{
	[TestCase("public class Target { public void Foo<T>() { } }", MethodNameOption.NoGenerics, "Foo")]
	[TestCase("public class Target { public void Foo<T>() { } }", MethodNameOption.IncludeGenerics, "Foo<T>")]
	public static void GetName(string code, MethodNameOption option, string expectedName)
	{
		var methodSymbol = IMethodSymbolExtensionsGetNameTests.GetMethodSymbol(code);
		var name = methodSymbol.GetName(option);

		Assert.That(name, Is.EqualTo(expectedName));
	}

	private static IMethodSymbol GetMethodSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Where(_ => _.Identifier.Text == "Foo").Single();
		return model.GetDeclaredSymbol(methodSyntax)!;
	}
}