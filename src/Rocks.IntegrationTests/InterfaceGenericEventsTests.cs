using NUnit.Framework;

namespace Rocks.IntegrationTests;

public sealed class InterfaceEventArgs
	: EventArgs
{ }

public interface IInterfaceGenericEvents<T>
	where T : EventArgs
{
	void Foo();
	event EventHandler<T> MyEvent;
}

public static class InterfaceGenericEventsTests
{
	[Test]
	[RockCreate<IInterfaceGenericEvents<InterfaceEventArgs>>]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = new IInterfaceGenericEventsOfInterfaceEventArgsCreateExpectations();
		expectations.Methods.Foo().AddRaiseEvent(new("MyEvent", new InterfaceEventArgs()));

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock.Foo();

		expectations.Verify();

		Assert.That(wasEventRaised, Is.True);
	}
}