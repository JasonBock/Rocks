using NUnit.Framework;

namespace Rocks.IntegrationTests;

public class ClassGenericIndexer<T>
{
	public virtual List<string>? this[int a] => default;
	public virtual int this[int a, T b] => default;
	public virtual T? this[string a] => default;
}

public class ClassGenericIndexerGetAndInit<T>
{
	public virtual List<string>? this[int a] { get => default; init { } }
	public virtual int this[int a, T b] { get => default; init { } }
	public virtual T? this[string a] { get => default; init { } }
}

public static class ClassGenericIndexerTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = Rock.Create<ClassGenericIndexer<int>>();
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
		var expectations = Rock.Create<ClassGenericIndexerGetAndInit<int>>();
		expectations.Indexers().Getters().This(4).Returns(returnValue);

		var mock = expectations.Instance(null);
		var value = mock[4];

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = Rock.Make<ClassGenericIndexer<int>>().Instance();
		var value = mock[4];

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = Rock.Make<ClassGenericIndexerGetAndInit<int>>().Instance(null);
		var value = mock[4];

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var expectations = Rock.Create<ClassGenericIndexer<int>>();
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
		var expectations = Rock.Create<ClassGenericIndexerGetAndInit<int>>();
		expectations.Indexers().Getters().This(4, 5).Returns(returnValue);

		var mock = expectations.Instance(null);
		var value = mock[4, 5];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = Rock.Make<ClassGenericIndexer<int>>().Instance();
		var value = mock[4, 5];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = Rock.Make<ClassGenericIndexerGetAndInit<int>>().Instance(null);
		var value = mock[4, 5];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterAsReturn()
	{
		var returnValue = 3;
		var expectations = Rock.Create<ClassGenericIndexer<int>>();
		expectations.Indexers().Getters().This("b").Returns(returnValue);

		var chunk = expectations.Instance();
		var value = chunk["b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterAsReturnWithInit()
	{
		var returnValue = 3;
		var expectations = Rock.Create<ClassGenericIndexerGetAndInit<int>>();
		expectations.Indexers().Getters().This("b").Returns(returnValue);

		var mock = expectations.Instance(null);
		var value = mock["b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterAsReturn()
	{
		var mock = Rock.Make<ClassGenericIndexer<int>>().Instance();
		var value = mock["b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterAsReturnWithInit()
	{
		var mock = Rock.Make<ClassGenericIndexerGetAndInit<int>>().Instance(null);
		var value = mock["b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}
}