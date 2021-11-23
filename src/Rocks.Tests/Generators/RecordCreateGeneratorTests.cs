using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public record Test
{
	public Test() { }

	public virtual void Foo() { }
}

public static class RecordCreateGeneratorTests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
@"using Rocks;
using System;

namespace MockTests
{
	public record RecordTest
	{
		public RecordTest() { }

		public virtual void Foo() { }
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<RecordTest>();
		}
	}
}";

		var generatedCode =
@"using MockTests;
using Rocks;
using Rocks.Exceptions;
using Rocks.Expectations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

#nullable enable
namespace MockTests
{
	internal static class CreateExpectationsOfRecordTestExtensions
	{
		internal static MethodExpectations<RecordTest> Methods(this Expectations<RecordTest> self) =>
			new(self);
		
		internal static RecordTest Instance(this Expectations<RecordTest> self)
		{
			var mock = new RockRecordTest(self);
			self.Mocks.Add(mock);
			return mock;
		}
		internal static RecordTest Instance(this Expectations<RecordTest> self, RecordTest original)
		{
			var mock = new RockRecordTest(self, original);
			self.Mocks.Add(mock);
			return mock;
		}
		
		private sealed record RockRecordTest
			: RecordTest, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockRecordTest(Expectations<RecordTest> expectations) =>
				this.handlers = expectations.Handlers;
			public RockRecordTest(Expectations<RecordTest> expectations, RecordTest original)
				: base(original) =>
					this.handlers = expectations.Handlers;
			
			[MemberIdentifier(2, ""void Foo()"")]
			public override void Foo()
			{
				if (this.handlers.TryGetValue(2, out var methodHandlers))
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
			
			[MemberIdentifier(3, ""string ToString()"")]
			public override string ToString()
			{
				if (this.handlers.TryGetValue(3, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method is not null ?
						((Func<string>)methodHandler.Method)() :
						((HandlerInformation<string>)methodHandler).ReturnValue;
					methodHandler.IncrementCallCount();
					return result!;
				}
				
				throw new ExpectationException(""No handlers were found for string ToString()"");
			}
			
			[MemberIdentifier(4, ""int GetHashCode()"")]
			public override int GetHashCode()
			{
				if (this.handlers.TryGetValue(4, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method is not null ?
						((Func<int>)methodHandler.Method)() :
						((HandlerInformation<int>)methodHandler).ReturnValue;
					methodHandler.IncrementCallCount();
					return result!;
				}
				
				throw new ExpectationException(""No handlers were found for int GetHashCode()"");
			}
			
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfRecordTestExtensions
	{
		internal static MethodAdornments<RecordTest, Action> Foo(this MethodExpectations<RecordTest> self) =>
			new MethodAdornments<RecordTest, Action>(self.Add(2, new List<Argument>()));
		internal static MethodAdornments<RecordTest, Func<string>, string> ToString(this MethodExpectations<RecordTest> self) =>
			new MethodAdornments<RecordTest, Func<string>, string>(self.Add<string>(3, new List<Argument>()));
		internal static MethodAdornments<RecordTest, Func<int>, int> GetHashCode(this MethodExpectations<RecordTest> self) =>
			new MethodAdornments<RecordTest, Func<int>, int>(self.Add<int>(4, new List<Argument>()));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "RecordTest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}