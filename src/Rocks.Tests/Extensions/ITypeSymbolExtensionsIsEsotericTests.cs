using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class ITypeSymbolExtensionsIsEsotericTests
{
	[TestCase("public class Target { public void Foo(string a) { } }", false)]
	[TestCase("public class Target { public unsafe void Foo(int* a) { } }", true)]
	[TestCase("public class Target { public unsafe void Foo(delegate*<int, void> a) { } }", true)]
	[TestCase("using System; public class Target { public void Foo(Span<int> a) { } }", true)]
	public static void IsEsoteric(string code, bool expectedResult) =>
		Assert.That(ITypeSymbolExtensionsIsEsotericTests.GetTypeSymbol(code).IsEsoteric(), Is.EqualTo(expectedResult));

	private static ITypeSymbol GetTypeSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type;
	}
}