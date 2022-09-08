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
		var expectations1 = Rock.Create<IMultipleRockCalls>();
		var expectations2 = Rock.Create<IMultipleRockCalls>();

		expectations1.Methods().Foo();
		expectations2.Methods().Foo();

		var mock1 = expectations1.Instance();
		var mock2 = expectations2.Instance();

		mock1.Foo();
		mock2.Foo();

		expectations1.Verify();
		expectations2.Verify();
	}

	[Test]
	public static void MakeMocks()
	{
		Rock.Make<IMultipleRockCalls>().Instance().Foo();
		Rock.Make<IMultipleRockCalls>().Instance().Foo();
	}
}