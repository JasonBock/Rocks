using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.AbstractClassMethodReturnWithEventsTestTypes;

public abstract class AbstractClassMethodReturnWithEvents
{
	public abstract int NoParameters();
	public abstract event EventHandler MyEvent;
}

public static class AbstractClassMethodReturnWithEventsTests
{
	[Test]
	public static void CreateRaiseEvent()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassMethodReturnWithEventsCreateExpectations>();
		expectations.Methods.NoParameters().RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock.NoParameters();

		Assert.Multiple(() =>
		{
			Assert.That(wasEventRaised, Is.True);
			Assert.That(value, Is.Default);
		});
	}

	[Test]
	public static void CreateRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassMethodReturnWithEventsCreateExpectations>();
		expectations.Methods.NoParameters()
			.RaiseMyEvent(EventArgs.Empty)
			.Callback(() =>
			{
				wasCallbackInvoked = true;
				return 3;
			});

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock.NoParameters();

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
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassMethodReturnWithEventsCreateExpectations>();
		expectations.Methods.NoParameters()
			.ExpectedCallCount(2)
			.RaiseMyEvent(EventArgs.Empty);

		var eventRaisedCount = 0;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => eventRaisedCount++;
		var valueOne = mock.NoParameters();
		var valueTwo = mock.NoParameters();

		Assert.Multiple(() =>
		{
			Assert.That(eventRaisedCount, Is.EqualTo(2));
			Assert.That(valueOne, Is.Default);
			Assert.That(valueTwo, Is.Default);
		});
	}

	[Test]
	public static void CreateRaiseEventWithMultipleCallsWithCallback()
	{
		var callbackInvokedCount = 0;
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassMethodReturnWithEventsCreateExpectations>();
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

		Assert.Multiple(() =>
		{
			Assert.That(eventRaisedCount, Is.EqualTo(2));
			Assert.That(callbackInvokedCount, Is.EqualTo(2));
			Assert.That(valueOne, Is.EqualTo(3));
			Assert.That(valueTwo, Is.EqualTo(3));
		});
	}
}