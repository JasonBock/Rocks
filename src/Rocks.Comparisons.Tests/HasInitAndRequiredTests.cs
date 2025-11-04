using Moq;
using NSubstitute;
using NUnit.Framework;

namespace Rocks.Comparisons.Tests;

internal static class HasInitAndRequiredTests
{
	[Test]
	public static void UseRocks()
	{
		using var context = new RockContext();
		var expectations = context.Create<HasInitAndRequiredCreateExpectations>();
		expectations.Setups.ProvideDenominator().ReturnValue(2);

		var mock = expectations.Instance(new() { InitValue = 5, RequiredValue = 3 });
		Assert.That(mock.Calculate(), Is.EqualTo(4));
	}

	// There's no way to set the init properties.
	// The Object property returns an object that derives from HasInitAndRequired,
	// but it doesn't provide the hook to set those properties on construction.
	// Furthermore, since they're not-virtual, you can't define setups either,
	// and if you try, you won't be informed of this until runtime,
	// where you'll get an exception like this:
	// System.NotSupportedException : Unsupported expression: _ => _.InitValue
	[Test]
	[Ignore("Only used to illustrate mocking framework failure")]
	public static void UseMoq()
	{
		var repository = new MockRepository(MockBehavior.Strict);
		var expectations = repository.Create<HasInitAndRequired>();
		expectations.Setup(_ => _.ProvideDenominator()).Returns(2);
		//expectations.SetupGet(_ => _.InitValue).Returns(3);
		//expectations.SetupGet(_ => _.RequiredValue).Returns(5);

		var mock = expectations.Object;
		//mock.InitValue = 5;
		//mock.RequiredValue = 3;

		Assert.That(mock.Calculate(), Is.EqualTo(4));

		repository.VerifyAll();
	}

	// NSubstitute has the same issues as Moq.
	// You can't set the property values, nor
	// can you provide expectations/setups for them either.
	// If you try, you won't find out until runtime
	// where you'll get a CouldNotSetReturnDueToNoLastCallException.
	[Test]
	[Ignore("Only used to illustrate mocking framework failure")]
	public static void UseNSubstitute()
	{
		var mock = Substitute.For<HasInitAndRequired>();
		mock.ProvideDenominator().Returns(2);
		//mock.InitValue.Returns(3);
		//mock.RequiredValue.Returns(3);

		Assert.That(mock.Calculate(), Is.EqualTo(4));
	}
}