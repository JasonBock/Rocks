using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;

namespace Rocks.Performance;

[MemoryDiagnoser]
public class CastingSpeed
{
	private readonly Argument argument = new Argument<string>("3");

	[Benchmark(Baseline = true)]
	public Argument<string> AsCast() =>
		(this.argument as Argument<string>)!;

	[Benchmark]
	public Argument<string> DirectCast() =>
		(Argument<string>)this.argument;

	[Benchmark]
	public Argument<string> UnsafeAs() =>
		Unsafe.As<Argument<string>>(this.argument);
}
