using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;

namespace Rocks.Tests.Generators;

public static class ClassConstructorCreateGeneratorTests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
@"using Rocks;
using System;

namespace MockTests
{
	public class BaseCtor
	{
		 public BaseCtor(int a, ref string b, out string c, params string[] d) { c = ""c""; }

		 public virtual void Foo() { }
	}

	public static class Test
	{
		public static void Generate()
		{
			var rock = Rock.Create<BaseCtor>();
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
	internal static class CreateExpectationsOfBaseCtorExtensions
	{
		internal static MethodExpectations<BaseCtor> Methods(this Expectations<BaseCtor> self) =>
			new(self);
		
		internal static BaseCtor Instance(this Expectations<BaseCtor> self, int a, ref string b, out string c, params string[] d)
		{
			if (self.Mock is null)
			{
				var mock = new RockBaseCtor(self, a, ref b, out c, d);
				self.Mock = mock;
				return mock;
			}
			else
			{
				throw new NewMockInstanceException(""Can only create a new mock once."");
			}
		}
		
		private sealed class RockBaseCtor
			: BaseCtor, IMock
		{
			private readonly Dictionary<int, List<HandlerInformation>> handlers;
			
			public RockBaseCtor(Expectations<BaseCtor> expectations, int a, ref string b, out string c, params string[] d)
				: base(a, ref b, out c, d) =>
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
	
	internal static class MethodExpectationsOfBaseCtorExtensions
	{
		internal static MethodAdornments<BaseCtor, Func<object?, bool>, bool> Equals(this MethodExpectations<BaseCtor> self, Argument<object?> obj) =>
			new MethodAdornments<BaseCtor, Func<object?, bool>, bool>(self.Add<bool>(0, new List<Argument>(1) { obj }));
		internal static MethodAdornments<BaseCtor, Func<int>, int> GetHashCode(this MethodExpectations<BaseCtor> self) =>
			new MethodAdornments<BaseCtor, Func<int>, int>(self.Add<int>(1, new List<Argument>()));
		internal static MethodAdornments<BaseCtor, Func<string?>, string?> ToString(this MethodExpectations<BaseCtor> self) =>
			new MethodAdornments<BaseCtor, Func<string?>, string?>(self.Add<string?>(2, new List<Argument>()));
		internal static MethodAdornments<BaseCtor, Action> Foo(this MethodExpectations<BaseCtor> self) =>
			new MethodAdornments<BaseCtor, Action>(self.Add(3, new List<Argument>()));
	}
}
";

		await TestAssistants.RunAsync<RockCreateGenerator>(code,
			new[] { (typeof(RockCreateGenerator), "BaseCtor_Rock_Create.g.cs", generatedCode) },
			Enumerable.Empty<DiagnosticResult>()).ConfigureAwait(false);
	}
}