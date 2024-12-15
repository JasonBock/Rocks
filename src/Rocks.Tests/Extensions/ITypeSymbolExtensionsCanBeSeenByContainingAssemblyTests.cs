using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class ITypeSymbolExtensionsCanBeSeenByContainingAssemblyTests
{
	[Test]
	public static void CallWhenTypeCanBeSeen()
	{
		var code =
			"""
			public class Section { }
			
			public class Source
			{
				public void Foo(Section section) { }
			}
			""";
		var (symbol, compilation) = ITypeSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.True);
	}

	[Test]
	public static void CallWhenTypeCannotBeSeen()
	{
		var code =
			"""
			public class Source
			{
				public void Foo(Section section) { }

				protected class Section { }
			}
			""";
		var (symbol, compilation) = ITypeSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.False);
	}

	[Test]
	public static void CallWhenGenericTypeCanBeSeen()
	{
		var code =
			"""
			using System.Collections.Generic;

			public class Section { }
			
			public class Source
			{
				public void Foo(List<Section> sections) { }
			}
			""";
		var (symbol, compilation) = ITypeSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.True);
	}

	[Test]
	public static void CallWhenGenericTypeCannotBeSeen()
	{
		var code =
			"""
			using System.Collections.Generic;
			
			public class Source
			{
				public void Foo(List<Section> sections) { }

				protected class Section { }
			}
			""";
		var (symbol, compilation) = ITypeSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.False);
	}

	private static (ITypeSymbol, Compilation) GetSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type, compilation);
	}
}