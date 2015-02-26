using System;
using System.Diagnostics;

namespace Rocks.Sketchpad
{
	public interface IRefsAndOuts
	{
		void Method(int x);
		void RefMethod(ref int x);
		void OutMethod(out int x);
	}

	public static class OutAndRef
	{
		public static void Test()
		{
			foreach(var method in typeof(IRefsAndOuts).GetMethods())
			{
				Console.Out.WriteLine(method.Name);
				
				foreach(var parameter in method.GetParameters())
				{
					Console.Out.WriteLine(parameter.IsOut);
					Console.Out.WriteLine(parameter.ParameterType.IsByRef);
				}
			}
		}
	}
}
