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
using R = Rocks;
using RE = Rocks.Exceptions;
using S = System;
using SCG = System.Collections.Generic;
using SCO = System.Collections.ObjectModel;
using SR = System.Reflection;
using STT = System.Threading.Tasks;
a
#pragma warning restore CS8019

namespace h
{
	i
	public unsafe sealed class b
		: c, R.IMock m
	{
		private SCO.ReadOnlyDictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>> handlers;

		k

		g

		d

		e

		f

		SCO.ReadOnlyDictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>> R.IMock.Handlers
		{
			get { return this.handlers; }
		}

		void R.IMock.Raise(string eventName, S.EventArgs args)
		{
			var thisType = this.GetType();

			var eventDelegate = (S.MulticastDelegate)thisType.GetField(eventName, 
				SR.BindingFlags.Instance | SR.BindingFlags.NonPublic).GetValue(this);

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
using R = Rocks;
using RE = Rocks.Exceptions;
using S = System;
using SCG = System.Collections.Generic;
using SCO = System.Collections.ObjectModel;
using SR = System.Reflection;
using STT = System.Threading.Tasks;
a
#pragma warning restore CS8019

namespace h
{
	i
	public  sealed class b
		: c, R.IMock m
	{
		private SCO.ReadOnlyDictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>> handlers;

		k

		g

		d

		e

		f

		SCO.ReadOnlyDictionary<int, SCO.ReadOnlyCollection<R.HandlerInformation>> R.IMock.Handlers
		{
			get { return this.handlers; }
		}

		void R.IMock.Raise(string eventName, S.EventArgs args)
		{
			var thisType = this.GetType();

			var eventDelegate = (S.MulticastDelegate)thisType.GetField(eventName, 
				SR.BindingFlags.Instance | SR.BindingFlags.NonPublic).GetValue(this);

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
