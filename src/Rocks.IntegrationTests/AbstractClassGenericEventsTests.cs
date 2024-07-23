using NUnit.Framework;

namespace Rocks.IntegrationTests.AbstractClassGenericEventsTestTypes;

public sealed class AbstractClassEventArgs
	: EventArgs
{ }

public abstract class AbstractClassGenericEvents<T>
	where T : EventArgs
{
	public abstract void Foo();

   public abstract event EventHandler<T> MyEvent;
}

public static class AbstractClassGenericEventsTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = new AbstractClassGenericEventsCreateExpectations<AbstractClassEventArgs>();
		expectations.Methods.Foo().AddRaiseEvent(new("MyEvent", new AbstractClassEventArgs()));

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock.Foo();

		expectations.Verify();

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = new AbstractClassGenericEventsMakeExpectations<AbstractClassEventArgs>().Instance();
		mock.Foo();
	}
}