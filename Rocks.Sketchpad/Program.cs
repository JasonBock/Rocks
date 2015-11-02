using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace Rocks.Sketchpad
{
	public static class Program
	{
		public static readonly string Key = Guid.NewGuid().ToString();

		static void Main(string[] args)
		{
			var o = new object();
			var timer = Stopwatch.StartNew();

			for(var i = 0; i < 100000; i++)
			{
				CallContext.SetData(Program.Key, o);
				CallContext.GetData(Program.Key);
				CallContext.FreeNamedDataSlot(Program.Key);
			}

			timer.Stop();

			Console.Out.WriteLine(timer.Elapsed);
		}
	}
}
