using NUnit.Framework;

namespace Rocks.IntegrationTests;

public abstract class AbstractClassGenericIndexer<T>
{
	public abstract List<string> this[int a] { get; }
	public abstract int this[int a, T b] { get; }
	public abstract T this[string a] { get; }
}

public abstract class AbstractClassGenericIndexerGetAndInit<T>
{
	public abstract List<string> this[int a] { get; init; }
	public abstract int this[int a, T b] { get; init; }
	public abstract T this[string a] { get; init; }
}

public static class AbstractClassGenericIndexerTests
{
	[Test]
	[RockCreate<AbstractClassGenericIndexer<int>>]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = new AbstractClassGenericIndexerOfintCreateExpectations();
		expectations.Indexers.Getters.This(4).ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock[4];

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	[RockCreate<AbstractClassGenericIndexerGetAndInit<int>>]
	public static void CreateUsingGenericTypeWithInit()
	{
		var returnValue = new List<string>();
		var expectations = new AbstractClassGenericIndexerGetAndInitOfintCreateExpectations();
		expectations.Indexers.Getters.This(4).ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock[4];

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	[RockMake<AbstractClassGenericIndexer<int>>]
	public static void MakeUsingGenericType()
	{
		var mock = new AbstractClassGenericIndexerOfintMakeExpectations().Instance();
		var value = mock[4];

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	[RockMake<AbstractClassGenericIndexerGetAndInit<int>>]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = new AbstractClassGenericIndexerGetAndInitOfintMakeExpectations().Instance(null);
		var value = mock[4];

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	[RockCreate<AbstractClassGenericIndexer<int>>]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var expectations = new AbstractClassGenericIndexerOfintCreateExpectations();
		expectations.Indexers.Getters.This(4, 5).ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock[4, 5];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockCreate<AbstractClassGenericIndexerGetAndInit<int>>]
	public static void CreateUsingGenericTypeParameterWithInit()
	{
		var returnValue = 3;
		var expectations = new AbstractClassGenericIndexerGetAndInitOfintCreateExpectations();
		expectations.Indexers.Getters.This(4, 5).ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock[4, 5];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockMake<AbstractClassGenericIndexer<int>>]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = new AbstractClassGenericIndexerOfintMakeExpectations().Instance();
		var value = mock[4, 5];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<AbstractClassGenericIndexerGetAndInit<int>>]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = new AbstractClassGenericIndexerGetAndInitOfintMakeExpectations().Instance(null);
		var value = mock[4, 5];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<AbstractClassGenericIndexer<int>>]
	public static void CreateUsingGenericTypeParameterAsReturn()
	{
		var returnValue = 3;
		var expectations = new AbstractClassGenericIndexerOfintCreateExpectations();
		expectations.Indexers.Getters.This("b").ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock["b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockCreate<AbstractClassGenericIndexerGetAndInit<int>>]
	public static void CreateUsingGenericTypeParameterAsReturnWithInit()
	{
		var returnValue = 3;
		var expectations = new AbstractClassGenericIndexerGetAndInitOfintCreateExpectations();
		expectations.Indexers.Getters.This("b").ReturnValue(returnValue);

		var chunk = expectations.Instance(null);
		var value = chunk["b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockMake<AbstractClassGenericIndexer<int>>]
	public static void MakeUsingGenericTypeParameterAsReturn()
	{
		var mock = new AbstractClassGenericIndexerOfintMakeExpectations().Instance();
		var value = mock["b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<AbstractClassGenericIndexerGetAndInit<int>>]
	public static void MakeUsingGenericTypeParameterAsReturnWithInit()
	{
		var mock = new AbstractClassGenericIndexerGetAndInitOfintMakeExpectations().Instance(null);
		var value = mock["b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}
}