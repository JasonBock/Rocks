using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class IPropertySymbolExtensionsCanBeSeenByContainingAssemblyTests
{
	[Test]
	public static void CallWhenPropertyCanBeSeen()
	{
		var code =
			"""
			public class Source
			{
				public string Data { get; }
			}
			""";
		var symbol = IPropertySymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol<PropertyDeclarationSyntax>(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.True);
	}

	[Test]
	public static void CallWhenPropertyCannotSeen()
	{
		var code =
			"""
			public class Source
			{
				private string Data { get; }
			}
			""";
		var symbol = IPropertySymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol<PropertyDeclarationSyntax>(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.False);
	}

	[Test]
	public static void CallWhenPropertyTypeCanBeSeen()
	{
		var code =
			"""
			public class Section { }
			
			public class Source
			{
				public Section Data { get; }
			}
			""";
		var symbol = IPropertySymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol<PropertyDeclarationSyntax>(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.True);
	}

	[Test]
	public static void CallWhenPropertyTypeCannotSeen()
	{
		var code =
			"""
			public class Source
			{
				public Section Data { get; }

				protected class Section { }
			}
			""";
		var symbol = IPropertySymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol<PropertyDeclarationSyntax>(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.False);
	}

	[Test]
	public static void CallWhenIndexerParameterCanBeSeen()
	{
		var code =
			"""
			public class Section { }
			
			public class Source
			{
				public string this[Section section] { get; }
			}
			""";
		var symbol = IPropertySymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol<IndexerDeclarationSyntax>(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.True);
	}

	[Test]
	public static void CallWhenIndexerParameterCannotSeen()
	{
		var code =
			"""
			public class Source
			{
				public string this[Section section] { get; }
			
				protected class Section { }
			}
			""";
		var symbol = IPropertySymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol<IndexerDeclarationSyntax>(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.False);
	}

	private static IPropertySymbol GetSymbol<TSyntax>(string source)
		where TSyntax : SyntaxNode
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var propertySyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<TSyntax>().Single();
		return (IPropertySymbol)model.GetDeclaredSymbol(propertySyntax)!;
	}
}