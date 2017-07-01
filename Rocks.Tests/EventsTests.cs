using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class EventsTests
	{
		[Test]
		public void CreateWhenEventsHaveGenericEventArgs()
		{
			var rock = Rock.Create<IHaveGenericEventArguments<ITarget>>();
			var chunk = rock.Make();
			Assert.That(chunk, Is.Not.Null);
		}
	}

	public class GenericEventArgs<T> : EventArgs { }

	public interface IHaveGenericEventArguments<T>
	{
		event EventHandler<GenericEventArgs<T>> AnEvent;
	}

	public interface ITarget { }
}
