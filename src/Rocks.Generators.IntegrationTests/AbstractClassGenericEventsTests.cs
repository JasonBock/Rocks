using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Rocks.IntegrationTests
{
	public sealed class AbstractClassEventArgs
		: EventArgs
	{ }

	public abstract class AbstractClassGenericEvents<T>
	{
		public abstract void Foo();
		public abstract event EventHandler<T> MyEvent;
	}

	public static class AbstractClassGenericEventsTests
	{
		[Test]
		public static void MockUsingGenericType()
		{
			var returnValue = new List<string>();
			var rock = Rock.Create<AbstractClassGenericEvents<AbstractClassEventArgs>>();
			rock.Methods().Foo().RaisesMyEvent(new AbstractClassEventArgs());

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