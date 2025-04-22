using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class IMethodSymbolExtensionsIsUnsafeTests
{
	[TestCase("public class Target { public void Foo() { } }", false)]
	[TestCase("public class Target { public void Foo(int a) { } }", false)]
	[TestCase("public class Target { public unsafe void Foo(int* a) { } }", true)]
	[TestCase("public class Target { public int Foo() => default; }", false)]
	[TestCase("public class Target { public unsafe int* Foo() => default; }", true)]
	public static void IsUnsafe(string code, bool expectedValue)
	{
		var methodSymbol = IMethodSymbolExtensionsIsUnsafeTests.GetMethodSymbol(code);

		Assert.That(methodSymbol.IsUnsafe(), Is.EqualTo(expectedValue));
	}

	private static IMethodSymbol GetMethodSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single(_ => _.Identifier.Text == "Foo");
		return model.GetDeclaredSymbol(methodSyntax)!;
	}
}