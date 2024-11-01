using NUnit.Framework;

namespace Rocks.IntegrationTests.PartialTestTypes;

public interface ITargetPartial
{
	void Work();
}

[RockPartial(typeof(ITargetPartial), BuildType.Create)]
public sealed partial class PartialCreateExpectations;

[RockPartial(typeof(ITargetPartial), BuildType.Make)]
public sealed partial class PartialMakeExpectations;

public static class PartialTests
{
	[Test]
	public static void Create()
	{
		var expectations = new PartialCreateExpectations();
		expectations.Methods.Work();

		var mock = expectations.Instance();
		mock.Work();

		expectations.Verify();
	}

	[Test]
	public static void Make()
	{
		var mock = new PartialMakeExpectations().Instance();
		mock.Work();
	}
}