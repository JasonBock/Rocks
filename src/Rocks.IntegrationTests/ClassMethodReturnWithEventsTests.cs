using NUnit.Framework;

namespace Rocks.IntegrationTests;

public class ClassMethodReturnWithEvents
{
	public virtual int NoParameters() => default;

#pragma warning disable CA1070 // Do not declare event fields as virtual
#pragma warning disable CS0067
	public virtual event EventHandler? MyEvent;
#pragma warning restore CS0067
#pragma warning restore CA1070 // Do not declare event fields as virtual
}

public static class ClassMethodReturnWithEventsTests
{
	[Test]
	[RockCreate<ClassMethodReturnWithEvents>]
	public static void CreateRaiseEvent()
	{
		var expectations = new ClassMethodReturnWithEventsCreateExpectations();
		expectations.Methods.NoParameters().RaiseMyEvent(EventArgs.Empty);

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
	[RockCreate<ClassMethodReturnWithEvents>]
	public static void CreateRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		var expectations = new ClassMethodReturnWithEventsCreateExpectations();
		expectations.Methods.NoParameters()
			.Callback(() =>
			{
				wasCallbackInvoked = true;
				return 3;
			})
			.RaiseMyEvent(EventArgs.Empty);

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
	[RockCreate<ClassMethodReturnWithEvents>]
	public static void CreateRaiseEventWithMultipleCalls()
	{
		var expectations = new ClassMethodReturnWithEventsCreateExpectations();
		expectations.Methods.NoParameters()
			.ExpectedCallCount(2)
			.RaiseMyEvent(EventArgs.Empty);

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
	[RockCreate<ClassMethodReturnWithEvents>]
	public static void CreateRaiseEventWithMultipleCallsWithCallback()
	{
		var callbackInvokedCount = 0;
		var expectations = new ClassMethodReturnWithEventsCreateExpectations();
		expectations.Methods.NoParameters()
			.ExpectedCallCount(2)
			.Callback(() =>
			{
				callbackInvokedCount++;
				return 3;
			})
			.RaiseMyEvent(EventArgs.Empty);

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