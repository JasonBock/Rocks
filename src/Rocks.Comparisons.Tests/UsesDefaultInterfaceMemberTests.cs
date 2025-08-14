using Moq;
using NSubstitute;
using NUnit.Framework;

namespace Rocks.Comparisons.Tests;

internal static class UsesDefaultInterfaceMemberTests
{
	[Test]
	public static void UseRocks()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveDefaultInterfaceMemberCreateExpectations>();
		expectations.Methods.Work().ReturnValue(3);

		var uses = new UsesDefaultInterfaceMember(expectations.Instance());
		Assert.That(uses.Execute(), Is.EqualTo(5));
	}

	[Test]
	public static void UseRocksOverridingDIM()
	{
		using var context = new RockContext();
		var expectations = context.Create<IHaveDefaultInterfaceMemberCreateExpectations>();
		expectations.Methods.Work().ReturnValue(3);
		expectations.Methods.DefaultWork().ReturnValue(3);

		var uses = new UsesDefaultInterfaceMember(expectations.Instance());
		Assert.That(uses.Execute(), Is.EqualTo(6));
	}

	// Even though you want to use the default implementation,
	// you still need to set up the DefaultWork() call,
	// and call CallBase() on the ISetup<> instance.
	[Test]
	public static void UseMoq()
	{
		var repository = new MockRepository(MockBehavior.Strict);
		var expectations = repository.Create<IHaveDefaultInterfaceMember>();
		expectations.Setup(_ => _.Work()).Returns(3);
		expectations.Setup(_ => _.DefaultWork()).CallBase();

		var uses = new UsesDefaultInterfaceMember(expectations.Object);
		Assert.That(uses.Execute(), Is.EqualTo(5));

		expectations.VerifyAll();
	}

	[Test]
	public static void UseMoqOverridingDIM()
	{
		var repository = new MockRepository(MockBehavior.Strict);
		var expectations = repository.Create<IHaveDefaultInterfaceMember>();
		expectations.Setup(_ => _.Work()).Returns(3);
		expectations.Setup(_ => _.DefaultWork()).Returns(3);

		var uses = new UsesDefaultInterfaceMember(expectations.Object);
		Assert.That(uses.Execute(), Is.EqualTo(6));

		expectations.VerifyAll();
	}

	// NSubstitute will fail if you don't provide an implementation.
	// In this test, if I didn't provide a return value for DefaultWork(),
	// the assertion fails.
	[Test]
	public static void UseNSubstitute()
	{
		var mock = Substitute.For<IHaveDefaultInterfaceMember>();
		mock.Work().Returns(3);
		mock.DefaultWork().Returns(2);

		var uses = new UsesDefaultInterfaceMember(mock);
		Assert.That(uses.Execute(), Is.EqualTo(5));
	}

	[Test]
	public static void UseNSubstituteOverridingDIM()
	{
		var mock = Substitute.For<IHaveDefaultInterfaceMember>();
		mock.Work().Returns(3);
		mock.DefaultWork().Returns(3);

		var uses = new UsesDefaultInterfaceMember(mock);
		Assert.That(uses.Execute(), Is.EqualTo(6));
	}
}