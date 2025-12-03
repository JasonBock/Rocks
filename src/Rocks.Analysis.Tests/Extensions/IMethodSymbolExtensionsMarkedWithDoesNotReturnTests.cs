using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class IMethodSymbolExtensionsMarkedWithDoesNotReturnTests
{
	[Test]
	public static async Task GetResultWhenMethodHasDoesNotReturnAttributeAsync()
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

		var (method, compilation) = await IMethodSymbolExtensionsMarkedWithDoesNotReturnTests.GetMethodSymbolAsync(code);
		Assert.That(method.IsMarkedWithDoesNotReturn(compilation), Is.True);
	}

	[Test]
	public static async Task GetResultWhenMethodDoesNotHaveDoesNotReturnAttributeAsync()
	{
		var code =
			"""
			public static class Test
			{
				public static void Foo() { }
			}
			""";

		var (method, compilation) = await IMethodSymbolExtensionsMarkedWithDoesNotReturnTests.GetMethodSymbolAsync(code);
		Assert.That(method.IsMarkedWithDoesNotReturn(compilation), Is.False);
	}

	private static async Task<(IMethodSymbol, Compilation)> GetMethodSymbolAsync(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single(_ => _.Identifier.Text == "Foo");
		return (model.GetDeclaredSymbol(methodSyntax)!, compilation);
	}
}