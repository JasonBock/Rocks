using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Rocks")]

namespace Rocks
{
	public interface IThing
	{
		void Foo(int x);
		int Bar(Guid a);
	}

	public class Thing
	{
		public Thing(int a) { }
		public Thing(string b, Guid c) { }

		public virtual void Foo(int x) { }
		public virtual int Bar(Guid a) { return a.GetHashCode(); }
	}

	public sealed class HandlerInfo
	{
		private int callCount;
		private uint expectedCount;

		public HandlerInfo(Delegate handler)
			: this(handler, 1)
		{ }

		public HandlerInfo(Delegate handler, uint expectedCount)
		{
			this.Handler = handler;
			this.expectedCount = expectedCount;
		}

		public Delegate Handler { get; private set; }
		public void IncrementCallCount()
		{
			System.Threading.Interlocked.Increment(ref this.callCount);
		}

		public int CallCount
		{
			get { return this.callCount; }
		}
	}

	public sealed class Rock57849
		: Thing
	{
		private ReadOnlyDictionary<string, Delegate> handlers;

		public Rock57849(ReadOnlyDictionary<string, Delegate> handlers, int a)
			: base(a)
		{
			this.handlers = handlers;
		}

		public Rock57849(ReadOnlyDictionary<string, Delegate> handlers, string b, Guid c) 
			: base(b, c)
		{
			this.handlers = handlers;
		}

		public override int Bar(Guid a)
		{
			Delegate handler = null;

			if (this.handlers.TryGetValue("int Bar(Guid)", out handler))
			{
				return (int)handler.DynamicInvoke(a);
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		public override void Foo(int x)
		{
			Delegate handler = null;

			if (this.handlers.TryGetValue("void Foo(Int32)", out handler))
			{
				handler.DynamicInvoke(x);
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}

	public sealed class Rock1234
		: IThing
	{
		private IImmutableDictionary<string, Delegate> handlers;

		internal Rock1234(IImmutableDictionary<string, Delegate> handlers)
		{
			this.handlers = handlers;
		}

		public int Bar(Guid a)
		{
			Delegate handler = null;

			if (this.handlers.TryGetValue("int Bar(Guid)", out handler))
			{
				return (int)handler.DynamicInvoke(a);
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		public void Foo(int x)
		{
			Delegate handler = null;

			if (this.handlers.TryGetValue("void Foo(Int32)", out handler))
			{
				handler.DynamicInvoke(x);
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}
