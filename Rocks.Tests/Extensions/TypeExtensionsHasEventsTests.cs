using NUnit.Framework;
using Rocks.Extensions;
using System;

namespace Rocks.Tests.Extensions
{
	public static class TypeExtensionsHasEventsTests
	{
		[Test]
		public static void HasEventsWhenTypeIsInterfaceAndHasEvents() =>
			Assert.That(typeof(IHasEvents).HasEvents(), Is.True);

		[Test]
		public static void HasEventsWhenTypeIsInterfaceAndHasNoEvents() =>
			Assert.That(typeof(IHasNoEvents).HasEvents(), Is.False);

		[Test]
		public static void HasEventsWhenTypeIsClassAndHasEvents() =>
			Assert.That(typeof(HasEvents).HasEvents(), Is.True);

		[Test]
		public static void HasEventsWhenTypeIsClassAndHasNoEvents() =>
			Assert.That(typeof(HasNoEvents).HasEvents(), Is.False);

		[Test]
		public static void HasEventsWhenTypeIsSubInterfaceAndHasEvents() =>
			Assert.That(typeof(IHasSubEvents).HasEvents(), Is.True);

		[Test]
		public static void HasEventsWhenTypeIsSubInterfaceAndHasNoEvents() =>
			Assert.That(typeof(IHasSubNoEvents).HasEvents(), Is.False);

		[Test]
		public static void HasEventsWhenTypeIsSubClassAndHasEvents() =>
			Assert.That(typeof(HasSubEvents).HasEvents(), Is.True);

		[Test]
		public static void HasEventsWhenTypeIsSubClassAndHasNoEvents() =>
			Assert.That(typeof(HasSubNoEvents).HasEvents(), Is.False);

		[Test]
		public static void HasEventsWhenEventIsProtected() =>
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
