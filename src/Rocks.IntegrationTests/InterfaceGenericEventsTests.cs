using NUnit.Framework;

namespace Rocks.IntegrationTests;

public sealed class InterfaceEventArgs
	: EventArgs
{ }

public interface IInterfaceGenericEvents<T>
{
	void Foo();
	event EventHandler<T> MyEvent;
}

public static class InterfaceGenericEventsTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = Rock.Create<IInterfaceGenericEvents<InterfaceEventArgs>>();
		expectations.Methods().Foo().RaisesMyEvent(new InterfaceEventArgs());

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock.Foo();

		expectations.Verify();

		Assert.That(wasEventRaised, Is.True);
	}
}