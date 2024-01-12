using NUnit.Framework;

namespace Rocks.IntegrationTests;

public class ClassMethodVoidWithEvents
{
	public virtual void NoParameters() { }

#pragma warning disable CA1070 // Do not declare event fields as virtual
#pragma warning disable CS0067
	public virtual event EventHandler? MyEvent;
#pragma warning restore CS0067
#pragma warning restore CA1070 // Do not declare event fields as virtual
}

public static class ClassMethodVoidWithEventsTests
{
	[Test]
	[RockCreate<ClassMethodVoidWithEvents>]
	public static void CreateEvent()
	{
		var expectations = new ClassMethodVoidWithEventsCreateExpectations();
		expectations.Methods.NoParameters().RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock.NoParameters();

		expectations.Verify();

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	[RockCreate<ClassMethodVoidWithEvents>]
	public static void CreateEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new ClassMethodVoidWithEventsCreateExpectations();
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
	[RockCreate<ClassMethodVoidWithEvents>]
	public static void CreateEventWithMultipleCalls()
	{
		var expectations = new ClassMethodVoidWithEventsCreateExpectations();
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
	[RockCreate<ClassMethodVoidWithEvents>]
	public static void CreateEventWithMultipleCallsWithCallback()
	{
		var callbackInvokedCount = 0;
		var expectations = new ClassMethodVoidWithEventsCreateExpectations();
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