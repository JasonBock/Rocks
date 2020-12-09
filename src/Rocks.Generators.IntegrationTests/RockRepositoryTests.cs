using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests
{
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

			var firstRock = repository.Add(Rock.Create<IFirstRepository>());
			firstRock.Methods().Foo();

			var secondRock = repository.Add(Rock.Create<ISecondRepository>());
			secondRock.Methods().Bar();

			var firstChunk = firstRock.Instance();
			firstChunk.Foo();

			var secondChunk = secondRock.Instance();
			secondChunk.Bar();
		}

		[Test]
		public static void UseRepositoryWhenExpectationIsNotMet() =>
			Assert.Multiple(() =>
			{
				Assert.That(() =>
				{
					using var repository = new RockRepository();

					var rock = repository.Add(Rock.Create<IFirstRepository>());
					rock.Methods().Foo();

					var chunk = rock.Instance();
				}, Throws.TypeOf<VerificationException>());
			});
	}
}