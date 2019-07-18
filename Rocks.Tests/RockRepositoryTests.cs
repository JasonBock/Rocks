using NUnit.Framework;
using Rocks.Exceptions;
using Rocks.Options;
using System;

namespace Rocks.Tests
{
	public static class RockRepositoryTests
	{
		[Test]
		public static void Create()
		{
			var options = new RockOptions();
			using var repository = new RockRepository(options);
			Assert.That(repository.Options, Is.SameAs(options));
		}

		[Test]
		public static void CreateWithDefaultOptions()
		{
			using var repository = new RockRepository();
			Assert.That(repository.Options, Is.Not.Null);
		}

		[Test]
		public static void CreateWithNullOptions() => 
			Assert.That(() => new RockRepository(null!), Throws.InstanceOf<ArgumentNullException>());

		[Test]
		public static void CreateMockWithValidVerification() => 
			Assert.That(() =>
			{
				using var repository = new RockRepository();
				var rock = repository.Create<ITestRepository>();
				rock.Handle(_ => _.Foo());
				var chunk = rock.Make();
				chunk.Foo();
			}, Throws.Nothing);

		[Test]
		public static void CreateMockWithInvalidVerification() =>
			Assert.That(() =>
			{
				using var repository = new RockRepository();
				var rock = repository.Create<ITestRepository>();
				rock.Handle(_ => _.Foo());
				var chunk = rock.Make();
			}, Throws.InstanceOf<VerificationException>());

		[Test]
		public static void TryCreateMockWithValidVerification() =>
			Assert.That(() =>
			{
				using var repository = new RockRepository();
				var (_, rock) = repository.TryCreate<ITestRepository>();
				rock!.Handle(_ => _.Foo());
				var chunk = rock.Make();
				chunk.Foo();
			}, Throws.Nothing);

		[Test]
		public static void TryCreateMockWithInvalidVerification() =>
			Assert.That(() =>
			{
				using var repository = new RockRepository();
				var (_, rock) = repository.TryCreate<ITestRepository>();
				rock!.Handle(_ => _.Foo());
				var chunk = rock.Make();
			}, Throws.InstanceOf<VerificationException>());

		[Test]
		public static void TryCreateMockWithUnsuccessfulCreation()
		{
			using var repository = new RockRepository();
			var (isSuccessful, _) = repository.TryCreate<TestRepositoryNotForMaking>();
			Assert.That(isSuccessful, Is.False);
		}
	}

	public sealed class TestRepositoryNotForMaking { }

	public interface ITestRepository
	{
		void Foo();
	}
}