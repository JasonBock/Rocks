using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class ITypeSymbolExtensionsCanBeSeenByContainingAssemblyTests
{
	[Test]
	public static async Task CallWhenTypeCanBeSeenAsync()
	{
		var code =
			"""
			public class Section { }
			
			public class Source
			{
				public void Foo(Section section) { }
			}
			""";
		var (symbol, compilation) = await ITypeSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.True);
	}

	[Test]
	public static async Task CallWhenTypeCannotBeSeenAsync()
	{
		var code =
			"""
			public class Source
			{
				public void Foo(Section section) { }

				protected class Section { }
			}
			""";
		var (symbol, compilation) = await ITypeSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.False);
	}

	[Test]
	public static async Task CallWhenGenericTypeCanBeSeenAsync()
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
		var (symbol, compilation) = await ITypeSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.True);
	}

	[Test]
	public static async Task CallWhenGenericTypeCannotBeSeenAsync()
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
		var (symbol, compilation) = await ITypeSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbolAsync(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.False);
	}

	private static async Task<(ITypeSymbol, Compilation)> GetSymbolAsync(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return (model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type, compilation);
	}
}