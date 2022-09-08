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

public static class AbstractClassGenericPropertyTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = Rock.Create<AbstractClassGenericProperty<int>>();
		expectations.Properties().Getters().Values().Returns(returnValue);

		var mock = expectations.Instance();
		var value = mock.Values;

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeWithInit()
	{
		var returnValue = new List<string>();
		var expectations = Rock.Create<AbstractClassGenericPropertyGetAndInit<int>>();
		expectations.Properties().Getters().Values().Returns(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Values;

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = Rock.Make<AbstractClassGenericProperty<int>>().Instance();
		var value = mock.Values;

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = Rock.Make<AbstractClassGenericPropertyGetAndInit<int>>().Instance(null);
		var value = mock.Values;

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var expectations = Rock.Create<AbstractClassGenericProperty<int>>();
		expectations.Properties().Getters().Data().Returns(returnValue);

		var mock = expectations.Instance();
		var value = mock.Data;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterWithInit()
	{
		var returnValue = 3;
		var expectations = Rock.Create<AbstractClassGenericPropertyGetAndInit<int>>();
		expectations.Properties().Getters().Data().Returns(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Data;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = Rock.Make<AbstractClassGenericProperty<int>>().Instance();
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = Rock.Make<AbstractClassGenericPropertyGetAndInit<int>>().Instance(null);
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}
}