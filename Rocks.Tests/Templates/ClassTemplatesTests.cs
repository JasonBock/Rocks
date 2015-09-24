using NUnit.Framework;
using Rocks.Templates;

namespace Rocks.Tests.Templates
{
	[TestFixture]
	public sealed class ClassTemplatesTests
	{
		[Test]
		public void GetClassWithObsoleteSuppression()
		{
			Assert.AreEqual(
@"#pragma warning disable CS0618
#pragma warning disable CS0672
a
#pragma warning restore CS0672
#pragma warning restore CS0618", ClassTemplates.GetClassWithObsoleteSuppression("a"));
		}

		[Test]
		public void GetClassTemplateWhenIsUnsafeIsTrue()
		{
			Assert.AreEqual(
@"#pragma warning disable CS8019
a
#pragma warning restore CS8019

namespace h
{
	i
	public unsafe sealed class b
		: c, Rocks.IMock m
	{
		private ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers;

		k

		g

		d

		e

		f

		ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> Rocks.IMock.Handlers
		{
			get { return this.handlers; }
		}

		void Rocks.IMock.Raise(string eventName, EventArgs args)
		{
			var thisType = this.GetType();

			var eventDelegate = (MulticastDelegate)thisType.GetField(eventName, 
				BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);

			if (eventDelegate != null)
			{
				foreach (var handler in eventDelegate.GetInvocationList())
				{
					handler.Method.Invoke(handler.Target, new object[] { this, args });
				}
			}
		}

		l
	}
}", ClassTemplates.GetClass("a", "b", "c", "d", "e", "f", "g", "h", "i", "k", "l", true, "m"));
		}

		[Test]
		public void GetClassTemplateWhenIsUnsafeIsFalse()
		{
			Assert.AreEqual(
@"#pragma warning disable CS8019
a
#pragma warning restore CS8019

namespace h
{
	i
	public  sealed class b
		: c, Rocks.IMock m
	{
		private ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers;

		k

		g

		d

		e

		f

		ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> Rocks.IMock.Handlers
		{
			get { return this.handlers; }
		}

		void Rocks.IMock.Raise(string eventName, EventArgs args)
		{
			var thisType = this.GetType();

			var eventDelegate = (MulticastDelegate)thisType.GetField(eventName, 
				BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);

			if (eventDelegate != null)
			{
				foreach (var handler in eventDelegate.GetInvocationList())
				{
					handler.Method.Invoke(handler.Target, new object[] { this, args });
				}
			}
		}

		l
	}
}", ClassTemplates.GetClass("a", "b", "c", "d", "e", "f", "g", "h", "i", "k", "l", false, "m"));
		}
	}
}
