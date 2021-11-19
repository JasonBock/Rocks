using NUnit.Framework;

namespace Rocks.IntegrationTests;

public sealed class AbstractClassEventArgs
	: EventArgs
{ }

public abstract class AbstractClassGenericEvents<T>
{
	public abstract void Foo();
#pragma warning disable CA1003 // Use generic event handler instances
	public abstract event EventHandler<T> MyEvent;
#pragma warning restore CA1003 // Use generic event handler instances
}

public static class AbstractClassGenericEventsTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var rock = Rock.Create<AbstractClassGenericEvents<AbstractClassEventArgs>>();
		rock.Methods().Foo().RaisesMyEvent(new AbstractClassEventArgs());

		var wasEventRaised = false;
		var chunk = rock.Instance();
		chunk.MyEvent += (s, e) => wasEventRaised = true;
		chunk.Foo();

		rock.Verify();

		Assert.That(wasEventRaised, Is.True);
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var chunk = Rock.Make<AbstractClassGenericEvents<AbstractClassEventArgs>>().Instance();
		chunk.Foo();
	}
}