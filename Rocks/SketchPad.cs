using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Rocks")]

namespace Rocks.SketchPad
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

	internal interface IRock
	{
		ReadOnlyDictionary<string, HandlerInformation> Handlers { get; }
	}

	public interface ISomething
	{
		ReadOnlyDictionary<string, HandlerInformation> Handlers { get; }
	}

	public static class UseRock
	{
		public static void UseIt()
		{
			var r = new Rock57849(new ReadOnlyDictionary<string, Delegate>(new Dictionary<string, Delegate>()), 1);
		}
	}

	public sealed class Rock57849
		: Thing, ISomething, IRock
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

		public ReadOnlyDictionary<string, HandlerInformation> Handlers
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		ReadOnlyDictionary<string, HandlerInformation> IRock.Handlers
		{
			get
			{
				throw new NotImplementedException();
			}
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
