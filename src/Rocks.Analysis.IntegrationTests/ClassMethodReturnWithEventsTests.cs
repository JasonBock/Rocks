using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.ClassMethodReturnWithEventsTestTypes;

public class ClassMethodReturnWithEvents
{
	public virtual int NoParameters() => default;

#pragma warning disable CA1070 // Do not declare event fields as virtual
#pragma warning disable CS0067
	public virtual event EventHandler? MyEvent;
#pragma warning restore CS0067
#pragma warning restore CA1070 // Do not declare event fields as virtual
}

internal static class ClassMethodReturnWithEventsTests
{
	[Test]
	public static void CreateRaiseEvent()
	{
		using var context = new RockContext();
		var expectations = context.Create<ClassMethodReturnWithEventsCreateExpectations>();
		expectations.Setups.NoParameters().RaiseMyEvent(EventArgs.Empty);

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
		var expectations = context.Create<ClassMethodReturnWithEventsCreateExpectations>();
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
		var expectations = context.Create<ClassMethodReturnWithEventsCreateExpectations>();
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
		var expectations = context.Create<ClassMethodReturnWithEventsCreateExpectations>();
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