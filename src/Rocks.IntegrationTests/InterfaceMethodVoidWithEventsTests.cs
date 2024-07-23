using NUnit.Framework;

namespace Rocks.IntegrationTests.InterfaceMethodVoidWithEventsTestTypes;

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
		var expectations = new IInterfaceMethodVoidWithEventsCreateExpectations();
		expectations.Methods.NoParameters()
			.RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock.NoParameters();

		expectations.Verify();

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	public static void CreateEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new IInterfaceMethodVoidWithEventsCreateExpectations();
		expectations.Methods.NoParameters()
			.Callback(() => wasCallbackInvoked = true)
			.RaiseMyEvent(EventArgs.Empty);

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
	public static void CreateEventWithMultipleCalls()
	{
		var expectations = new IInterfaceMethodVoidWithEventsCreateExpectations();
		expectations.Methods.NoParameters()
			.ExpectedCallCount(2)
			.RaiseMyEvent(EventArgs.Empty);

		var eventRaisedCount = 0;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => eventRaisedCount++;
		mock.NoParameters();
		mock.NoParameters();

		expectations.Verify();

		Assert.That(eventRaisedCount, Is.EqualTo(2));
	}

	[Test]
	public static void CreateEventWithMultipleCallsWithCallback()
	{
		var callbackInvokedCount = 0;
		var expectations = new IInterfaceMethodVoidWithEventsCreateExpectations();
		expectations.Methods.NoParameters()
			.ExpectedCallCount(2)
			.Callback(() => callbackInvokedCount++)
			.RaiseMyEvent(EventArgs.Empty);

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