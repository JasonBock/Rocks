using System;
using System.Diagnostics;

namespace Rocks.Sketchpad
{
	public class DelegateDefinitions
	{
		public delegate void TargetDelegate(int x);
	}

	public static class OutAndRef
	{
		public static void Test()
		{
			var @delegate = new DelegateDefinitions.TargetDelegate(a => a++);
			Console.Out.WriteLine(@delegate.GetType().Name);

			var action = new Action<int, string, Guid>((a, b, c) => { });
			Delegate delegateAction = action;

			var dynamicTime = Stopwatch.StartNew();

			for(var i = 0; i < 400000; i++)
			{
				delegateAction.DynamicInvoke(44, "some data here", Guid.NewGuid());
			}

			Console.Out.WriteLine(dynamicTime.Elapsed);

			var actionTime = Stopwatch.StartNew();

			for (var i = 0; i < 400000; i++)
			{
				(delegateAction as Action<int, string, Guid>)(44, "some data here", Guid.NewGuid());
			}

			Console.Out.WriteLine(actionTime.Elapsed);
		}
	}
}
