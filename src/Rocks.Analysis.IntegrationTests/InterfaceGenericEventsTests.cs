using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.InterfaceGenericEventsTestTypes;

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
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceGenericEventsCreateExpectations<InterfaceEventArgs>>();
		expectations.Setups.Foo().AddRaiseEvent(new("MyEvent", new InterfaceEventArgs()));

		var wasEventRaised = false;
		var mock = expectations.Instance();
		mock.MyEvent += (s, e) => wasEventRaised = true;
		mock.Foo();

		Assert.That(wasEventRaised, Is.True);
	}
}