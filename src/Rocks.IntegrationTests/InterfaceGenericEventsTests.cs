using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Rocks.IntegrationTests
{
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
			var rock = Rock.Create<IInterfaceGenericEvents<InterfaceEventArgs>>();
			rock.Methods().Foo().RaisesMyEvent(new InterfaceEventArgs());

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			chunk.Foo();

			rock.Verify();

			Assert.That(wasEventRaised, Is.True);
		}
	}
}