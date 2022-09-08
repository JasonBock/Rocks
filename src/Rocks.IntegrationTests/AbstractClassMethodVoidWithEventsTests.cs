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
	public static void CreateEvent()
	{
		var expectations = Rock.Create<AbstractClassMethodVoidWithEvents>();
		expectations.Methods().NoParameters().RaisesMyEvent(EventArgs.Empty);

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
		var expectations = Rock.Create<AbstractClassMethodVoidWithEvents>();
		expectations.Methods().NoParameters()
			.Callback(() => wasCallbackInvoked = true)
			.RaisesMyEvent(EventArgs.Empty);

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
		var expectations = Rock.Create<AbstractClassMethodVoidWithEvents>();
		expectations.Methods().NoParameters()
			.CallCount(2)
			.RaisesMyEvent(EventArgs.Empty);

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
		var expectations = Rock.Create<AbstractClassMethodVoidWithEvents>();
		expectations.Methods().NoParameters()
			.CallCount(2)
			.Callback(() => callbackInvokedCount++)
			.RaisesMyEvent(EventArgs.Empty);

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