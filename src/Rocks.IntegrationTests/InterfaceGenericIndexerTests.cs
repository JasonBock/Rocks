﻿using NUnit.Framework;

namespace Rocks.IntegrationTests.InterfaceGenericIndexerTestTypes;

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
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = new IInterfaceGenericIndexerCreateExpectations<int>();
		expectations.Indexers.Getters.This(4).ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock[4];

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeWithInit()
	{
		var returnValue = new List<string>();
		var expectations = new IInterfaceGenericIndexerGetAndInitCreateExpectations<int>();
		expectations.Indexers.Getters.This(4).ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock[4];

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = new IInterfaceGenericIndexerMakeExpectations<int>().Instance();
		var value = mock[4];

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = new IInterfaceGenericIndexerGetAndInitMakeExpectations<int>().Instance(null);
		var value = mock[4];

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var expectations = new IInterfaceGenericIndexerCreateExpectations<int>();
		expectations.Indexers.Getters.This(4, 5).ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock[4, 5];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterWithInit()
	{
		var returnValue = 3;
		var expectations = new IInterfaceGenericIndexerGetAndInitCreateExpectations<int>();
		expectations.Indexers.Getters.This(4, 5).ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock[4, 5];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = new IInterfaceGenericIndexerMakeExpectations<int>().Instance();
		var value = mock[4, 5];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = new IInterfaceGenericIndexerGetAndInitMakeExpectations<int>().Instance(null);
		var value = mock[4, 5];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterAsReturn()
	{
		var returnValue = 3;
		var expectations = new IInterfaceGenericIndexerCreateExpectations<int>();
		expectations.Indexers.Getters.This("b").ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock["b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterAsReturnWithInit()
	{
		var returnValue = 3;
		var expectations = new IInterfaceGenericIndexerGetAndInitCreateExpectations<int>();
		expectations.Indexers.Getters.This("b").ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock["b"];

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterAsReturn()
	{
		var mock = new IInterfaceGenericIndexerMakeExpectations<int>().Instance();
		var value = mock["b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterAsReturnWithInit()
	{
		var mock = new IInterfaceGenericIndexerGetAndInitMakeExpectations<int>().Instance(null);
		var value = mock["b"];

		Assert.That(value, Is.EqualTo(default(int)));
	}
}