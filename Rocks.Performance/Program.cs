using BenchmarkDotNet.Running;

namespace Rocks.Performance
{
	class Program
	{
		static void Main() => 
			BenchmarkRunner.Run<RocksPerformanceTests>();
	}
}
