using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Builders;

namespace Rocks.Tests.Builders;

public static class NamespaceGathererTests
{
	[Test]
	public static void AddNamespaceFromType()
	{
		var gatherer = new NamespaceGatherer();
		var listType = typeof(List<>);
		gatherer.Add(listType);

		var namespaces = gatherer.Values;

		Assert.Multiple(() =>
		{
			Assert.That(namespaces, Has.Count.EqualTo(1));
			Assert.That(namespaces, Contains.Item(listType.Namespace));
		});
	}

	[Test]
	public static void AddNamespaceSymbol()
	{
		var namespaceValue = "MyNamespace.Stuff";
		var code = $"namespace {namespaceValue} {{ }}";
		var @namespace = NamespaceGathererTests.GetNamespaceSymbol(code);

		var gatherer = new NamespaceGatherer();
		gatherer.Add(@namespace);

		var namespaces = gatherer.Values;

		Assert.Multiple(() =>
		{
			Assert.That(namespaces.Count, Is.EqualTo(1));
			Assert.That(namespaces, Contains.Item(namespaceValue));
		});
	}

	[Test]
	public static void AddSameNamespaceSymbol()
	{
		var namespaceValue = "MyNamespace.Stuff";
		var code = $"namespace {namespaceValue} {{ }}";
		var @namespace = NamespaceGathererTests.GetNamespaceSymbol(code);

		var gatherer = new NamespaceGatherer();
		gatherer.Add(@namespace);
		gatherer.Add(@namespace);

		var namespaces = gatherer.Values;

		Assert.Multiple(() =>
		{
			Assert.That(namespaces, Has.Count.EqualTo(1));
			Assert.That(namespaces, Contains.Item(namespaceValue));
		});
	}

	[Test]
	public static void AddNamespaceSymbols()
	{
		var namespaceValue1 = "MyNamespace.Stuff";
		var code1 = $"namespace {namespaceValue1} {{ }}";
		var namespace1 = NamespaceGathererTests.GetNamespaceSymbol(code1);

		var namespaceValue2 = "MyNamespace.Things";
		var code2 = $"namespace {namespaceValue2} {{ }}";
		var namespace2 = NamespaceGathererTests.GetNamespaceSymbol(code2);

		var gatherer = new NamespaceGatherer();
		gatherer.AddRange(new[] { namespace1, namespace2 });

		var namespaces = gatherer.Values;

		Assert.Multiple(() =>
		{
			Assert.That(namespaces.Count, Is.EqualTo(2));
			Assert.That(namespaces, Contains.Item(namespaceValue1));
			Assert.That(namespaces, Contains.Item(namespaceValue2));
		});
	}

	private static INamespaceSymbol GetNamespaceSymbol(string source)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location));
		var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		var model = compilation.GetSemanticModel(syntaxTree, true);

		var typeSyntax = syntaxTree.GetRoot().DescendantNodes(_ => true)
			.OfType<NamespaceDeclarationSyntax>().Single();
		return model.GetDeclaredSymbol(typeSyntax)!;
	}
}