using System;

namespace Rocks.Sketchpad
{
	public class ActionForThis
	{
		public void Callback(Action<ActionForThis, Action<int, string>> callback) => callback(this, null!);
	}

	public class Caller
	{
		public void Foo()
		{
			//new ActionForThis().Callback(@this, (a, b) => { });
		}
	}
}
