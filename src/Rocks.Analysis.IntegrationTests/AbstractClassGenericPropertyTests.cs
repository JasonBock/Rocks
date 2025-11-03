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
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassGenericPropertyCreateExpectations<int>>();
		expectations.Setups.Values.Gets().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.Values;

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeWithInit()
	{
		var returnValue = new List<string>();
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassGenericPropertyGetAndInitCreateExpectations<int>>();
		expectations.Setups.Values.Gets().ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Values;

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = new AbstractClassGenericPropertyMakeExpectations<int>().Instance();
		var value = mock.Values;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = new AbstractClassGenericPropertyGetAndInitMakeExpectations<int>().Instance(null);
		var value = mock.Values;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassGenericPropertyCreateExpectations<int>>();
		expectations.Setups.Data.Gets().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterWithInit()
	{
		var returnValue = 3;
		using var context = new RockContext(); 
		var expectations = context.Create<AbstractClassGenericPropertyGetAndInitCreateExpectations<int>>();
		expectations.Setups.Data.Gets().ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = new AbstractClassGenericPropertyMakeExpectations<int>().Instance();
		var value = mock.Data;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = new AbstractClassGenericPropertyGetAndInitMakeExpectations<int>().Instance(null);
		var value = mock.Data;

		Assert.That(value, Is.Default);
	}
}