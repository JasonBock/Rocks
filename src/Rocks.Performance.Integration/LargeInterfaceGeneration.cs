using BenchmarkDotNet.Attributes;

namespace Rocks.Performance.Integration;

/*
https://github.com/moq/moq/issues/1350

I was curious to see how Rocks compared to the perf
currently seen in Moq.

Grabbed the interface from:
https://gist.github.com/rauhs/4cbe672e26dd6727e84f7b96c68dcf1f

As it turns out, Rocks is pretty snappy :)

| Method   | Mean     | Error   | StdDev  | Allocated |
|--------- |---------:|--------:|--------:|----------:|
| CallMock | 164.4 us | 3.28 us | 5.38 us |         - |

This means that it takes 0.5 ms to do all 30,000 calls, 
which looks like it would be even better even with the
proposed code change to make Moq faster.
*/


[MemoryDiagnoser]
[RockCreate<IHaveLotsOfMembers>]
public class LargeInterfaceGeneration
{
	private readonly IHaveLotsOfMembersCreateExpectations expectations = null!;
	private readonly IHaveLotsOfMembers mock = null!;
	public const int CallCount = 30_000;

	public LargeInterfaceGeneration()
	{
		this.expectations = new IHaveLotsOfMembersCreateExpectations();
		this.expectations.Properties.Getters
			.SomeProp().ReturnValue(1).ExpectedCallCount(LargeInterfaceGeneration.CallCount);
		this.expectations.Methods
			.SomeMethod().ReturnValue(1).ExpectedCallCount(LargeInterfaceGeneration.CallCount);

		this.mock = this.expectations.Instance();
	}

	[Benchmark]
	public int CallMock()
	{
		var calls = 0;

		for (var i = 0; i < LargeInterfaceGeneration.CallCount; i++)
		{
			calls += this.mock.SomeProp;
			calls += this.mock.SomeMethod();
		}

		return calls;
	}
}