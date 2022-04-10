using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ClassCreateGeneratorTests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
@"using Rocks;
using System;

namespace MockTests
{
	public class ClassTest
	{
		public ClassTest() { }

		public virtual void Foo() { }
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<ClassTest>();
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
	internal static class CreateExpectationsOfClassTestExtensions
	{
		internal static MethodExpectations<ClassTest> Methods(this Expectations<ClassTest> self) =>
			new(self);
		
		internal static ClassTest Instance(this Expectations<ClassTest> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockClassTest(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockClassTest
			: ClassTest, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockClassTest(Expectations<ClassTest> expectations) =>
				this.handlers = expectations.Handlers;
			
			[MemberIdentifier(0, ""bool Equals(object? obj)"")]
			public override bool Equals(object? obj)
			{
				if (this.handlers.TryGetValue(0, out var methodHandlers))
				{
					foreach (var methodHandler in methodHandlers)
					{
						if (((methodHandler.Expectations[0] as Argument<object?>)?.IsValid(obj) ?? false))
						{
							var result = methodHandler.Method is not null ?
								((Func<object?, bool>)methodHandler.Method)(obj) :
								((HandlerInformation<bool>)methodHandler).ReturnValue;
							methodHandler.IncrementCallCount();
							return result!;
						}
					}
					
					throw new ExpectationException(""No handlers match for bool Equals(object? obj)"");
				}
				else
				{
					return base.Equals(obj);
				}
			}
			
			[MemberIdentifier(1, ""int GetHashCode()"")]
			public override int GetHashCode()
			{
				if (this.handlers.TryGetValue(1, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method is not null ?
						((Func<int>)methodHandler.Method)() :
						((HandlerInformation<int>)methodHandler).ReturnValue;
					methodHandler.IncrementCallCount();
					return result!;
				}
				else
				{
					return base.GetHashCode();
				}
			}
			
			[MemberIdentifier(2, ""string? ToString()"")]
			public override string? ToString()
			{
				if (this.handlers.TryGetValue(2, out var methodHandlers))
				{
					var methodHandler = methodHandlers[0];
					var result = methodHandler.Method is not null ?
						((Func<string?>)methodHandler.Method)() :
						((HandlerInformation<string?>)methodHandler).ReturnValue;
					methodHandler.IncrementCallCount();
					return result!;
				}
				else
				{
					return base.ToString();
				}
			}
			
			[MemberIdentifier(3, ""void Foo()"")]
			public override void Foo()
			{
				if (this.handlers.TryGetValue(3, out var methodHandlers))
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
					base.Foo();
				}
			}
			
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfClassTestExtensions
	{
		internal static MethodAdornments<ClassTest, Func<object?, bool>, bool> Equals(this MethodExpectations<ClassTest> self, Argument<object?> obj) =>
			new MethodAdornments<ClassTest, Func<object?, bool>, bool>(self.Add<bool>(0, new List<Argument>(1) { obj }));
		internal static MethodAdornments<ClassTest, Func<int>, int> GetHashCode(this MethodExpectations<ClassTest> self) =>
			new MethodAdornments<ClassTest, Func<int>, int>(self.Add<int>(1, new List<Argument>()));
		internal static MethodAdornments<ClassTest, Func<string?>, string?> ToString(this MethodExpectations<ClassTest> self) =>
			new MethodAdornments<ClassTest, Func<string?>, string?>(self.Add<string?>(2, new List<Argument>()));
		internal static MethodAdornments<ClassTest, Action> Foo(this MethodExpectations<ClassTest> self) =>
			new MethodAdornments<ClassTest, Action>(self.Add(3, new List<Argument>()));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "ClassTest_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}