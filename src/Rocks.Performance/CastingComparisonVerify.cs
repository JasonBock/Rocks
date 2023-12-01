using BenchmarkDotNet.Attributes;
using Rocks.Expectations;

namespace Rocks.Performance;

[MemoryDiagnoser]
public class CastingComparisonVerify
{
	private readonly Guid id = Guid.NewGuid();

	[Benchmark(Baseline = true)]
	public string VerifyOldWay()
	{
		var expectations = new Expectations<ISimple>();
		expectations.Methods().DoStuff(3, this.id).Returns("4");
		var returnValue = expectations.Instance().DoStuff(3, this.id);
		expectations.Verify();
		return returnValue;
	}

	[Benchmark]
	public string VerifyNewWay()
	{
		var expectations = new ISimpleCreateExpectations();
		expectations.Methods.DoStuff(3, this.id).Returns("4");
		var returnValue = expectations.Instance().DoStuff(3, this.id);
		expectations.Verify();
		return returnValue;
	}
}