using NUnit.Framework;

namespace Rocks.IntegrationTests.MockVisibilityTestTypes;

public interface IProcessor<TProcessor>
	where TProcessor : IProcessor<TProcessor>
{
	void Process();
}

internal static class MockVisibilityTests
{
	[Test]
	public void Create()
	{
		var expectations = new IProcessorCreateExpectations<IProcessorCreateExpectations<>.Mock>
	}
}
