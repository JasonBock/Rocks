using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class EventsTests
	{
		[Test]
		public void CreateWhenTargetDoesNotHaveEvents()
		{
			var rock = Rock.Create<IDoNotHaveEvents>();
			var chunk = rock.Make();
			Assert.That(chunk, Is.AssignableTo<IMock>());
		}

		[Test]
		public void CreateWhenTargetHasEvents()
		{
			var rock = Rock.Create<IHaveEvents>();
			var chunk = rock.Make();
			Assert.That(chunk, Is.AssignableTo<IMockWithEvents>());
		}

		[Test]
		public void CreateWhenEventsHaveGenericEventArgs()
		{
			var rock = Rock.Create<IHaveGenericEventArguments<ITarget>>();
			var chunk = rock.Make();
			Assert.That(chunk, Is.Not.Null);
		}
	}

	public class GenericEventArgs<T> : EventArgs { }

	public interface IDoNotHaveEvents
	{
		void DoSomething();
	}

	public interface IHaveEvents
	{
		event EventHandler AnEvent;
		void DoSomething();
	}

	public interface IHaveGenericEventArguments<T>
	{
		event EventHandler<GenericEventArgs<T>> AnEvent;
	}

	public interface ITarget { }
}
