using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Rocks.IntegrationTests
{
	public sealed class MyEventArgs
		: EventArgs
	{ }

	public interface IGenericEvents<T>
	{
		void Foo();
		event EventHandler<T> MyEvent;
	}

	public static class InterfaceGenericEventsTests
	{
		[Test]
		public static void MockUsingGenericType()
		{
			var returnValue = new List<string>();
			var rock = Rock.Create<IGenericEvents<MyEventArgs>>();
			rock.Methods().Foo().RaisesMyEvent(new MyEventArgs());

			var wasEventRaised = false;
			var chunk = rock.Instance();
			chunk.MyEvent += (s, e) => wasEventRaised = true;
			chunk.Foo();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(wasEventRaised, Is.True);
			});
		}
	}
}