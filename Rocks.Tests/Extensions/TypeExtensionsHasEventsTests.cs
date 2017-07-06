using NUnit.Framework;
using Rocks.Extensions;
using System;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsHasEventsTests
	{
		[Test]
		public void HasEventsWhenTypeIsInterfaceAndHasEvents() =>
			Assert.That(typeof(IHasEvents).HasEvents(), Is.True);

		[Test]
		public void HasEventsWhenTypeIsInterfaceAndHasNoEvents() =>
			Assert.That(typeof(IHasNoEvents).HasEvents(), Is.False);

		[Test]
		public void HasEventsWhenTypeIsClassAndHasEvents() =>
			Assert.That(typeof(HasEvents).HasEvents(), Is.True);

		[Test]
		public void HasEventsWhenTypeIsClassAndHasNoEvents() =>
			Assert.That(typeof(HasNoEvents).HasEvents(), Is.False);

		[Test]
		public void HasEventsWhenTypeIsSubInterfaceAndHasEvents() =>
			Assert.That(typeof(IHasSubEvents).HasEvents(), Is.True);

		[Test]
		public void HasEventsWhenTypeIsSubInterfaceAndHasNoEvents() =>
			Assert.That(typeof(IHasSubNoEvents).HasEvents(), Is.False);

		[Test]
		public void HasEventsWhenTypeIsSubClassAndHasEvents() =>
			Assert.That(typeof(HasSubEvents).HasEvents(), Is.True);

		[Test]
		public void HasEventsWhenTypeIsSubClassAndHasNoEvents() =>
			Assert.That(typeof(HasSubNoEvents).HasEvents(), Is.False);

		[Test]
		public void HasEventsWhenEventIsProtected() =>
			Assert.That(typeof(HasProtectedEvents).HasEvents(), Is.True);
	}

#pragma warning disable CS0067
	public interface IHasEvents
	{
		event EventHandler AnEvent;
	}

	public interface IHasNoEvents { }

	public interface IHasSubEvents : IHasEvents { }

	public interface IHasSubNoEvents : IHasNoEvents { }

	public class HasEvents
	{
		public event EventHandler AnEvent;
	}

	public class HasNoEvents { }

	public class HasSubEvents : HasEvents { }

	public class HasSubNoEvents : HasNoEvents { }

	public class HasProtectedEvents
	{
		protected event EventHandler AnEvent;
	}
#pragma warning restore CS0067
}
