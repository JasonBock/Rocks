using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class IPropertySymbolExtensionsCanBeSeenByContainingAssemblyTests
{
	[Test]
	public static async Task CallWhenPropertyCanBeSeenAsync()
	{
		var code =
			"""
			public class Source
			{
				public string Data { get; }
			}
			""";
		var (symbol, compilation) = await GetSymbolAsync<PropertyDeclarationSyntax>(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.True);
	}

	[Test]
	public static async Task CallWhenPropertyCannotSeenAsync()
	{
		var code =
			"""
			public class Source
			{
				private string Data { get; }
			}
			""";
		var (symbol, compilation) = await GetSymbolAsync<PropertyDeclarationSyntax>(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.False);
	}

	[Test]
	public static async Task CallWhenPropertyTypeCanBeSeenAsync()
	{
		var code =
			"""
			public class Section { }
			
			public class Source
			{
				public Section Data { get; }
			}
			""";
		var (symbol, compilation) = await GetSymbolAsync<PropertyDeclarationSyntax>(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.True);
	}

	[Test]
	public static async Task CallWhenPropertyTypeCannotSeenAsync()
	{
		var code =
			"""
			public class Source
			{
				public Section Data { get; }

				protected class Section { }
			}
			""";
		var (symbol, compilation) = await GetSymbolAsync<PropertyDeclarationSyntax>(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.False);
	}

	[Test]
	public static async Task CallWhenIndexerParameterCanBeSeenAsync()
	{
		var code =
			"""
			public class Section { }
			
			public class Source
			{
				public string this[Section section] { get; }
			}
			""";
		var (symbol, compilation) = await GetSymbolAsync<IndexerDeclarationSyntax>(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.True);
	}

	[Test]
	public static async Task CallWhenIndexerParameterCannotSeenAsync()
	{
		var code =
			"""
			public class Source
			{
				public string this[Section section] { get; }
			
				protected class Section { }
			}
			""";
		var (symbol, compilation) = await GetSymbolAsync<IndexerDeclarationSyntax>(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly, compilation), Is.False);
	}

	private static async Task<(IPropertySymbol, Compilation)> GetSymbolAsync<TSyntax>(string source)
		where TSyntax : SyntaxNode
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", [syntaxTree],
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var propertySyntax = (await syntaxTree.GetRootAsync()).DescendantNodes(_ => true)
			.OfType<TSyntax>().Single();
		return ((IPropertySymbol)model.GetDeclaredSymbol(propertySyntax)!, compilation);
	}
}