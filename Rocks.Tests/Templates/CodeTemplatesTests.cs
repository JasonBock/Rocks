using NUnit.Framework;
using Rocks.Templates;

namespace Rocks.Tests.Templates
{
	[TestFixture]
	public sealed class CodeTemplatesTests
	{
		[Test]
		public void GetExpectation()
		{
			Assert.AreEqual("(methodHandler.Expectations[\"a\"] as ArgumentExpectation<b>).IsValid(a, \"a\")", CodeTemplates.GetExpectation("a", "b"));
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
		: c, IMock m
	{
		private ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers;

		k

		g

		d

		e

#pragma warning disable CS0067
		f
#pragma warning restore CS0067

		ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> IMock.Handlers
		{
			get { return this.handlers; }
		}

		void IMock.Raise(string eventName, EventArgs args)
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
}", CodeTemplates.GetClass("a", "b", "c", "d", "e", "f", "g", "h", "i", "k", "l", true, "m"));
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
		: c, IMock m
	{
		private ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> handlers;

		k

		g

		d

		e

#pragma warning disable CS0067
		f
#pragma warning restore CS0067

		ReadOnlyDictionary<int, ReadOnlyCollection<HandlerInformation>> IMock.Handlers
		{
			get { return this.handlers; }
		}

		void IMock.Raise(string eventName, EventArgs args)
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
}", CodeTemplates.GetClass("a", "b", "c", "d", "e", "f", "g", "h", "i", "k", "l", false, "m"));
		}
	}
}
