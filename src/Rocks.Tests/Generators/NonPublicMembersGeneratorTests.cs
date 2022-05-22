using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class NonPublicMembersGeneratorTests
{
	//[Test]
	//public static async Task CreateWithProtectedVirtualMethodAsync()
	//{

	//}

	[Test]
	public static async Task CreateWithProtectedAbstractMethodAsync()
	{
		var code =
@"using Rocks;
using System;

namespace MockTests
{
	public abstract class Test
	{
		protected abstract void FindMe();
	}

	public static class TestUser
	{
		public static void Generate()
		{
			var rock = Rock.Create<Test>();
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
	internal static class CreateExpectationsOfTestExtensions
	{
		internal static MethodExpectations<Test> Methods(this Expectations<Test> self) =>
			new(self);
		
		internal static Test Instance(this Expectations<Test> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockTest(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockTest
			: Test, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockTest(Expectations<Test> expectations) =>
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
			
			[MemberIdentifier(3, ""void FindMe()"")]
			protected override void FindMe()
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
					throw new ExpectationException(""No handlers were found for void FindMe()"");
				}
			}
			
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfTestExtensions
	{
		internal static MethodAdornments<Test, Func<object?, bool>, bool> Equals(this MethodExpectations<Test> self, Argument<object?> obj) =>
			new MethodAdornments<Test, Func<object?, bool>, bool>(self.Add<bool>(0, new List<Argument>(1) { obj }));
		internal static MethodAdornments<Test, Func<int>, int> GetHashCode(this MethodExpectations<Test> self) =>
			new MethodAdornments<Test, Func<int>, int>(self.Add<int>(1, new List<Argument>()));
		internal static MethodAdornments<Test, Func<string?>, string?> ToString(this MethodExpectations<Test> self) =>
			new MethodAdornments<Test, Func<string?>, string?>(self.Add<string?>(2, new List<Argument>()));
		internal static MethodAdornments<Test, Action> FindMe(this MethodExpectations<Test> self) =>
			new MethodAdornments<Test, Action>(self.Add(3, new List<Argument>()));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "Test_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	[Test]
	public static async Task MakeWithProtectedVirtualMethodAsync()
	{
		var code =
@"using Rocks;
using System;

namespace MockTests
{
	public abstract class Test
	{
		protected abstract void FindMe();
	}

	public static class TestUser
	{
		public static void Generate()
		{
			var rock = Rock.Make<Test>();
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
	internal static class MakeExpectationsOfTestExtensions
	{
		internal static Test Instance(this Expectations<Test> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockTest(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only Make a new mock once."");
			}
		}
		
		private sealed class RockTest
			: Test, IMock
		{
			
			public RockTest(Expectations<Test> expectations) =>
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
			
			[MemberIdentifier(3, ""void FindMe()"")]
			protected override void FindMe()
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
					throw new ExpectationException(""No handlers were found for void FindMe()"");
				}
			}
			
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "Test_Rock_Make.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}

	//[Test]
	//public static async Task MakeWithProtectedAbstractMethodAsync()
	//{

	//}
}