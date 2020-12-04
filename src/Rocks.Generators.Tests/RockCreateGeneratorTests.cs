using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Tests
{
	public static class RockCreateGeneratorTests
	{
		[Test]
		public static void GenerateWhenTargetTypeIsValid()
		{
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
@"using Rocks;
using System;
using System.Threading.Tasks;

namespace MockTests
{
	public interface IMock
	{
		void Foo(int a, params string[] b);
		int this[int a, params string[] b] { get; }
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<IMock>();
		}
	}
}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output, Does.Contain("internal static class ExpectationsOfIMockExtensions"));
			});
		}

		[Test]
		public static void GenerateWhenInvocationExistsInTopLevelStatements()
		{
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
@"using Rocks;
using System;

var rock = Rock.Create<IMock>();

namespace MockTests
{
	public interface IMock
	{
		void Foo();
	}
}", OutputKind.ConsoleApplication);

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output, Does.Contain("internal static class ExpectationsOfIMockExtensions"));
			});
		}

		[Test]
		public static void GenerateWhenTargetTypeIsInvalid()
		{
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
@"using Rocks;

namespace MockTests
{
	public interface IMock { }

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<IMock>();
		}
	}
}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.GreaterThan(0));
				Assert.That(output, Is.EqualTo(string.Empty));
			});
		}

		private static (ImmutableArray<Diagnostic>, string) GetGeneratedOutput(string source, OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location))
				.Concat(new[] { MetadataReference.CreateFromFile(typeof(RockCreateGenerator).Assembly.Location) });
			var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
				references, new CSharpCompilationOptions(outputKind));
			var originalTreeCount = compilation.SyntaxTrees.Length;

			var generator = new RockCreateGenerator();

			var driver = CSharpGeneratorDriver.Create(ImmutableArray.Create<ISourceGenerator>(generator));
			driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

			var trees = outputCompilation.SyntaxTrees.ToList();

			return (diagnostics, trees.Count != originalTreeCount ? trees[^1].ToString() : string.Empty);
		}
	}
}