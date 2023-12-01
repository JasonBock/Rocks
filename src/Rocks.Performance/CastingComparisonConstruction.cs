using BenchmarkDotNet.Attributes;
using Rocks.Expectations;

namespace Rocks.Performance;

[MemoryDiagnoser]
public class CastingComparisonConstruction
{
	[Benchmark(Baseline = true)]
#pragma warning disable CA1822 // Mark members as static
   public ISimple ConstructOldWay() => new Expectations<ISimple>().Instance();

   [Benchmark]
	public ISimple ConstructNewWay() => new ISimpleCreateExpectations().Instance();
#pragma warning restore CA1822 // Mark members as static
}