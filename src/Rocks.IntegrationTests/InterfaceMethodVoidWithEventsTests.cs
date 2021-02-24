using NUnit.Framework;
using System;

namespace Rocks.IntegrationTests
{
	public interface IInterfaceMethodVoidWithEvents
	{
		void NoParameters();
		event EventHandler MyEvent;
	}

	public static class InterfaceMethodVoidWithEventsTests
	{
		[Test]
		public static void CreateEvent()
		{
			var rock = Rock.Create<IInterfaceMethodVoidWithEvents>();
			rock.Methods().NoParameters().RaisesMyEvent(EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			chunk.NoParameters();

			rock.Verify();

			Assert.That(wasEventRaised, Is.True);
		}

		[Test]
		public static void CreateEventWithCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<IInterfaceMethodVoidWithEvents>();
			rock.Methods().NoParameters()
				.Callback(() => wasCallbackInvoked = true)
				.RaisesMyEvent(EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			chunk.NoParameters();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(wasEventRaised, Is.True);
				Assert.That(wasCallbackInvoked, Is.True);
			});
		}

		[Test]
		public static void CreateEventWithMultipleCalls()
		{
			var rock = Rock.Create<IInterfaceMethodVoidWithEvents>();
			rock.Methods().NoParameters()
				.CallCount(2)
				.RaisesMyEvent(EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => eventRaisedCount++;
			chunk.NoParameters();
			chunk.NoParameters();

			rock.Verify();

			Assert.That(eventRaisedCount, Is.EqualTo(2));
		}

		[Test]
		public static void CreateEventWithMultipleCallsWithCallback()
		{
			var callbackInvokedCount = 0;
			var rock = Rock.Create<IInterfaceMethodVoidWithEvents>();
			rock.Methods().NoParameters()
				.CallCount(2)
				.Callback(() => callbackInvokedCount++)
				.RaisesMyEvent(EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => eventRaisedCount++;
			chunk.NoParameters();
			chunk.NoParameters();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(eventRaisedCount, Is.EqualTo(2));
				Assert.That(callbackInvokedCount, Is.EqualTo(2));
			});
		}
	}
}