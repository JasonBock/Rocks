// TODO: Gotta come back to this!

//using NUnit.Framework;
//using Rocks.Exceptions;

//namespace Rocks.IntegrationTests;

//public interface IFirstRepository
//{
//	void Foo();
//}

//public interface ISecondRepository
//{
//	void Bar();
//}

//public static class RockRepositoryTests
//{
//	[Test]
//	public static void CreateAfterDisposeWasCalled()
//	{
//		var repository = new RockRepository();

//		var expectationsFirst = repository.Create<IFirstRepository>();
//		expectationsFirst.Methods().Foo();

//		var mockFirst = expectationsFirst.Instance();
//		mockFirst.Foo();

//		repository.Dispose();

//		Assert.That(() => _ = repository.Create<IFirstRepository>(), 
//			Throws.TypeOf<ObjectDisposedException>());
//	}

//	[Test]
//	public static void DisposeMoreThanOnce()
//	{
//		var repository = new RockRepository();

//		var expectationsFirst = repository.Create<IFirstRepository>();
//		expectationsFirst.Methods().Foo();

//		var mockFirst = expectationsFirst.Instance();
//		mockFirst.Foo();

//		repository.Dispose();

//		Assert.That(() => repository.Dispose(),
//			Throws.TypeOf<ObjectDisposedException>());
//	}

//	[Test]
//	public static void UseRepository()
//	{
//		using var repository = new RockRepository();

//		var expectationsFirst = repository.Create<IFirstRepository>();
//		expectationsFirst.Methods().Foo();

//		var expectationsSecond = repository.Create<ISecondRepository>();
//		expectationsSecond.Methods().Bar();

//		var mockFirst = expectationsFirst.Instance();
//		mockFirst.Foo();

//		var mockSecond = expectationsSecond.Instance();
//		mockSecond.Bar();
//	}

//	[Test]
//	public static void UseRepositoryWhenExpectationIsNotMet() =>
//		Assert.That(() =>
//		{
//			using var repository = new RockRepository();

//			var expectations = repository.Create<IFirstRepository>();
//			expectations.Methods().Foo();

//			_ = expectations.Instance();
//		}, Throws.TypeOf<VerificationException>());
//}