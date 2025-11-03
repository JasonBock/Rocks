using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.AbstractClassGenericEventsTestTypes;

public sealed class AbstractClassEventArgs
	: EventArgs
{ }

public abstract class AbstractClassGenericEvents<T>
	where T : EventArgs
{
	public abstract void Foo();

	public abstract event EventHandler<T> MyEvent;
}

internal static class AbstractClassGenericEventsTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		using var context = new RockContext();
		var expectations = context.Create<AbstractClassGenericEventsCreateExpectations<AbstractClassEventArgs>>();
		expectations.Setups.Foo().AddRaiseEvent(new("MyEvent", new AbstractClassEventArgs()));

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock.Foo();

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = new AbstractClassGenericEventsMakeExpectations<AbstractClassEventArgs>().Instance();
		mock.Foo();
	}
}