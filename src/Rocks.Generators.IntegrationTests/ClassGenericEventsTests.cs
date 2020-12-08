using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Rocks.IntegrationTests
{
	public sealed class ClassEventArgs
		: EventArgs
	{ }

	public class ClassGenericEvents<T>
	{
		public virtual void Foo() { }
#pragma warning disable CA1070 // Do not declare event fields as virtual
#pragma warning disable CS0067 
		public virtual event EventHandler<T>? MyEvent;
#pragma warning restore CS0067 
#pragma warning restore CA1070 // Do not declare event fields as virtual
	}

	public static class ClassGenericEventsTests
	{
		[Test]
		public static void CreateUsingGenericType()
		{
			var returnValue = new List<string>();
			var rock = Rock.Create<ClassGenericEvents<ClassEventArgs>>();
			rock.Methods().Foo().RaisesMyEvent(new ClassEventArgs());

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