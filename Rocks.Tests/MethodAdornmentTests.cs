using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class MethodAdornmentTests
	{
		[Test]
		public static void Create()
		{
			var info = new HandlerInformation();
			var adornments = new MethodAdornments(info);

			var events = info.GetRaiseEvents();
			Assert.That(events.Count, Is.EqualTo(0), nameof(events.Count));
		}

		[Test]
		public static void Raises()
		{
			var args = new EventArgs();
			var info = new HandlerInformation();
			var adornments = new MethodAdornments(info);
			var newAdornments = adornments.Raises("a", args);

			var events = info.GetRaiseEvents();
			Assert.That(events.Count, Is.EqualTo(1), nameof(events.Count));
			var @event = events[0];
			Assert.That(@event.Name, Is.EqualTo("a"), nameof(@event.Name));
			Assert.That(@event.Args, Is.EqualTo(args), nameof(@event.Args));
			Assert.That(newAdornments, Is.EqualTo(adornments), nameof(newAdornments));
		}
	}
}
