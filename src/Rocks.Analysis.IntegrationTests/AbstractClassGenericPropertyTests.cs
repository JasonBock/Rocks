using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.AbstractClassGenericPropertyTestTypes;

public abstract class AbstractClassGenericProperty<T>
{
	public abstract List<string> Values { get; }
	public abstract T Data { get; }
}

public abstract class AbstractClassGenericPropertyGetAndInit<T>
{
	public abstract List<string> Values { get; init; }
	public abstract T Data { get; init; }
}

public static class AbstractClassGenericPropertyTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = new AbstractClassGenericPropertyCreateExpectations<int>();
		expectations.Properties.Getters.Values().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.Values;

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeWithInit()
	{
		var returnValue = new List<string>();
		var expectations = new AbstractClassGenericPropertyGetAndInitCreateExpectations<int>();
		expectations.Properties.Getters.Values().ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Values;

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = new AbstractClassGenericPropertyMakeExpectations<int>().Instance();
		var value = mock.Values;

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = new AbstractClassGenericPropertyGetAndInitMakeExpectations<int>().Instance(null);
		var value = mock.Values;

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var expectations = new AbstractClassGenericPropertyCreateExpectations<int>();
		expectations.Properties.Getters.Data().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.Data;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterWithInit()
	{
		var returnValue = 3;
		var expectations = new AbstractClassGenericPropertyGetAndInitCreateExpectations<int>();
		expectations.Properties.Getters.Data().ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Data;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = new AbstractClassGenericPropertyMakeExpectations<int>().Instance();
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = new AbstractClassGenericPropertyGetAndInitMakeExpectations<int>().Instance(null);
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}
}