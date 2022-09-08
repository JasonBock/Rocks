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
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = Rock.Create<AbstractClassGenericIndexer<int>>();
		expectations.Indexers().Getters().This(4).Returns(returnValue);

		var mock = expectations.Instance();
		var value = mock[4];

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeWithInit()
	{
		var returnValue = new List<string>();
		var expectations = Rock.Create<AbstractClassGenericIndexerGetAndInit<int>>();
		expectations.Indexers().Getters().This(4).Returns(returnValue);

		var mock = expectations.Instance();
		var value = mock[4];

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = Rock.Make<AbstractClassGenericIndexer<int>>().Instance();
		var value = mock[4];

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = Rock.Make<AbstractClassGenericIndexerGetAndInit<int>>().Instance();
		var value = mock[4];

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var expectations = Rock.Create<AbstractClassGenericIndexer<int>>();
		expectations.Indexers().Getters().This(4, 5).Returns(returnValue);

		var mock = expectations.Instance();
		var value = mock[4, 5];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterWithInit()
	{
		var returnValue = 3;
		var expectations = Rock.Create<AbstractClassGenericIndexerGetAndInit<int>>();
		expectations.Indexers().Getters().This(4, 5).Returns(returnValue);

		var mock = expectations.Instance();
		var value = mock[4, 5];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = Rock.Make<AbstractClassGenericIndexer<int>>().Instance();
		var value = mock[4, 5];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = Rock.Make<AbstractClassGenericIndexerGetAndInit<int>>().Instance();
		var value = mock[4, 5];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterAsReturn()
	{
		var returnValue = 3;
		var expectations = Rock.Create<AbstractClassGenericIndexer<int>>();
		expectations.Indexers().Getters().This("b").Returns(returnValue);

		var mock = expectations.Instance();
		var value = mock["b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterAsReturnWithInit()
	{
		var returnValue = 3;
		var expectations = Rock.Create<AbstractClassGenericIndexerGetAndInit<int>>();
		expectations.Indexers().Getters().This("b").Returns(returnValue);

		var chunk = expectations.Instance();
		var value = chunk["b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterAsReturn()
	{
		var mock = Rock.Make<AbstractClassGenericIndexer<int>>().Instance();
		var value = mock["b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterAsReturnWithInit()
	{
		var mock = Rock.Make<AbstractClassGenericIndexerGetAndInit<int>>().Instance();
		var value = mock["b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}
}