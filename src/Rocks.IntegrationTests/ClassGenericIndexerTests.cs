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
	[RockCreate<ClassGenericIndexer<int>>]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = new ClassGenericIndexerOfintCreateExpectations();
		expectations.Indexers.Getters.This(4).ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock[4];

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	[RockCreate<ClassGenericIndexerGetAndInit<int>>]
	public static void CreateUsingGenericTypeWithInit()
	{
		var returnValue = new List<string>();
		var expectations = new ClassGenericIndexerGetAndInitOfintCreateExpectations();
		expectations.Indexers.Getters.This(4).ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock[4];

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	[RockMake<ClassGenericIndexer<int>>]
	public static void MakeUsingGenericType()
	{
		var mock = new ClassGenericIndexerOfintMakeExpectations().Instance();
		var value = mock[4];

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	[RockMake<ClassGenericIndexerGetAndInit<int>>]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = new ClassGenericIndexerGetAndInitOfintMakeExpectations().Instance(null);
		var value = mock[4];

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	[RockCreate<ClassGenericIndexer<int>>]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var expectations = new ClassGenericIndexerOfintCreateExpectations();
		expectations.Indexers.Getters.This(4, 5).ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock[4, 5];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockCreate<ClassGenericIndexerGetAndInit<int>>]
	public static void CreateUsingGenericTypeParameterWithInit()
	{
		var returnValue = 3;
		var expectations = new ClassGenericIndexerGetAndInitOfintCreateExpectations();
		expectations.Indexers.Getters.This(4, 5).ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock[4, 5];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockMake<ClassGenericIndexer<int>>]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = new ClassGenericIndexerOfintMakeExpectations().Instance();
		var value = mock[4, 5];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<ClassGenericIndexerGetAndInit<int>>]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = new ClassGenericIndexerGetAndInitOfintMakeExpectations().Instance(null);
		var value = mock[4, 5];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<ClassGenericIndexer<int>>]
	public static void CreateUsingGenericTypeParameterAsReturn()
	{
		var returnValue = 3;
		var expectations = new ClassGenericIndexerOfintCreateExpectations();
		expectations.Indexers.Getters.This("b").ReturnValue(returnValue);

		var chunk = expectations.Instance();
		var value = chunk["b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockCreate<ClassGenericIndexerGetAndInit<int>>]
	public static void CreateUsingGenericTypeParameterAsReturnWithInit()
	{
		var returnValue = 3;
		var expectations = new ClassGenericIndexerGetAndInitOfintCreateExpectations();
		expectations.Indexers.Getters.This("b").ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock["b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockMake<ClassGenericIndexer<int>>]
	public static void MakeUsingGenericTypeParameterAsReturn()
	{
		var mock = new ClassGenericIndexerOfintMakeExpectations().Instance();
		var value = mock["b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<ClassGenericIndexerGetAndInit<int>>]
	public static void MakeUsingGenericTypeParameterAsReturnWithInit()
	{
		var mock = new ClassGenericIndexerGetAndInitOfintMakeExpectations().Instance(null);
		var value = mock["b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}
}