using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class IParameterSymbolExtensionsTests
{
	[TestCase("public class Test { public void Foo(string a) { } }", false)]
	[TestCase("public class Test { public void Foo(scoped ref int a) { } }", true)]
	[TestCase("using System; public class Test { public void Foo(scoped Span<int> a) { } }", true)]
	public static void IsScoped(string code, bool expectedValue)
	{
		var symbol = IParameterSymbolExtensionsTests.GetParameterSymbol(code);
		Assert.That(symbol.IsScoped(), Is.EqualTo(expectedValue));
	}

	[TestCase("public class Test { public void Foo(string a) { } }", false)]
	[TestCase("public class Test { public void Foo(string a = \"c\") { } }", false)]
	[TestCase("public class Test { public void Foo(string a = null) { } }", true)]
	[TestCase("public class Test { public void Foo(string? a = null) { } }", false)]
	public static void RequiresForcedNullableAnnotation(string code, bool expectedValue)
	{
		var symbol = IParameterSymbolExtensionsTests.GetParameterSymbol(code);
		Assert.That(symbol.RequiresForcedNullableAnnotation(), Is.EqualTo(expectedValue));
	}

	private static IParameterSymbol GetParameterSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return model.GetDeclaredSymbol(methodSyntax)!.Parameters[0];
	}
}