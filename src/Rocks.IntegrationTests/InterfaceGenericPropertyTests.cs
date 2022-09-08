using NUnit.Framework;

namespace Rocks.IntegrationTests;

public interface IInterfaceGenericProperty<T>
{
	List<string> Values { get; }
	T Data { get; }
}

public interface IInterfaceGenericPropertyGetAndInit<T>
{
	List<string> Values { get; init; }
	T Data { get; init; }
}

public static class InterfaceGenericPropertyTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = Rock.Create<IInterfaceGenericProperty<int>>();
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
		var expectations = Rock.Create<IInterfaceGenericPropertyGetAndInit<int>>();
		expectations.Properties().Getters().Values().Returns(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Values;

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = Rock.Make<IInterfaceGenericProperty<int>>().Instance();
		var value = mock.Values;

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = Rock.Make<IInterfaceGenericPropertyGetAndInit<int>>().Instance(null);
		var value = mock.Values;

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var expectations = Rock.Create<IInterfaceGenericProperty<int>>();
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
		var expectations = Rock.Create<IInterfaceGenericPropertyGetAndInit<int>>();
		expectations.Properties().Getters().Data().Returns(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Data;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = Rock.Make<IInterfaceGenericProperty<int>>().Instance();
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = Rock.Make<IInterfaceGenericPropertyGetAndInit<int>>().Instance(null);
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}
}