using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class INamespaceSymbolExtensionsTests
{
	[Test]
	public static void GetName()
	{
		var namespaceValue = "MyNamespace.Stuff";
		var code = $"namespace {namespaceValue} {{ }}";
		var @namespace = INamespaceSymbolExtensionsTests.GetNamespaceSymbol(code);
		Assert.That(@namespace.GetName(), Is.EqualTo(namespaceValue));
	}

	[Test]
	public static void GetNameWhenNamespaceIsNull() =>
		Assert.That((null as INamespaceSymbol).GetName(), Is.EqualTo(string.Empty));

	private static INamespaceSymbol GetNamespaceSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			Shared.References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<NamespaceDeclarationSyntax>().Single();
		return model.GetDeclaredSymbol(typeSyntax)!;
	}
}