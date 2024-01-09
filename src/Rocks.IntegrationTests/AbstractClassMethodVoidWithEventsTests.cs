using NUnit.Framework;

namespace Rocks.IntegrationTests;

public abstract class AbstractClassMethodVoidWithEvents
{
	public abstract void NoParameters();
	public abstract event EventHandler MyEvent;
}

public static class AbstractClassMethodVoidWithEventsTests
{
	[Test]
	[RockCreate<AbstractClassMethodVoidWithEvents>]
	public static void CreateEvent()
	{
		var expectations = new AbstractClassMethodVoidWithEventsCreateExpectations();
		expectations.Methods.NoParameters().AddRaiseEvent(new(nameof(AbstractClassMethodVoidWithEvents.MyEvent), EventArgs.Empty));

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock.NoParameters();

		expectations.Verify();

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	[RockCreate<AbstractClassMethodVoidWithEvents>]
	public static void CreateEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new AbstractClassMethodVoidWithEventsCreateExpectations();
		expectations.Methods.NoParameters()
			.Callback(() => wasCallbackInvoked = true)
			.AddRaiseEvent(new(nameof(AbstractClassMethodVoidWithEvents.MyEvent), EventArgs.Empty));

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock.NoParameters();

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
		});
	}

	[Test]
	[RockCreate<AbstractClassMethodVoidWithEvents>]
	public static void CreateEventWithMultipleCalls()
	{
		var expectations = new AbstractClassMethodVoidWithEventsCreateExpectations();
		expectations.Methods.NoParameters()
			.ExpectedCallCount(2)
			.AddRaiseEvent(new(nameof(AbstractClassMethodVoidWithEvents.MyEvent), EventArgs.Empty));

		var eventRaisedCount = 0;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => eventRaisedCount++;
		mock.NoParameters();
		mock.NoParameters();

		expectations.Verify();

		Assert.That(eventRaisedCount, Is.EqualTo(2));
	}

	[Test]
	[RockCreate<AbstractClassMethodVoidWithEvents>]
	public static void CreateEventWithMultipleCallsWithCallback()
	{
		var callbackInvokedCount = 0;
		var expectations = new AbstractClassMethodVoidWithEventsCreateExpectations();
		expectations.Methods.NoParameters()
			.ExpectedCallCount(2)
			.Callback(() => callbackInvokedCount++)
			.AddRaiseEvent(new(nameof(AbstractClassMethodVoidWithEvents.MyEvent), EventArgs.Empty));

		var eventRaisedCount = 0;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => eventRaisedCount++;
		mock.NoParameters();
		mock.NoParameters();

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(eventRaisedCount, Is.EqualTo(2));
			Assert.That(callbackInvokedCount, Is.EqualTo(2));
		});
	}
}