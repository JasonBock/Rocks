using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class MethodAdornmentTests
	{
		[Test]
		public void Create()
		{
			var info = new HandlerInformation();
			var adornments = new MethodAdornments(info);

			var events = info.GetRaiseEvents();
			Assert.AreEqual(0, events.Count, nameof(events.Count));
		}

		[Test]
		public void Raises()
		{
			var args = new EventArgs();
         var info = new HandlerInformation();
         var adornments = new MethodAdornments(info);
			var newAdornments = adornments.Raises("a", args);

			var events = info.GetRaiseEvents();
			Assert.AreEqual(1, events.Count);
			var @event = events[0];
			Assert.AreEqual("a", @event.Name, nameof(@event.Name));
			Assert.AreSame(args, @event.Args, nameof(@event.Args));
			Assert.AreSame(adornments, newAdornments, nameof(newAdornments));
		}
	}
}
