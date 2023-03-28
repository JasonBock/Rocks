using NUnit.Framework;

namespace Rocks.IntegrationTests;

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
		var expectations = Rock.Create<AbstractClassGenericEvents<AbstractClassEventArgs>>();
		expectations.Methods().Foo().RaisesMyEvent(new AbstractClassEventArgs());

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
		var mock = Rock.Make<AbstractClassGenericEvents<AbstractClassEventArgs>>().Instance();
		mock.Foo();
	}
}