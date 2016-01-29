using System;

namespace Rocks.Sketchpad
{
	public static class Program
	{
		public static readonly string Key = Guid.NewGuid().ToString();

		static void Main(string[] args)
		{
			MethodCallContext.Test();
		}
	}
}
