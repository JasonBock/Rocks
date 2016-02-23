using NUnit.Framework;
using Rocks.Construction.InMemory;
using System;
using System.Linq;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsGetMockableEventsTests
	{
		[Test]
		public void GetMockableEventsFromBaseInterface()
		{
			var events = typeof(IMockableEventsBase).GetMockableEvents(new InMemoryNameGenerator());
			Assert.AreEqual(1, events.Count);
			Assert.IsTrue(events.Where(_ => _.Name == nameof(IMockableEventsBase.BaseInterfaceEvent)).Any());
		}

		[Test]
		public void GetMockableEventsFromSubInterface()
		{
			var events = typeof(IMockableEventsSub).GetMockableEvents(new InMemoryNameGenerator());
			Assert.AreEqual(2, events.Count);
			Assert.IsTrue(events.Where(_ => _.Name == nameof(IMockableEventsBase.BaseInterfaceEvent)).Any());
			Assert.IsTrue(events.Where(_ => _.Name == nameof(IMockableEventsSub.SubInterfaceEvent)).Any());
		}

		[Test]
		public void GetMockableEventsFromAbstractClass()
		{
			var events = typeof(MockableEventsAbstract).GetMockableEvents(new InMemoryNameGenerator());
			Assert.AreEqual(1, events.Count);
			Assert.IsTrue(events.Where(_ => _.Name == "AbstractClassEvent").Any());
		}

		[Test]
		public void GetMockableEventsFromSubClassFromAbstract()
		{
			var events = typeof(MockableEventsSubFromAbstract).GetMockableEvents(new InMemoryNameGenerator());
			Assert.AreEqual(1, events.Count);
			Assert.IsTrue(events.Where(_ => _.Name == "AbstractClassEvent").Any());
		}

		[Test]
		public void GetMockableEventsFromBaseClass()
		{
			var events = typeof(MockableEventsBase).GetMockableEvents(new InMemoryNameGenerator());
			Assert.AreEqual(1, events.Count);
			Assert.IsTrue(events.Where(_ => _.Name == nameof(MockableEventsBase.BaseVirtualClassEvent)).Any());
		}

		[Test]
		public void GetMockableEventsFromSubClass()
		{
			var events = typeof(MockableEventsSub).GetMockableEvents(new InMemoryNameGenerator());
			Assert.AreEqual(1, events.Count);
			Assert.IsTrue(events.Where(_ => _.Name == nameof(MockableEventsBase.BaseVirtualClassEvent)).Any());
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
		protected override event EventHandler AbstractClassEvent;
	}

	public class MockableEventsBase
	{
		public event EventHandler BaseClassEvent;
		public virtual event EventHandler BaseVirtualClassEvent;
	}

	public class MockableEventsSub
		: MockableEventsBase, IMockableEventsSub
	{
		public event EventHandler BaseInterfaceEvent;
		public event EventHandler SubInterfaceEvent;
	}
#pragma warning restore 67
}
