using BenchmarkDotNet.Attributes;

namespace Rocks.Performance;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class GettingTypes
{
	[Benchmark(Baseline = true)]
	public Type ObtainViaGetType() => this.GetType();

	[Benchmark]
#pragma warning disable CA1822 // Mark members as static
	public Type ObtainViaTypeOf() => typeof(GettingTypes);
#pragma warning restore CA1822 // Mark members as static
}