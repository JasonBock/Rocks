using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Extensions;

namespace Rocks.Analysis.Tests.Extensions;

public static class CompilationExtensionsTests
{
	[Test]
	public static void GetAliasesWhenNoneExist()
	{
		var compilation = CompilationExtensionsTests.GetCompilation("test", [], []);
		Assert.That(compilation.GetAliases(), Is.Empty);
	}

	[Test]
	public static void GetAliasesWhenOneExistsOnOneReference()
	{
		var compilation = CompilationExtensionsTests.GetCompilation("test", ["first"], []);
		Assert.That(compilation.GetAliases(), Has.Length.EqualTo(1));
	}

	[Test]
	public static void GetAliasesWhenMultipleExistOnOneReference()
	{
		var compilation = CompilationExtensionsTests.GetCompilation("test", ["first", "second"], []);
		Assert.That(compilation.GetAliases(), Has.Length.EqualTo(1));
	}

	[Test]
	public static void GetAliasesWhenOneExistsOnTwoReferences()
	{
		var compilation = CompilationExtensionsTests.GetCompilation("test", ["first"], ["second"]);

		using (Assert.EnterMultipleScope())
		{
			var aliases = compilation.GetAliases();
			Assert.That(aliases, Has.Length.EqualTo(2));
			Assert.That(aliases[0], Is.EqualTo("first"));
			Assert.That(aliases[1], Is.EqualTo("second"));
		}
	}

	[Test]
	public static void GetAliasesWhenMultipleExistsOnTwoReferences()
	{
		var compilation = CompilationExtensionsTests.GetCompilation("test", ["first", "third"], ["second", "fourth"]);

		using (Assert.EnterMultipleScope())
		{
			var aliases = compilation.GetAliases();
			Assert.That(aliases, Has.Length.EqualTo(2));
			Assert.That(aliases[0], Is.EqualTo("first"));
			Assert.That(aliases[1], Is.EqualTo("second"));
		}
	}

	[Test]
	public static void GetAliasesWhenOneDuplicateExistsOnTwoReferences()
	{
		var compilation = CompilationExtensionsTests.GetCompilation("test", ["first"], ["first"]);

		using (Assert.EnterMultipleScope())
		{
			var aliases = compilation.GetAliases();
			Assert.That(aliases, Has.Length.EqualTo(1));
			Assert.That(aliases[0], Is.EqualTo("first"));
		}
	}

	[Test]
	public static void GetAliasesWhenOneExistsOnTwoReferencesReordered()
	{
		var compilation = CompilationExtensionsTests.GetCompilation("test", ["second"], ["first"]);

		using (Assert.EnterMultipleScope())
		{
			var aliases = compilation.GetAliases();
			Assert.That(aliases, Has.Length.EqualTo(2));
			Assert.That(aliases[0], Is.EqualTo("first"));
			Assert.That(aliases[1], Is.EqualTo("second"));
		}
	}

	private static CSharpCompilation GetCompilation(string name, string[] firstAliases, string[] secondAliases)
	{
		var firstSource = "public class Test { }";
		var firstSyntaxTree = CSharpSyntaxTree.ParseText(firstSource);
		var firstCompilation = CSharpCompilation.Create(name, [firstSyntaxTree],
			options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		var secondSource = "public class Test { }";
		var secondSyntaxTree = CSharpSyntaxTree.ParseText(secondSource);
		var secondCompilation = CSharpCompilation.Create(name, [secondSyntaxTree],
			options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

		var finalSource = "public class Final { }";
		var finalSyntaxTree = CSharpSyntaxTree.ParseText(finalSource);
		return CSharpCompilation.Create(name, [finalSyntaxTree],
			references:
			[
				firstCompilation.ToMetadataReference().WithAliases(firstAliases),
				secondCompilation.ToMetadataReference().WithAliases(secondAliases)
			],
			options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
	}
}