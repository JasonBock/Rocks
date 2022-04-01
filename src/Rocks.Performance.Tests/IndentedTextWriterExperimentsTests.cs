using NUnit.Framework;

namespace Rocks.Performance.Tests;

public static class IndentedTextWriterExperimentsTests
{
	[Test]
	public static void CreateWithBuilderSetToCapacity()
	{
		var experiments = new IndentedTextWriterExperiments();
		Assert.That(experiments.CreateWithBuilderSetToCapacity(200), Has.Length.EqualTo(3070));
	}
}