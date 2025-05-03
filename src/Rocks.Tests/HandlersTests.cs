using NUnit.Framework;

namespace Rocks.Tests;

public static class HandlersTests
{
	private sealed class HandlerTest
		: Handler<Action<int>>
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public Argument<int> input { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	}

	[Test]
	public static void Create()
	{
		var handlers = new Handlers<HandlerTest>(new() { input = 1 });
		Assert.That(handlers.First.input.IsValid(1), Is.True);
	}

	[Test]
	public static void AddOne()
	{
		var handlers = new Handlers<HandlerTest>(new() { input = 1 })
		{
			new() { input = 2 },
		};

		Assert.Multiple(() =>
		{
			var index = 1;

			foreach (var handler in handlers)
			{
				Assert.That(handler.input.IsValid(index), Is.True);
				index++;
			}

			Assert.That(index, Is.EqualTo(3));
		});
	}

	[Test]
	public static void AddTwo()
	{
		var handlers = new Handlers<HandlerTest>(new() { input = 1 })
		{
			new() { input = 2 },
			new() { input = 3 },
		};

		Assert.Multiple(() =>
		{
			var index = 1;

			foreach (var handler in handlers)
			{
				Assert.That(handler.input.IsValid(index), Is.True);
				index++;
			}

			Assert.That(index, Is.EqualTo(4));
		});
	}
}