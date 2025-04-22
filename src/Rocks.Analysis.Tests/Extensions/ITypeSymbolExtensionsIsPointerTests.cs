using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class ITypeSymbolExtensionsIsPointerTests
{
	[TestCase("public class Target { public void Foo(string a) { } }", false)]
	[TestCase("public class Target { public unsafe void Foo(int* a) { } }", true)]
	[TestCase("public class Target { public unsafe void Foo(delegate*<int, void> a) { } }", true)]
	[TestCase("using System; public class Target { public void Foo(Span<int> a) { } }", false)]
	public static void IsPointer(string code, bool expectedResult) =>
		Assert.That(ITypeSymbolExtensionsIsPointerTests.GetTypeSymbol(code).IsPointer(), Is.EqualTo(expectedResult));

	private static ITypeSymbol GetTypeSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type;
	}
}