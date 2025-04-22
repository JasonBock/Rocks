using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class INamedTypeSymbolExtensionsIsHasOpenGenericTests
{
	[TestCase("public class A<T1, T2, T3> { } public class Target { public void Foo(int a) { } }", false)]
	[TestCase("public class A<T1, T2, T3> { } public class Target { public void Foo<T1, T3>(A<T1, int, T3> a) { } }", true)]
	[TestCase("public class A<T1, T2, T3> { } public class Target { public void Foo(A<string, int, int> a) { } }", false)]
	[TestCase("public class A<T1, T2, T3> { } public class Target { public void Foo(A<string, int, A<string, int, int>> a) { } }", false)]
	[TestCase("public class A<T1, T2, T3> { } public class Target { public void Foo<T3>(A<string, int, A<string, T3, int>> a) { } }", true)]
	public static void HasOpenGenerics(string code, bool expectedValue)
	{
		var typeSymbol = INamedTypeSymbolExtensionsIsHasOpenGenericTests.GetNamedTypeSymbol(code);

		Assert.That(typeSymbol.HasOpenGenerics(), Is.EqualTo(expectedValue));
	}

	private static INamedTypeSymbol GetNamedTypeSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return (INamedTypeSymbol)model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type;
	}
}