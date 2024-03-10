using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class IMethodSymbolExtensionsCanBeSeenByContainingAssemblyTests
{
	[Test]
	public static void CallWhenMethodCanBeSeen()
	{
		var code =
			"""
			public class Source
			{
				public void Foo() { }
			}
			""";
		var symbol = IMethodSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.True);
	}

	[Test]
	public static void CallWhenMethodCannotBeSeen()
	{
		var code =
			"""
			public class Source
			{
				private void Foo() { }
			}
			""";
		var symbol = IMethodSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.False);
	}

	[Test]
	public static void CallWhenMethodParameterTypeCanBeSeen()
	{
		var code =
			"""
			public class Section { }

			public class Source
			{
				public void Foo(Section section) { }
			}
			""";
		var symbol = IMethodSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.True);
	}

	[Test]
	public static void CallWhenMethodParameterTypeCannotBeSeen()
	{
		var code =
			"""
			public class Source
			{
				public void Foo(Section section) { }

				protected class Section { }
			}
			""";
		var symbol = IMethodSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.False);
	}

	[Test]
	public static void CallWhenMethodReturnTypeCanBeSeen()
	{
		var code =
			"""
			public class Section { }

			public class Source
			{
				public Section Foo() => default;
			}
			""";
		var symbol = IMethodSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.True);
	}

	[Test]
	public static void CallWhenMethodReturnTypeCannotBeSeen()
	{
		var code =
			"""
			public class Source
			{
				public Section Foo() => default;

				protected class Section { }
			}
			""";
		var symbol = IMethodSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.False);
	}

	private static IMethodSymbol GetSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return model.GetDeclaredSymbol(methodSyntax)!;
	}
}