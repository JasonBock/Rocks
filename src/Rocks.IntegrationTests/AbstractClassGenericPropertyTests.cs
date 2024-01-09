using NUnit.Framework;

namespace Rocks.IntegrationTests;

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

[RockCreate<AbstractClassGenericProperty<int>>]
[RockCreate<AbstractClassGenericPropertyGetAndInit<int>>]
[RockMake<AbstractClassGenericProperty<int>>]
[RockMake<AbstractClassGenericPropertyGetAndInit<int>>]
public static class AbstractClassGenericPropertyTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = new AbstractClassGenericPropertyOfintCreateExpectations();
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
		var expectations = new AbstractClassGenericPropertyGetAndInitOfintCreateExpectations();
		expectations.Properties.Getters.Values().ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Values;

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = new AbstractClassGenericPropertyOfintMakeExpectations().Instance();
		var value = mock.Values;

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = new AbstractClassGenericPropertyGetAndInitOfintMakeExpectations().Instance(null);
		var value = mock.Values;

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var expectations = new AbstractClassGenericPropertyOfintCreateExpectations();
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
		var expectations = new AbstractClassGenericPropertyGetAndInitOfintCreateExpectations();
		expectations.Properties.Getters.Data().ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Data;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = new AbstractClassGenericPropertyOfintMakeExpectations().Instance();
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = new AbstractClassGenericPropertyGetAndInitOfintMakeExpectations().Instance(null);
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}
}