using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class IMethodSymbolExtensionsMarkedWithDoesNotReturnTests
{
	[Test]
	public static void GetResultWhenMethodHasDoesNotReturnAttribute()
	{
		var code =
			"""
			using System.Diagnostics.CodeAnalysis;

			public static class Test
			{
				[DoesNotReturn]
				public static void Foo() { }
			}
			""";

		var (method, compilation) = IMethodSymbolExtensionsMarkedWithDoesNotReturnTests.GetMethodSymbol(code);
		Assert.That(method.IsMarkedWithDoesNotReturn(compilation), Is.True);
	}

	[Test]
	public static void GetResultWhenMethodDoesNotHaveDoesNotReturnAttribute()
	{
		var code =
			"""
			public static class Test
			{
				public static void Foo() { }
			}
			""";

		var (method, compilation) = IMethodSymbolExtensionsMarkedWithDoesNotReturnTests.GetMethodSymbol(code);
		Assert.That(method.IsMarkedWithDoesNotReturn(compilation), Is.False);
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