using NUnit.Framework;

namespace Rocks.Performance.Integration.Tests;

public static class LargeInterfaceGenerationTests
{
	[Test]
	public static void Run()
	{
		var benchmarks = new LargeInterfaceGeneration();
		var calls = benchmarks.CallMock();

		Assert.That(calls, Is.EqualTo(LargeInterfaceGeneration.CallCount * 2));
	}
}