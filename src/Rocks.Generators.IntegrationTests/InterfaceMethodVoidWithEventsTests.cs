using NUnit.Framework;
using System;

namespace Rocks.IntegrationTests
{
	public interface IInterfaceMethodVoidWithEventsTests
	{
		void NoParameters();
		event EventHandler MyEvent;
	}

	public static class InterfaceMethodVoidWithEventsTests
	{
		[Test]
		public static void MockEvent()
		{
			var rock = Rock.Create<IInterfaceMethodVoidWithEventsTests>();
			rock.Methods().NoParameters().RaisesMyEvent(EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			chunk.NoParameters();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(wasEventRaised, Is.True);
			});
		}

		[Test]
		public static void MockEventWithCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<IInterfaceMethodVoidWithEventsTests>();
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
		public static void MockEventWithMultipleCalls()
		{
			var rock = Rock.Create<IInterfaceMethodVoidWithEventsTests>();
			rock.Methods().NoParameters()
				.CallCount(2)
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
			});
		}

		[Test]
		public static void MockEventWithMultipleCallsWithCallback()
		{
			var callbackInvokedCount = 0;
			var rock = Rock.Create<IInterfaceMethodVoidWithEventsTests>();
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