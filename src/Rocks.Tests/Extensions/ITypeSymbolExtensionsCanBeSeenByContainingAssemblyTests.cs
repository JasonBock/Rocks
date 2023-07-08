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
		var symbol = ITypeSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.True);
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
		var symbol = ITypeSymbolExtensionsCanBeSeenByContainingAssemblyTests.GetSymbol(code);

		Assert.That(symbol.CanBeSeenByContainingAssembly(symbol.ContainingAssembly), Is.False);
	}

	private static ITypeSymbol GetSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var methodSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<MethodDeclarationSyntax>().Single();
		return model.GetDeclaredSymbol(methodSyntax)!.Parameters[0].Type;
	}
}