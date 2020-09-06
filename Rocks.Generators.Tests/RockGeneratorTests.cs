using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Tests
{
	public static class RockGeneratorTests
	{
		[Test]
		public static void GenerateHappyPath()
		{
			var (diagnostics, output) = RockGeneratorTests.GetGeneratedOutput(
@"using Rocks;

public interface IMock { }

public static class Test
{
	public static void Generate()
	{
		var rock = Rock.Create<IMock>();
	}
}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output, Does.Contain("public static class ExpectationsOfIMockExtensions"));
			});
		}

		private static (ImmutableArray<Diagnostic>, string) GetGeneratedOutput(string source)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location))
				.Concat(new[] { MetadataReference.CreateFromFile(typeof(Rock).Assembly.Location) });
			var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
				references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
			var generator = new RockGenerator();

			var driver = new CSharpGeneratorDriver(compilation.SyntaxTrees[0].Options,
				ImmutableArray.Create<ISourceGenerator>(generator), default!, ImmutableArray<AdditionalText>.Empty);
			driver.RunFullGeneration(compilation, out var outputCompilation, out var diagnostics);

			var trees = outputCompilation.SyntaxTrees.ToList();

			return (diagnostics, trees.Count == 2 ? trees[^1].ToString() : string.Empty);
		}
	}
}