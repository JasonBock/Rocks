using NUnit.Framework;

namespace Rocks.IntegrationTests;

public interface IMultipleRockCalls
{
	void Foo();
}

/// <summary>
/// These tests exists because at one point in development,
/// the generator would happily create multiple versions of the mock
/// which would cause an error because the hint names were the same,
/// and I want to prevent a regression of that ever happening.
/// </summary>
public static class MultipleRockCallsTests
{
	[Test]
	public static void CreateMocks()
	{
		var rock1 = Rock.Create<IMultipleRockCalls>();
		var rock2 = Rock.Create<IMultipleRockCalls>();

		rock1.Methods().Foo();
		rock2.Methods().Foo();

		var chunk1 = rock1.Instance();
		var chunk2 = rock2.Instance();

		chunk1.Foo();
		chunk2.Foo();

		rock1.Verify();
		rock2.Verify();
	}

	[Test]
	public static void MakeMocks()
	{
		Rock.Make<IMultipleRockCalls>().Instance().Foo();
		Rock.Make<IMultipleRockCalls>().Instance().Foo();
	}
}