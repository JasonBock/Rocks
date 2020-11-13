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
		public static void GenerateHappyPathForIndexers()
		{
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
@"using Rocks;
using System;

namespace EII
{
	public interface IA
	{
		int this[int x, string y, Guid z] { get; set; }
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<IA>();
		}
	}
}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output, Does.Contain("internal static class ExpectationsOfICExtensions"));
			});
		}

		[Test]
		public static void GenerateHappyPathForEvents()
		{
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
@"using Rocks;
using System;

namespace EII
{
	public interface IA
	{
		void Foo();
		event EventHandler MyEvent;
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<IA>();
		}
	}
}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output, Does.Contain("internal static class ExpectationsOfICExtensions"));
			});
		}

		[Test]
		public static void GenerateHappyPathForGenericEvents()
		{
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
@"using Rocks;
using System;

namespace EII
{
	public class MyEventArgs
		: EventArgs { }

	public interface IA
	{
		void Foo();
		event EventHandler<MyEventArgs> MyEvent;
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<IA>();
		}
	}
}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output, Does.Contain("internal static class ExpectationsOfICExtensions"));
			});
		}

		[Test]
		public static void GenerateHappyPathForExplicitInterfaceImplementation()
		{
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
@"using Rocks;

namespace EII
{
	public interface IA
	{
		void Foo();
	}

	public interface IB
	{
		void Foo();
	}

	public interface IC
		: IA, IB
	{ }

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<IC>();
		}
	}
}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output, Does.Contain("internal static class ExpectationsOfICExtensions"));
			});
		}

		[Test]
		public static void GenerateHappyPath()
		{
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
@"using Rocks;

public interface IMock 
{ 
	void Foo();
}

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
				Assert.That(output, Does.Contain("internal static class ExpectationsOfIMockExtensions"));
			});
		}

		[Test]
		public static void GenerateMultipleCalls()
		{
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
@"using Rocks;

namespace MockTests
{
	public interface IMock 
	{ 
		void Foo();
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<IMock>();
			var rock2 = Rock.Create<IMock>();
		}
	}
}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output, Does.Contain("internal static class ExpectationsOfIMockExtensions"));
			});
		}

		private static (ImmutableArray<Diagnostic>, string) GetGeneratedOutput(string source)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location))
				.Concat(new[] { MetadataReference.CreateFromFile(typeof(RockCreateGenerator).Assembly.Location) });
			var compilation = CSharpCompilation.Create("generator", new SyntaxTree[] { syntaxTree },
				references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
			var originalTreeCount = compilation.SyntaxTrees.Length;

			var generator = new RockCreateGenerator();

			var driver = CSharpGeneratorDriver.Create(ImmutableArray.Create<ISourceGenerator>(generator));
			driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

			var trees = outputCompilation.SyntaxTrees.ToList();

			return (diagnostics, trees.Count != originalTreeCount ? trees[^1].ToString() : string.Empty);
		}
	}
}