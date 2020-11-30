using NUnit.Framework;
using System;

namespace Rocks.IntegrationTests
{
	public interface IInterfaceMethodReturnWithEvents
	{
		int NoParameters();
		event EventHandler MyEvent;
	}

	public static class InterfaceMethodReturnWithEventsTests
	{
		[Test]
		public static void MockRaiseEvent()
		{
			var rock = Rock.Create<IInterfaceMethodReturnWithEvents>();
			rock.Methods().NoParameters().RaisesMyEvent(EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			var value = chunk.NoParameters();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(wasEventRaised, Is.True);
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}

		[Test]
		public static void MockRaiseEventWithCallback()
		{
			var wasCallbackInvoked = false;
			var rock = Rock.Create<IInterfaceMethodReturnWithEvents>();
			rock.Methods().NoParameters()
				.Callback(() =>
				{
					wasCallbackInvoked = true;
					return 3;
				})
				.RaisesMyEvent(EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			var value = chunk.NoParameters();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(wasEventRaised, Is.True);
				Assert.That(wasCallbackInvoked, Is.True);
				Assert.That(value, Is.EqualTo(3));
			});
		}

		[Test]
		public static void MockRaiseEventWithMultipleCalls()
		{
			var rock = Rock.Create<IInterfaceMethodReturnWithEvents>();
			rock.Methods().NoParameters()
				.CallCount(2)
				.RaisesMyEvent(EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => eventRaisedCount++;
			var valueOne = chunk.NoParameters();
			var valueTwo = chunk.NoParameters();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(eventRaisedCount, Is.EqualTo(2));
				Assert.That(valueOne, Is.EqualTo(default(int)));
				Assert.That(valueTwo, Is.EqualTo(default(int)));
			});
		}

		[Test]
		public static void MockRaiseEventWithMultipleCallsWithCallback()
		{
			var callbackInvokedCount = 0;
			var rock = Rock.Create<IInterfaceMethodReturnWithEvents>();
			rock.Methods().NoParameters()
				.CallCount(2)
				.Callback(() =>
				{
					callbackInvokedCount++;
					return 3;
				})
				.RaisesMyEvent(EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => eventRaisedCount++;
			var valueOne = chunk.NoParameters();
			var valueTwo = chunk.NoParameters();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(eventRaisedCount, Is.EqualTo(2));
				Assert.That(callbackInvokedCount, Is.EqualTo(2));
				Assert.That(valueOne, Is.EqualTo(3));
				Assert.That(valueTwo, Is.EqualTo(3));
			});
		}
	}
}