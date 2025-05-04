using NUnit.Framework;
using Rocks.Analysis.IntegrationTests.Referenced;

namespace Rocks.Analysis.IntegrationTests.VisibilityTestTypes;

public static class VisibilityTests
{
	[Test]
	public static void CreateUsingInternalStuff()
	{
		using var context = new RockContext();
		var expectations = context.Create<IDoInternalStuffCreateExpectations>();
		expectations.Methods.Perform(Arg.Any<InternalDataStuff>());

		var mock = expectations.Instance();
		mock.Perform(new());
	}

	[Test]
	public static void CreateUsingPublicStuff()
	{
		using var context = new RockContext();
		var expectations = context.Create<IDoPublicStuffCreateExpectations>();
		expectations.Methods.Perform(Arg.Any<PublicDataStuff>());

		var mock = expectations.Instance();
		mock.Perform(new());
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