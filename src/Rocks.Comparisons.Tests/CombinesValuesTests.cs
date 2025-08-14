using Moq;
using NSubstitute;
using NUnit.Framework;

namespace Rocks.Comparisons.Tests;

internal static class CombinesValuesTests
{
	[Test]
	public static void UseRocks()
	{
		const string computeValue = "Mocked";
		var identifier = Guid.Parse("3f00bab2-ebbc-446c-b8b6-cb267734a327");
		const int number = 3;

		using var context = new RockContext();
		var expectations = context.Create<CombinesValuesCreateExpectations>();
		expectations.Methods.Compute().ReturnValue(computeValue);

		var mock = expectations.Instance(identifier, number);

		Assert.That(mock.Combine(), Is.EqualTo("3f00bab2-ebbc-446c-b8b6-cb267734a327 | 3 | Mocked"));
	}

	// This works, but note that the parameters need to be passed in
	// with a params object[] parameter. Therefore, boxing will occur
	// with value types, and you have to remember the order
	// of the constructor parameters.
	[Test]
	public static void UseMoq()
	{
		const string computeValue = "Mocked";
		var identifier = Guid.Parse("3f00bab2-ebbc-446c-b8b6-cb267734a327");
		const int number = 3;

		var repository = new MockRepository(MockBehavior.Strict);
		var expectations = repository.Create<CombinesValues>(identifier, number);
		expectations.Setup(_ => _.Compute()).Returns(computeValue);

		var mock = expectations.Object;
		Assert.That(mock.Combine(), Is.EqualTo("3f00bab2-ebbc-446c-b8b6-cb267734a327 | 3 | Mocked"));

		repository.VerifyAll();
	}

	// Same concerns with NSubstitute as I have with Moq.
	[Test]
	public static void UseNSubstitute()
	{
		const string computeValue = "Mocked";
		var identifier = Guid.Parse("3f00bab2-ebbc-446c-b8b6-cb267734a327");
		const int number = 3;

		var mock = Substitute.For<CombinesValues>(identifier, number);
		mock.Compute().Returns(computeValue);

		Assert.That(mock.Combine(), Is.EqualTo("3f00bab2-ebbc-446c-b8b6-cb267734a327 | 3 | Mocked"));
	}
}