using NUnit.Framework;

namespace Rocks.IntegrationTests;

public sealed class InterfaceEventArgs
	: EventArgs
{ }

public interface IInterfaceGenericEvents<T>
{
	void Foo();
#pragma warning disable CA1003 // Use generic event handler instances
	event EventHandler<T> MyEvent;
#pragma warning restore CA1003 // Use generic event handler instances
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