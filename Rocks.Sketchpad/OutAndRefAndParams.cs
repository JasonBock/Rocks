using System;
using System.Diagnostics;

namespace Rocks.Sketchpad
{
	public interface IRefsAndOutsAndParams
	{
		void Method(int x);
		void RefMethod(ref int x);
		void OutMethod(out int x);
		void ParamsMethod(int x, params string[] b);
	}

	public static class OutAndRefAndParams
	{
		public static void Test()
		{
			foreach(var method in typeof(IRefsAndOutsAndParams).GetMethods())
			{
				Console.Out.WriteLine(method.Name);
				
				foreach(var parameter in method.GetParameters())
				{
					Console.Out.WriteLine($"Name = {parameter.Name}");
					Console.Out.WriteLine($"{nameof(parameter.IsOut)} = {parameter.IsOut}");
               Console.Out.WriteLine($"{parameter.ParameterType.IsByRef} = {parameter.ParameterType.IsByRef}");
					Console.Out.WriteLine($"Is params = {parameter.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0}");
				}

				Console.Out.WriteLine();
			}
		}

		public class x : IRefsAndOutsAndParams
		{
			public void Method(int x)
			{
				throw new NotImplementedException();
			}

			public void OutMethod(out int x)
			{
				throw new NotImplementedException();
			}

			public void ParamsMethod(int x, params string[] b)
			{
				throw new NotImplementedException();
			}

			public void RefMethod(ref int x)
			{
				throw new NotImplementedException();
			}
		}
	}
}
