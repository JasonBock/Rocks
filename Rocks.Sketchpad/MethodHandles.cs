using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocks.Sketchpad
{
	public static class MethodHandles
	{
		public static void Test()
		{
			var type = typeof(Methods);

			foreach (var method in type.GetMethods())
			{
				Console.Out.WriteLine($"{method} {method.MethodHandle.Value.ToInt32()}");
			}
		}
	}

	public class Methods
	{
		public void Method1() { }
		public void Method1(int a) { }
		public void Method1<T>(T a) { }
	}
}
