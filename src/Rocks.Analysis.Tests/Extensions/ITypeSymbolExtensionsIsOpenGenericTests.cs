using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class ITypeSymbolExtensionsIsOpenGenericTests
{
	[TestCase("public class A<T1, T2, T3> { } public class Target { public void Foo(int a) { } }", false)]
	[TestCase("public class A<T1, T2, T3> { } public class Target { public void Foo<T1, T3>(A<T1, int, T3> a) { } }", true)]
	[TestCase("public class A<T1, T2, T3> { } public class Target { public void Foo(A<string, int, int> a) { } }", false)]
	public static void IsOpenGeneric(string code, bool expectedValue)
	{
		var typeSymbol = ITypeSymbolExtensionsIsOpenGenericTests.GetTypeSymbol(code);

		Assert.That(typeSymbol.IsOpenGeneric(), Is.EqualTo(expectedValue));
	}

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