using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class IMethodSymbolExtensionsRequiresProjectedDelegateTests
{
	[TestCase("public class Target { public void Foo(int a) { } }", false)]
	[TestCase("public class Target { public void Foo(ref int a) { } }", true)]
	[TestCase("public class Target { public void Foo(out int a) { } }", true)]
	[TestCase("public class Target { public unsafe void Foo(int* a) { } }", true)]
	[TestCase("public class Target { public int Foo() => default; }", false)]
	[TestCase("public class Target { public unsafe int* Foo() => default; }", true)]
	[TestCase(
		"""
		public class Target 
		{ 
			public void Foo(int i0, int i1, int i2, int i3, int i4,
				int i5, int i6, int i7, int i8, int i9,
				int i10, int i11, int i12, int i13, int i14,
				int i15, int i16, int i17, int i18, int i19) { } 
		}
		""", true)]
	public static void RequiresProjectedDelegate(string code, bool expectedValue)
	{
		var (typeSymbol, compilation) = IMethodSymbolExtensionsRequiresProjectedDelegateTests.GetMethodSymbol(code);

		Assert.That(typeSymbol.RequiresProjectedDelegate(compilation), Is.EqualTo(expectedValue));
	}

	private static (IMethodSymbol, Compilation) GetMethodSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single(_ => _.Identifier.Text == "Foo");
		return (model.GetDeclaredSymbol(methodSyntax)!, compilation);
	}
}