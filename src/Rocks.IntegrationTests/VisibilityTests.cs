using NUnit.Framework;
using Rocks.IntegrationTests.Referenced;

namespace Rocks.IntegrationTests.VisibilityTestTypes;

public static class VisibilityTests
{
	[Test]
	public static void CreateUsingInternalStuff()
	{
		var expectations = new IDoInternalStuffCreateExpectations();
		expectations.Methods.Perform(Arg.Any<InternalDataStuff>());

		var mock = expectations.Instance();
		mock.Perform(new());

		expectations.Verify();
	}

	[Test]
	public static void CreateUsingPublicStuff()
	{
		var expectations = new IDoPublicStuffCreateExpectations();
		expectations.Methods.Perform(Arg.Any<PublicDataStuff>());

		var mock = expectations.Instance();
		mock.Perform(new());

		expectations.Verify();
	}

	[Test]
	public static void MakeUsingInternalStuff()
	{
		var mock = new IDoInternalStuffMakeExpectations().Instance();
		mock.Perform(new());
	}

	[Test]
	public static void MakeUsingPublicStuff()
	{
		var mock = new IDoPublicStuffMakeExpectations().Instance();
		mock.Perform(new());
	}
}