using NUnit.Framework;

namespace Rocks.IntegrationTests;

public interface IInterfaceGenericIndexer<T>
{
	List<string> this[int a] { get; }
	int this[int a, T b] { get; }
	T this[string a] { get; }
}

public interface IInterfaceGenericIndexerGetAndInit<T>
{
	List<string> this[int a] { get; init; }
	int this[int a, T b] { get; init; }
	T this[string a] { get; init; }
}

public static class InterfaceGenericIndexerTests
{
	[Test]
	[RockCreate<IInterfaceGenericIndexer<int>>]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = new IInterfaceGenericIndexerOfintCreateExpectations();
		expectations.Indexers.Getters.This(4).ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock[4];

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	[RockCreate<IInterfaceGenericIndexerGetAndInit<int>>]
	public static void CreateUsingGenericTypeWithInit()
	{
		var returnValue = new List<string>();
		var expectations = new IInterfaceGenericIndexerGetAndInitOfintCreateExpectations();
		expectations.Indexers.Getters.This(4).ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock[4];

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	[RockMake<IInterfaceGenericIndexer<int>>]
	public static void MakeUsingGenericType()
	{
		var mock = new IInterfaceGenericIndexerOfintMakeExpectations().Instance();
		var value = mock[4];

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	[RockMake<IInterfaceGenericIndexerGetAndInit<int>>]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = new IInterfaceGenericIndexerGetAndInitOfintMakeExpectations().Instance(null);
		var value = mock[4];

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	[RockCreate<IInterfaceGenericIndexer<int>>]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var expectations = new IInterfaceGenericIndexerOfintCreateExpectations();
		expectations.Indexers.Getters.This(4, 5).ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock[4, 5];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockCreate<IInterfaceGenericIndexerGetAndInit<int>>]
	public static void CreateUsingGenericTypeParameterWithInit()
	{
		var returnValue = 3;
		var expectations = new IInterfaceGenericIndexerGetAndInitOfintCreateExpectations();
		expectations.Indexers.Getters.This(4, 5).ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock[4, 5];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockMake<IInterfaceGenericIndexer<int>>]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = new IInterfaceGenericIndexerOfintMakeExpectations().Instance();
		var value = mock[4, 5];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<IInterfaceGenericIndexerGetAndInit<int>>]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = new IInterfaceGenericIndexerGetAndInitOfintMakeExpectations().Instance(null);
		var value = mock[4, 5];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<IInterfaceGenericIndexer<int>>]
	public static void CreateUsingGenericTypeParameterAsReturn()
	{
		var returnValue = 3;
		var expectations = new IInterfaceGenericIndexerOfintCreateExpectations();
		expectations.Indexers.Getters.This("b").ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock["b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockCreate<IInterfaceGenericIndexerGetAndInit<int>>]
	public static void CreateUsingGenericTypeParameterAsReturnWithInit()
	{
		var returnValue = 3;
		var expectations = new IInterfaceGenericIndexerGetAndInitOfintCreateExpectations();
		expectations.Indexers.Getters.This("b").ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock["b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockMake<IInterfaceGenericIndexer<int>>]
	public static void MakeUsingGenericTypeParameterAsReturn()
	{
		var mock = new IInterfaceGenericIndexerOfintMakeExpectations().Instance();
		var value = mock["b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<IInterfaceGenericIndexerGetAndInit<int>>]
	public static void MakeUsingGenericTypeParameterAsReturnWithInit()
	{
		var mock = new IInterfaceGenericIndexerGetAndInitOfintMakeExpectations().Instance(null);
		var value = mock["b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}
}