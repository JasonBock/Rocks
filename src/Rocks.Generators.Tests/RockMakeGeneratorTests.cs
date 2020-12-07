using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Tests
{
	public static class RockMakeGeneratorTests
	{
		[Test]
		public static void GenerateWhenTargetTypeIsValid()
		{
			var (diagnostics, output) = RockMakeGeneratorTests.GetGeneratedOutput(
@"using Rocks;
using System;

namespace MockTests
{
	public abstract class AbstractClassConstructor
	{
		protected AbstractClassConstructor(string stringData) => 
			this.StringData = stringData;
		public AbstractClassConstructor(int intData) => 
			this.IntData = intData;
		
		public abstract int NoParameters();

		public int IntData { get;  }
		public string? StringData { get; }
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Make<AbstractClassConstructor>();
		}
	}
}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output, Does.Contain("internal static class MakeExpectationsOfIMockExtensions"));
			});
		}

		[Test]
		public static void GenerateWhenInvocationExistsInTopLevelStatements()
		{
			var (diagnostics, output) = RockMakeGeneratorTests.GetGeneratedOutput(
@"using Rocks;
using System;

var rock = Rock.Make<ITest>();

namespace MockTests
{
	public interface ITest
	{
		void Foo();
	}
}", OutputKind.ConsoleApplication);

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output, Does.Contain("internal static class MakeExpectationsOfIMockExtensions"));
			});
		}

		[Test]
		public static void GenerateWhenTargetTypeIsInvalid()
		{
			var (diagnostics, output) = RockMakeGeneratorTests.GetGeneratedOutput(
@"using Rocks;

namespace MockTests
{
	public interface ITest { }

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Make<ITest>();
		}
	}
}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.GreaterThan(0));
				Assert.That(output, Is.EqualTo(string.Empty));
			});
		}

		[Test]
		public static void GenerateWhenTargetTypeHasDiagnostics()
		{
			var (diagnostics, output) = RockMakeGeneratorTests.GetGeneratedOutput(
@"using Rocks;

namespace MockTests
{
	public interface ITest 
	{ 
		// Note the missing semicolon
		void Foo()
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Make<ITest>();
		}
	}
}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output, Is.EqualTo(string.Empty));
			});
		}

		[Test]
		public static void GenerateWhenTargetTypeIsValidButOtherCodeHasDiagnostics()
		{
			var (diagnostics, output) = RockMakeGeneratorTests.GetGeneratedOutput(
@"using Rocks;

namespace MockTests
{
	public interface ITest 
	{ 
		void Foo();
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Make<ITest>();
		}
// Note the missing closing brace
	}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output, Does.Contain("internal static class MakeExpectationsOfIMockExtensions"));
			});
		}

		private static (ImmutableArray<Diagnostic>, string) GetGeneratedOutput(string source, OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location))
				.Concat(new[] { MetadataReference.CreateFromFile(typeof(RockMakeGenerator).Assembly.Location) });
			var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
				references, new CSharpCompilationOptions(outputKind));
			var originalTreeCount = compilation.SyntaxTrees.Length;

			var generator = new RockMakeGenerator();

			var driver = CSharpGeneratorDriver.Create(ImmutableArray.Create<ISourceGenerator>(generator));
			driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

			var trees = outputCompilation.SyntaxTrees.ToList();

			return (diagnostics, trees.Count != originalTreeCount ? trees[^1].ToString() : string.Empty);
		}
	}
}