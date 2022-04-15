using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class IndexerInitCreateGeneratorTests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
@"using Rocks;
using System;

namespace MockTests
{
	public abstract class Target
	{
		public abstract int this[int a] { get; init; }
	}
	
	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<Target>();
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
	internal static class CreateExpectationsOfTargetExtensions
	{
		internal static MethodExpectations<Target> Methods(this Expectations<Target> self) =>
			new(self);
		
		internal static IndexerExpectations<Target> Indexers(this Expectations<Target> self) =>
			new(self);
		
		internal static IndexerGetterExpectations<Target> Getters(this IndexerExpectations<Target> self) =>
			new(self.Expectations);
		
		internal static Target Instance(this Expectations<Target> self)
		{
			if (self.Mock is null)
			{
				var mock = new RockTarget(self);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockTarget
			: Target, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockTarget(Expectations<Target> expectations) =>
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
			
			[MemberIdentifier(3, ""this[int a]"")]
			[MemberIdentifier(4, ""this[int a]"")]
			public override int this[int a]
			{
				get
				{
					if (this.handlers.TryGetValue(3, out var methodHandlers))
					{
						foreach (var methodHandler in methodHandlers)
						{
							if (((methodHandler.Expectations[0] as Argument<int>)?.IsValid(a) ?? false))
							{
								var result = methodHandler.Method is not null ?
									((Func<int, int>)methodHandler.Method)(a) :
									((HandlerInformation<int>)methodHandler).ReturnValue;
								methodHandler.IncrementCallCount();
								return result!;
							}
						}
						
						throw new ExpectationException(""No handlers match for this[int a]"");
					}
					
					throw new ExpectationException(""No handlers were found for this[int a])"");
				}
				init { }
			}
			
			Dictionary<int, List<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}
	
	internal static class MethodExpectationsOfTargetExtensions
	{
		internal static MethodAdornments<Target, Func<object?, bool>, bool> Equals(this MethodExpectations<Target> self, Argument<object?> obj) =>
			new MethodAdornments<Target, Func<object?, bool>, bool>(self.Add<bool>(0, new List<Argument>(1) { obj }));
		internal static MethodAdornments<Target, Func<int>, int> GetHashCode(this MethodExpectations<Target> self) =>
			new MethodAdornments<Target, Func<int>, int>(self.Add<int>(1, new List<Argument>()));
		internal static MethodAdornments<Target, Func<string?>, string?> ToString(this MethodExpectations<Target> self) =>
			new MethodAdornments<Target, Func<string?>, string?>(self.Add<string?>(2, new List<Argument>()));
	}
	
	internal static class IndexerGetterExpectationsOfTargetExtensions
	{
		internal static IndexerAdornments<Target, Func<int, int>, int> This(this IndexerGetterExpectations<Target> self, Argument<int> a) =>
			new IndexerAdornments<Target, Func<int, int>, int>(self.Add<int>(3, new List<Argument>(1) { a }));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "Target_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}