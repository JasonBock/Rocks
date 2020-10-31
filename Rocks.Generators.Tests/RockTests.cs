using NUnit.Framework;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Rocks.Tests
{
	public static class RockTests
	{
		[Test]
		public static void Walkthrough()
		{
			// This is purely a test to see if the code I plan on generating would work.
			var rock = Rock.Create<IMockable>();
			rock.Methods().Foo(Arg.Is(3)).CallCount(2);

			var chunk = rock.Instance();
			chunk.Foo(3);
			chunk.Foo(3);

			rock.Verify();
		}
	}

	public interface IMockable
	{
		void Foo(int a);
	}

	internal static class ExpectationsOfIMockableExtensions
	{
		internal static MethodExpectations<IMockable> Methods(this Expectations<IMockable> self) =>
			new MethodExpectations<IMockable>(self);

		internal static IMockable Instance(this Expectations<IMockable> self)
		{
			var mock = new RockIMockable(self);
			self.Mocks.Add(mock);
			return mock;
		}

		private sealed class RockIMockable
			: IMockable, IMock
		{
			private readonly ImmutableDictionary<int, ImmutableArray<HandlerInformation>> handlers;

			public RockIMockable(Expectations<IMockable> expectations) => 
				this.handlers = expectations.CreateHandlers();

			[MemberIdentifier(0, "Foo(int a)")]
			public void Foo(int a)
			{
				if (this.handlers.TryGetValue(0, out var methodHandlers))
				{
					var foundMatch = false;

					foreach (var methodHandler in methodHandlers)
					{
						if (((Arg<int>)methodHandler.Expectations["a"]).IsValid(a))
						{
							foundMatch = true;

							if (methodHandler.Method != null)
							{
#pragma warning disable CS8604
								((Action<int>)methodHandler.Method)(a);
#pragma warning restore CS8604
							}

							methodHandler.IncrementCallCount();
							break;
						}
					}

					if (!foundMatch)
					{
						throw new ExpectationException($"No handlers were found for Foo({a})");
					}
				}
				else
				{
					throw new ExpectationException($"No handlers were found for Foo({a})");
				}
			}

			ImmutableDictionary<int, ImmutableArray<HandlerInformation>> IMock.Handlers => this.handlers;
		}
	}

	internal static class MethodExpectationsOfIMockableExtensions
	{
		internal static MethodAdornments Foo(this MethodExpectations<IMockable> self, Arg<int> a) =>
			new MethodAdornments(self.Add(0, new Dictionary<string, Arg>
			{
				{ "a", a }
			}));
	}
}