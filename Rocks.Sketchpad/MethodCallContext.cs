using System;
using System.Runtime.Remoting.Messaging;

namespace Rocks.Sketchpad
{
	public static class MethodCallContext
	{
		public static void Test()
		{
			var outer = new OuterClass();
			outer.OuterMethod();
		}
	}

	public class ThisContext
		: IDisposable
	{
		private static readonly string Key = Guid.NewGuid().ToString();
		private readonly object oldThis;

		public ThisContext(object @this)
		{
			this.oldThis = CallContext.GetData(Key);
			CallContext.SetData(ThisContext.Key, @this);
		}

		public void Dispose()
		{
			if (this.oldThis != null)
			{
				CallContext.SetData(ThisContext.Key, this.oldThis);
			}
			else
			{
				CallContext.FreeNamedDataSlot(ThisContext.Key);
			}
		}

		public static object GetThis() => CallContext.GetData(ThisContext.Key);
	}

	public class OuterClass
	{
		public void OuterMethod()
		{
			using (var manager = new ThisContext(this))
			{
				Console.Out.WriteLine(ThisContext.GetThis().GetType().Name);
				var inner = new InnerClass();
				inner.InnerMethod();
				Console.Out.WriteLine(ThisContext.GetThis().GetType().Name);
			}
		}
	}

	public class InnerClass
	{
		public void InnerMethod()
		{
			using (var manager = new ThisContext(this))
			{
				Console.Out.WriteLine(ThisContext.GetThis().GetType().Name);
			}
		}
	}
}
