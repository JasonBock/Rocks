﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Rocks.Tests.Targets;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Rocks.Tests
{
	public static class RockCreateGeneratorTests
	{
		[Test]
		public static void GenerateWhenTargetTypeContainsCompilerGeneratedMembers()
		{
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
@"using Rocks;
using Rocks.Tests.Targets;
using System;

namespace MockTests
{
	public interface IContainNullableReferences
	{
		string? DoSomething(string? a, string b) => string.Empty;
	}
	
	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<IContainNullableReferences>();
		}
	}
}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output.Length, Is.GreaterThan(0));
				Assert.That(output, Does.Not.Contain("[Nullable"));
				Assert.That(output, Does.Not.Contain("[NullableContext"));
			});
		}

		[Test]
		public static void GenerateWhenTargetTypeIsValid()
		{
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
@"using Rocks;
using System;

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
			var rock = Rock.Create<ITest>();
		}
	}
}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output, Does.Contain("internal static class CreateExpectationsOfITestExtensions"));
			});
		}

		[Test]
		public static void GenerateWhenTargetTypeIsInGlobalNamespace()
		{
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
@"using Rocks;
using System;

var rock = Rock.Create<ITest>();

public interface ITest
{
	void Foo();
}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output, Does.Contain("internal static class CreateExpectationsOfITestExtensions"));
			});
		}

		[Test]
		public static void GenerateWhenTargetTypeIsValidForRockRepository()
		{
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
@"using Rocks;
using System;

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
			var repository = new RockRepository();
			var rock = repository.Create<ITest>();
		}
	}
}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output, Does.Contain("internal static class CreateExpectationsOfITestExtensions"));
			});
		}

		[Test]
		public static void GenerateWhenInvocationExistsInTopLevelStatements()
		{
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
@"using MockTests;
using Rocks;
using System;

var rock = Rock.Create<ITest>();

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
				Assert.That(output, Does.Contain("internal static class CreateExpectationsOfITestExtensions"));
			});
		}

		[Test]
		public static void GenerateWhenTargetTypeIsInvalid()
		{
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
@"using Rocks;

namespace MockTests
{
	public interface ITest { }

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<ITest>();
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
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
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
			var rock = Rock.Create<ITest>();
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
			var (diagnostics, output) = RockCreateGeneratorTests.GetGeneratedOutput(
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
			var rock = Rock.Create<ITest>();
		}
// Note the missing closing brace
	}");

			Assert.Multiple(() =>
			{
				Assert.That(diagnostics.Length, Is.EqualTo(0));
				Assert.That(output, Does.Contain("internal static class CreateExpectationsOfITestExtensions"));
			});
		}

		private static (ImmutableArray<Diagnostic>, string) GetGeneratedOutput(string source, OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText(source);
			var references = AppDomain.CurrentDomain.GetAssemblies()
				.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
				.Select(_ => MetadataReference.CreateFromFile(_.Location))
				.Concat(new[] { MetadataReference.CreateFromFile(typeof(RockCreateGenerator).Assembly.Location) })
				.Concat(new[] { MetadataReference.CreateFromFile(typeof(IContainNullableReferences).Assembly.Location) });
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