using BenchmarkDotNet.Attributes;
using Rocks.Expectations;

namespace Rocks.Performance;

[MemoryDiagnoser]
public class CastingComparisonCallback
{
	private readonly Guid id = Guid.NewGuid();

	[Benchmark(Baseline = true)]
   public bool CallbackOldWay()
	{
		var wasCalled = false;
		var expectations = new Expectations<ISimple>();
		expectations.Methods().DoStuff(3, this.id).Callback((_, _) => { wasCalled = true; return "4"; });
		var returnValue = expectations.Instance().DoStuff(3, this.id);
		expectations.Verify();
		return wasCalled;
	}

	[Benchmark]
	public bool CallbackNewWay()
	{
		var wasCalled = false;
		var expectations = new ISimpleCreateExpectations();
		expectations.Methods.DoStuff(3, this.id).Callback((_, _) => { wasCalled = true; return "4"; });
		var returnValue = expectations.Instance().DoStuff(3, this.id);
		expectations.Verify();
		return wasCalled;
	}
}