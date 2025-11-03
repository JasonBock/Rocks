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
		using var context = new RockContext();
		var expectations = context.Create<IHaveStaticVirtualsCreateExpectations>();
		expectations.Setups.InstanceLift().ReturnValue("a");
		expectations.Setups.InstancePush.Gets().ReturnValue("b");
		expectations.Setups.InstancePush.Sets("c");

		var mock = expectations.Instance();
		mock.InstancePush = "c";

	  using (Assert.EnterMultipleScope())
	  {
			Assert.That(mock.InstancePush, Is.EqualTo("b"));
			Assert.That(mock.InstanceLift(), Is.EqualTo("a"));
		}
	}

	[Test]
	public static void Make()
	{
		var mock = new IHaveStaticVirtualsMakeExpectations().Instance();

	  using (Assert.EnterMultipleScope())
	  {
			mock.InstancePush = "a";
			Assert.That(mock.InstanceLift(), Is.Null);
			Assert.That(mock.InstancePush, Is.Null);
		}
	}
}