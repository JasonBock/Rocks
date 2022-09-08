using NUnit.Framework;

namespace Rocks.IntegrationTests;

public interface IInterfaceMethodReturnWithEvents
{
	int NoParameters();
	event EventHandler MyEvent;
}

public static class InterfaceMethodReturnWithEventsTests
{
	[Test]
	public static void CreateRaiseEvent()
	{
		var expectations = Rock.Create<IInterfaceMethodReturnWithEvents>();
		expectations.Methods().NoParameters().RaisesMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock.NoParameters();

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(wasEventRaised, Is.True);
			Assert.That(value, Is.EqualTo(default(int)));
		});
	}

	[Test]
	public static void CreateRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = Rock.Create<IInterfaceMethodReturnWithEvents>();
		expectations.Methods().NoParameters()
			.Callback(() =>
			{
				wasCallbackInvoked = true;
				return 3;
			})
			.RaisesMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock.NoParameters();

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
			Assert.That(value, Is.EqualTo(3));
		});
	}

	[Test]
	public static void CreateRaiseEventWithMultipleCalls()
	{
		var expectations = Rock.Create<IInterfaceMethodReturnWithEvents>();
		expectations.Methods().NoParameters()
			.CallCount(2)
			.RaisesMyEvent(EventArgs.Empty);

		var eventRaisedCount = 0;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => eventRaisedCount++;
		var valueOne = mock.NoParameters();
		var valueTwo = mock.NoParameters();

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(eventRaisedCount, Is.EqualTo(2));
			Assert.That(valueOne, Is.EqualTo(default(int)));
			Assert.That(valueTwo, Is.EqualTo(default(int)));
		});
	}

	[Test]
	public static void CreateRaiseEventWithMultipleCallsWithCallback()
	{
		var callbackInvokedCount = 0;
		var expectations = Rock.Create<IInterfaceMethodReturnWithEvents>();
		expectations.Methods().NoParameters()
			.CallCount(2)
			.Callback(() =>
			{
				callbackInvokedCount++;
				return 3;
			})
			.RaisesMyEvent(EventArgs.Empty);

		var eventRaisedCount = 0;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => eventRaisedCount++;
		var valueOne = mock.NoParameters();
		var valueTwo = mock.NoParameters();

		expectations.Verify();

		Assert.Multiple(() =>
		{
			Assert.That(eventRaisedCount, Is.EqualTo(2));
			Assert.That(callbackInvokedCount, Is.EqualTo(2));
			Assert.That(valueOne, Is.EqualTo(3));
			Assert.That(valueTwo, Is.EqualTo(3));
		});
	}
}