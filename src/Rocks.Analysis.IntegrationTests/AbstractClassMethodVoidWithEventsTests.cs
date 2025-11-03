using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.AbstractClassMethodVoidWithEventsTestTypes;

public abstract class AbstractClassMethodVoidWithEvents
{
	public abstract void NoParameters();
	public abstract event EventHandler MyEvent;
}

internal static class AbstractClassMethodVoidWithEventsTests
{
	[Test]
	public static void CreateEvent()
	{
		using var context = new RockContext();
		var expectations = context.Create<AbstractClassMethodVoidWithEventsCreateExpectations>();
		expectations.Setups.NoParameters().RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock.NoParameters();

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	public static void CreateEventWithCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext();
		var expectations = context.Create<AbstractClassMethodVoidWithEventsCreateExpectations>();
		expectations.Setups.NoParameters()
			.Callback(() => wasCallbackInvoked = true)
			.RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock.NoParameters();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		}
	}

	[Test]
	public static void CreateEventWithMultipleCalls()
	{
		using var context = new RockContext();
		var expectations = context.Create<AbstractClassMethodVoidWithEventsCreateExpectations>();
		expectations.Setups.NoParameters()
			.ExpectedCallCount(2)
			.RaiseMyEvent(EventArgs.Empty);

		var eventRaisedCount = 0;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => eventRaisedCount++;
		mock.NoParameters();
		mock.NoParameters();

		Assert.That(eventRaisedCount, Is.EqualTo(2));
	}

	[Test]
	public static void CreateEventWithMultipleCallsWithCallback()
	{
		var callbackInvokedCount = 0;
		using var context = new RockContext();
		var expectations = context.Create<AbstractClassMethodVoidWithEventsCreateExpectations>();
		expectations.Setups.NoParameters()
			.ExpectedCallCount(2)
			.Callback(() => callbackInvokedCount++)
			.RaiseMyEvent(EventArgs.Empty);

		var eventRaisedCount = 0;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => eventRaisedCount++;
		mock.NoParameters();
		mock.NoParameters();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(eventRaisedCount, Is.EqualTo(2));
			Assert.That(callbackInvokedCount, Is.EqualTo(2));
		}
	}
}