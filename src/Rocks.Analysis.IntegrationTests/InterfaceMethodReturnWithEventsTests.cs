using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.InterfaceMethodReturnWithEventsTestTypes;

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
		using var context = new RockContext();
		var expectations = context.Create<IInterfaceMethodReturnWithEventsCreateExpectations>();
		expectations.Setups.NoParameters()
			.RaiseMyEvent(EventArgs.Empty);

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		var value = mock.NoParameters();

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(wasEventRaised, Is.True);
			Assert.That(value, Is.Default);
		}
	}

	[Test]
	public static void CreateRaiseEventWithCallback()
	{
		var wasCallbackInvoked = false;
		using var context = new RockContext();
		var expectations = context.Create<IInterfaceMethodReturnWithEventsCreateExpectations>();
		expectations.Setups.NoParameters()
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

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(wasEventRaised, Is.True);
			Assert.That(wasCallbackInvoked, Is.True);
			Assert.That(value, Is.EqualTo(3));
		}
	}

	[Test]
	public static void CreateRaiseEventWithMultipleCalls()
	{
		using var context = new RockContext();
		var expectations = context.Create<IInterfaceMethodReturnWithEventsCreateExpectations>();
		expectations.Setups.NoParameters()
			.ExpectedCallCount(2)
			.RaiseMyEvent(EventArgs.Empty);

		var eventRaisedCount = 0;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => eventRaisedCount++;
		var valueOne = mock.NoParameters();
		var valueTwo = mock.NoParameters();

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(eventRaisedCount, Is.EqualTo(2));
			Assert.That(valueOne, Is.Default);
			Assert.That(valueTwo, Is.Default);
		}
	}

	[Test]
	public static void CreateRaiseEventWithMultipleCallsWithCallback()
	{
		var callbackInvokedCount = 0;
		using var context = new RockContext();
		var expectations = context.Create<IInterfaceMethodReturnWithEventsCreateExpectations>();
		expectations.Setups.NoParameters()
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

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(eventRaisedCount, Is.EqualTo(2));
			Assert.That(callbackInvokedCount, Is.EqualTo(2));
			Assert.That(valueOne, Is.EqualTo(3));
			Assert.That(valueTwo, Is.EqualTo(3));
		}
	}
}