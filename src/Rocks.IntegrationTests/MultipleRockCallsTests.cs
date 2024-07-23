using NUnit.Framework;

namespace Rocks.IntegrationTests.MultipleRockCallsTestTypes;

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
		var expectations1 = new IMultipleRockCallsCreateExpectations();
		var expectations2 = new IMultipleRockCallsCreateExpectations();

		expectations1.Methods.Foo();
		expectations2.Methods.Foo();

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
		new IMultipleRockCallsMakeExpectations().Instance().Foo();
		new IMultipleRockCallsMakeExpectations().Instance().Foo();
	}
}