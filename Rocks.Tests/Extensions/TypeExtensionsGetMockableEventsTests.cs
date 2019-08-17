using NUnit.Framework;
using Rocks.Construction.InMemory;
using System;
using System.Linq;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	public static class TypeExtensionsGetMockableEventsTests
	{
		[Test]
		public static void GetMockableEventsFromBaseInterface()
		{
			var events = typeof(IMockableEventsBase).GetMockableEvents(new InMemoryNameGenerator());
			Assert.That(events.Count, Is.EqualTo(1));
			Assert.That(events.Where(_ => _.Name == nameof(IMockableEventsBase.BaseInterfaceEvent)).Any(), Is.True);
		}

		[Test]
		public static void GetMockableEventsFromSubInterface()
		{
			var events = typeof(IMockableEventsSub).GetMockableEvents(new InMemoryNameGenerator());
			Assert.That(events.Count, Is.EqualTo(2));
			Assert.That(events.Where(_ => _.Name == nameof(IMockableEventsBase.BaseInterfaceEvent)).Any(), Is.True);
			Assert.That(events.Where(_ => _.Name == nameof(IMockableEventsSub.SubInterfaceEvent)).Any(), Is.True);
		}

		[Test]
		public static void GetMockableEventsFromAbstractClass()
		{
			var events = typeof(MockableEventsAbstract).GetMockableEvents(new InMemoryNameGenerator());
			Assert.That(events.Count, Is.EqualTo(1));
			Assert.That(events.Where(_ => _.Name == "AbstractClassEvent").Any(), Is.True);
		}

		[Test]
		public static void GetMockableEventsFromSubClassFromAbstract()
		{
			var events = typeof(MockableEventsSubFromAbstract).GetMockableEvents(new InMemoryNameGenerator());
			Assert.That(events.Count, Is.EqualTo(1));
			Assert.That(events.Where(_ => _.Name == "AbstractClassEvent").Any(), Is.True);
		}

		[Test]
		public static void GetMockableEventsFromBaseClass()
		{
			var events = typeof(MockableEventsBase).GetMockableEvents(new InMemoryNameGenerator());
			Assert.That(events.Count, Is.EqualTo(1));
			Assert.That(events.Where(_ => _.Name == nameof(MockableEventsBase.BaseVirtualClassEvent)).Any(), Is.True);
		}

		[Test]
		public static void GetMockableEventsFromSubClass()
		{
			var events = typeof(MockableEventsSub).GetMockableEvents(new InMemoryNameGenerator());
			Assert.That(events.Count, Is.EqualTo(1));
			Assert.That(events.Where(_ => _.Name == nameof(MockableEventsBase.BaseVirtualClassEvent)).Any(), Is.True);
		}
	}

	public interface IMockableEventsBase
	{
		event EventHandler BaseInterfaceEvent;
	}

	public interface IMockableEventsSub
		: IMockableEventsBase
	{
		event EventHandler SubInterfaceEvent;
	}

	public abstract class MockableEventsAbstract
	{
		protected abstract event EventHandler AbstractClassEvent;
	}

#pragma warning disable 67
	public class MockableEventsSubFromAbstract
		: MockableEventsAbstract
	{
		protected override event EventHandler? AbstractClassEvent;
	}

	public class MockableEventsBase
	{
		public event EventHandler? BaseClassEvent;
		public virtual event EventHandler? BaseVirtualClassEvent;
	}

	public class MockableEventsSub
		: MockableEventsBase, IMockableEventsSub
	{
		public event EventHandler? BaseInterfaceEvent;
		public event EventHandler? SubInterfaceEvent;
	}
#pragma warning restore 67
}
