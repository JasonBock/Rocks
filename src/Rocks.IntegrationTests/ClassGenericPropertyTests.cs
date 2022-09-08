using NUnit.Framework;

namespace Rocks.IntegrationTests;

public class ClassGenericProperty<T>
{
	public virtual List<string> Values => default!;
	public virtual T Data => default!;
}

public class ClassGenericPropertyGetAndInit<T>
{
	public virtual List<string> Values { get => default!; init { } }
	public virtual T Data { get => default!; init { } }
}

public static class ClassGenericPropertyTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = Rock.Create<ClassGenericProperty<int>>();
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
		var expectations = Rock.Create<ClassGenericPropertyGetAndInit<int>>();
		expectations.Properties().Getters().Values().Returns(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Values;

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = Rock.Make<ClassGenericProperty<int>>().Instance();
		var value = mock.Values;

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = Rock.Make<ClassGenericPropertyGetAndInit<int>>().Instance(null);
		var value = mock.Values;

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var expectations = Rock.Create<ClassGenericProperty<int>>();
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
		var expectations = Rock.Create<ClassGenericPropertyGetAndInit<int>>();
		expectations.Properties().Getters().Data().Returns(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Data;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = Rock.Make<ClassGenericProperty<int>>().Instance();
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = Rock.Make<ClassGenericPropertyGetAndInit<int>>().Instance(null);
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}
}