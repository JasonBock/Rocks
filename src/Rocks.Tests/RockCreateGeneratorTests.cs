using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;
using Rocks.Tests.Targets;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Rocks.Tests
{
	public static class RockCreateGeneratorTests
	{
		[Test]
		public static async Task GenerateWhenTargetTypeContainsCompilerGeneratedMembersAsync()
		{
			var code =
@"using Rocks;
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
}";

			var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using Rocks.Expectations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

#nullable enable
namespace MockTests
{
	internal static class CreateExpectationsOfIContainNullableReferencesExtensions
	{
		internal static MethodExpectations<IContainNullableReferences> Methods(this Expectations<IContainNullableReferences> self) =>
			new(self);
		
		internal static IContainNullableReferences Instance(this Expectations<IContainNullableReferences> self)
		{
			var mock = new RockIContainNullableReferences(self);
			self.Mocks.Add(mock);
			return mock;
		}
		
		private sealed class RockIContainNullableReferences
			: IContainNullableReferences, IMock
		{
			private readonly ImmutableDictionary<int, ImmutableArray<HandlerInformation>> handlers;
			
			public RockIContainNullableReferences(Expectations<IContainNullableReferences> expectations) =>
				this.handlers = expectations.CreateHandlers();
			
			[MemberIdentifier(0, ""string? DoSomething(string? a, string b)"")]
			public string? DoSomething(string? a, string b)
			{
				if (this.handlers.TryGetValue(0, out var methodHandlers))
				{
					foreach (var methodHandler in methodHandlers)
					{
						if (((methodHandler.Expectations[0] as Argument<string?>)?.IsValid(a) ?? false) &&
							((methodHandler.Expectations[1] as Argument<string>)?.IsValid(b) ?? false))
						{
							var result = methodHandler.Method is not null ?
								((Func<string?, string, string?>)methodHandler.Method)(a, b) :
								((HandlerInformation<string?>)methodHandler).ReturnValue;
							methodHandler.IncrementCallCount();
							return result!;
						}
					}
				}
				
				throw new ExpectationException(""No handlers were found for string? DoSomething(string? a, string b)"");
			}
			
			
			ImmutableDictionary<int, ImmutableArray<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfIContainNullableReferencesExtensions
	{
		internal static MethodAdornments<IContainNullableReferences, Func<string?, string, string?>, string?> DoSomething(this MethodExpectations<IContainNullableReferences> self, Argument<string?> a, Argument<string> b) =>
			new MethodAdornments<IContainNullableReferences, Func<string?, string, string?>, string?>(self.Add<string?>(0, new List<Argument> { a, b }));
	}
}
";

			await TestAssistants.RunAsync<RockCreateGenerator>(code,
				new[] { (typeof(RockCreateGenerator), "IContainNullableReferences_Rock_Create.g.cs", generatedCode) },
				Enumerable.Empty<DiagnosticResult>());
		}

		[Test]
		public static async Task GenerateWhenTargetTypeIsValidAsync()
		{
			var code =
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
}";

			var generatedCode =
@"using Rocks;
using Rocks.Exceptions;
using Rocks.Expectations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

#nullable enable
namespace MockTests
{
	internal static class CreateExpectationsOfITestExtensions
	{
		internal static MethodExpectations<ITest> Methods(this Expectations<ITest> self) =>
			new(self);
		
		internal static ITest Instance(this Expectations<ITest> self)
		{
			var mock = new RockITest(self);
			self.Mocks.Add(mock);
			return mock;
		}
		
		private sealed class RockITest
			: ITest, IMock
		{
			private readonly ImmutableDictionary<int, ImmutableArray<HandlerInformation>> handlers;
			
			public RockITest(Expectations<ITest> expectations) =>
				this.handlers = expectations.CreateHandlers();
			
			[MemberIdentifier(0, ""void Foo()"")]
			public void Foo()
			{
				if (this.handlers.TryGetValue(0, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					if (methodHandler.Method is not null)
					{
						((Action)methodHandler.Method)();
					}
					
					methodHandler.IncrementCallCount();
				}
				else
				{
					throw new ExpectationException(""No handlers were found for void Foo())"");
				}
			}
			
			
			ImmutableDictionary<int, ImmutableArray<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfITestExtensions
	{
		internal static MethodAdornments<ITest, Action> Foo(this MethodExpectations<ITest> self) =>
			new MethodAdornments<ITest, Action>(self.Add(0, new List<Argument>()));
	}
}
";

			await TestAssistants.RunAsync<RockCreateGenerator>(code,
				new[] { (typeof(RockCreateGenerator), "ITest_Rock_Create.g.cs", generatedCode) },
				Enumerable.Empty<DiagnosticResult>());
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