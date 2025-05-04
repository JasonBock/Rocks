using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.ClassGenericIndexerTestTypes;

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
		using var context = new RockContext(); 
		var expectations = context.Create<ClassGenericIndexerCreateExpectations<int>>();
		expectations.Indexers.Getters.This(4).ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock[4];

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeWithInit()
	{
		var returnValue = new List<string>();
		using var context = new RockContext(); 
		var expectations = context.Create<ClassGenericIndexerGetAndInitCreateExpectations<int>>();
		expectations.Indexers.Getters.This(4).ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock[4];

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = new ClassGenericIndexerMakeExpectations<int>().Instance();
		var value = mock[4];

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = new ClassGenericIndexerGetAndInitMakeExpectations<int>().Instance(null);
		var value = mock[4];

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		using var context = new RockContext(); 
		var expectations = context.Create<ClassGenericIndexerCreateExpectations<int>>();
		expectations.Indexers.Getters.This(4, 5).ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock[4, 5];

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterWithInit()
	{
		var returnValue = 3;
		using var context = new RockContext(); 
		var expectations = context.Create<ClassGenericIndexerGetAndInitCreateExpectations<int>>();
		expectations.Indexers.Getters.This(4, 5).ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock[4, 5];

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = new ClassGenericIndexerMakeExpectations<int>().Instance();
		var value = mock[4, 5];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = new ClassGenericIndexerGetAndInitMakeExpectations<int>().Instance(null);
		var value = mock[4, 5];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateUsingGenericTypeParameterAsReturn()
	{
		var returnValue = 3;
		using var context = new RockContext(); 
		var expectations = context.Create<ClassGenericIndexerCreateExpectations<int>>();
		expectations.Indexers.Getters.This("b").ReturnValue(returnValue);

		var chunk = expectations.Instance();
		var value = chunk["b"];

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterAsReturnWithInit()
	{
		var returnValue = 3;
		using var context = new RockContext(); 
		var expectations = context.Create<ClassGenericIndexerGetAndInitCreateExpectations<int>>();
		expectations.Indexers.Getters.This("b").ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock["b"];

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterAsReturn()
	{
		var mock = new ClassGenericIndexerMakeExpectations<int>().Instance();
		var value = mock["b"];

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeUsingGenericTypeParameterAsReturnWithInit()
	{
		var mock = new ClassGenericIndexerGetAndInitMakeExpectations<int>().Instance(null);
		var value = mock["b"];

		Assert.That(value, Is.Default);
	}
}