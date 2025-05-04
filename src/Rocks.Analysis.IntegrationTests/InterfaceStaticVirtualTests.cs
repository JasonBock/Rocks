using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.InterfaceStaticVirtualTestTypes;

public interface IHaveStaticVirtuals
{
	string InstanceLift();

	string? InstancePush { get; set; }

	public static virtual string StaticLift() => "Lift";

	public static virtual string? StaticPush { get; set; }
}

public static class InterfaceStaticVirtualTests
{
	[Test]
	public static void Create()
	{
		var expectations = new IHaveStaticVirtualsCreateExpectations();
		expectations.Methods.InstanceLift().ReturnValue("a");
		expectations.Properties.Getters.InstancePush().ReturnValue("b");
		expectations.Properties.Setters.InstancePush("c");

		var mock = expectations.Instance();
		mock.InstancePush = "c";

		Assert.Multiple(() =>
		{
			Assert.That(mock.InstancePush, Is.EqualTo("b"));
			Assert.That(mock.InstanceLift(), Is.EqualTo("a"));
		});

		expectations.Verify();
	}

	[Test]
	public static void Make()
	{
		var mock = new IHaveStaticVirtualsMakeExpectations().Instance();

		Assert.Multiple(() =>
		{
			mock.InstancePush = "a";
			Assert.That(mock.InstanceLift(), Is.Null);
			Assert.That(mock.InstancePush, Is.Null);
		});
	}
}