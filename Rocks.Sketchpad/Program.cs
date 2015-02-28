using System;

namespace Rocks.Sketchpad
{
	public static class Program
	{
		static void Main(string[] args)
		{
			var m = new Action<Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32>(
				(a, b, c, d, e, f, g, h, i) => { });

			var q = (m as Action<Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32>);
			q(1, 1, 1, 1, 1, 1, 1, 1, 1);
		}

		public static void Target(int a, int b, int c, int d, int e, int f, int g, int h, int i) { }
   }
}
