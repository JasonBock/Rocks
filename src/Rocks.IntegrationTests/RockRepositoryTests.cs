using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests;

public interface IFirstRepository
{
	void Foo();
}

public interface ISecondRepository
{
	void Bar();
}

public static class RockRepositoryTests
{
	[Test]
	public static void UseRepository()
	{
		using var repository = new RockRepository();

		var expectationsFirst = repository.Create<IFirstRepository>();
		expectationsFirst.Methods().Foo();

		var expectationsSecond = repository.Create<ISecondRepository>();
		expectationsSecond.Methods().Bar();

		var mockFirst = expectationsFirst.Instance();
		mockFirst.Foo();

		var mockSecond = expectationsSecond.Instance();
		mockSecond.Bar();
	}

	[Test]
	public static void UseRepositoryWhenExpectationIsNotMet() =>
		Assert.That(() =>
		{
			using var repository = new RockRepository();

			var expectations = repository.Create<IFirstRepository>();
			expectations.Methods().Foo();

			_ = expectations.Instance();
		}, Throws.TypeOf<VerificationException>());
}