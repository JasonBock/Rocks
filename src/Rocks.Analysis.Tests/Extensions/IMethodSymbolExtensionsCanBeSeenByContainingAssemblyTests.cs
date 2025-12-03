using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class IMethodSymbolExtensionsCanBeSeenByContainingAssemblyTests
{
	[Test]
	public static async Task CallWhenMethodCanBeSeenAsync()
	{
		var code =
			"""
			public class Source
			{
				public void Foo() { }
			}
			""";
		var (symbol, compilation) = await IMethodSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.True);
	}

	[Test]
	public static async Task CallWhenMethodCannotBeSeenAsync()
	{
		var code =
			"""
			public class Source
			{
				private void Foo() { }
			}
			""";
		var (symbol, compilation) = await IMethodSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.False);
	}

	[Test]
	public static async Task CallWhenMethodParameterTypeCanBeSeenAsync()
	{
		var code =
			"""
			public class Section { }

			public class Source
			{
				public void Foo(Section section) { }
			}
			""";
		var (symbol, compilation) = await IMethodSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.True);
	}

	[Test]
	public static async Task CallWhenMethodParameterTypeCannotBeSeenAsync()
	{
		var code =
			"""
			public class Source
			{
				public void Foo(Section section) { }

				protected class Section { }
			}
			""";
		var (symbol, compilation) = await IMethodSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.False);
	}

	[Test]
	public static async Task CallWhenMethodReturnTypeCanBeSeenAsync()
	{
		var code =
			"""
			public class Section { }

			public class Source
			{
				public Section Foo() => default;
			}
			""";
		var (symbol, compilation) = await IMethodSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.True);
	}

	[Test]
	public static async Task CallWhenMethodReturnTypeCannotBeSeenAsync()
	{
		var code =
			"""
			public class Source
			{
				public Section Foo() => default;

				protected class Section { }
			}
			""";
		var (symbol, compilation) = await IMethodSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.False);
	}

	private static async Task<(IMethodSymbol, Compilation)> GetSymbolAsync(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(methodSyntax)!, compilation);
	}
}