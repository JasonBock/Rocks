using BenchmarkDotNet.Attributes;
using Rocks;
using Rocks.Performance.Integration;

[assembly: Rock<IHaveLotsOfMembers>(BuildType.Create)]

namespace Rocks.Performance.Integration;

/*
https://github.com/moq/moq/issues/1350

I was curious to see how Rocks compared to the perf
currently seen in Moq.

Grabbed the interface from:
https://gist.github.com/rauhs/4cbe672e26dd6727e84f7b96c68dcf1f

As it turns out, Rocks is pretty snappy :)

|   Method |     Mean |   Error |  StdDev | Allocated |
|--------- |---------:|--------:|--------:|----------:|
| CallMock | 530.7 us | 5.19 us | 4.85 us |       1 B |

This means that it takes 0.5 ms to do all 30,000 calls, 
which looks like it would be even better even with the
proposed code change to make Moq faster.
*/

[MemoryDiagnoser]
public class LargeInterfaceGenerationV4
{
	private readonly IHaveLotsOfMembers mock = null!;
	private readonly IHaveLotsOfMembers mockV4 = null!;
	public const int CallCount = 30_000;

	public LargeInterfaceGenerationV4()
	{
		var expectations = Rock.Create<IHaveLotsOfMembers>();
		expectations.Properties().Getters()
			.SomeProp().Returns(1).CallCount(LargeInterfaceGeneration.CallCount);
		expectations.Methods()
			.SomeMethod().Returns(1).CallCount(LargeInterfaceGeneration.CallCount);

		this.mock = expectations.Instance();

		var expectationsV4 = new IHaveLotsOfMembersCreateExpectations();
		expectationsV4.Properties.Getters.SomeProp().ReturnValue(1).ExpectedCallCount(LargeInterfaceGeneration.CallCount);
		expectationsV4.Methods.SomeMethod().ReturnValue(1).ExpectedCallCount(LargeInterfaceGeneration.CallCount);

		this.mockV4 = expectationsV4.Instance();
	}

	[Benchmark(Baseline = true)]
	public int CallCurrentWay()
	{
		var calls = 0;

		for (var i = 0; i < LargeInterfaceGeneration.CallCount; i++)
		{
			calls += this.mock.SomeProp;
			calls += this.mock.SomeMethod();
		}

		return calls;
	}

	[Benchmark]
	public int CallV4Way()
	{
		var calls = 0;

		for (var i = 0; i < LargeInterfaceGeneration.CallCount; i++)
		{
			calls += this.mockV4.SomeProp;
			calls += this.mockV4.SomeMethod();
		}

		return calls;
	}
}