using BenchmarkDotNet.Running;
using System;

namespace Rocks.Performance
{
	class Program
	{
		static void Main()
		{
			var sOpen = typeof(Span<>);
			Console.Out.WriteLine(sOpen.FullName.StartsWith("System.Span`1"));
			var sClosed = typeof(Span<char>);
			Console.Out.WriteLine(sClosed.FullName.StartsWith("System.Span`1"));
			//BenchmarkRunner.Run<RocksPerformanceTests>();
		}
	}
}
