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
	[RockCreate<IInterfaceGenericProperty<int>>]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = new IInterfaceGenericPropertyOfintCreateExpectations();
		expectations.Properties.Getters.Values().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.Values;

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	[RockCreate<IInterfaceGenericPropertyGetAndInit<int>>]
	public static void CreateUsingGenericTypeWithInit()
	{
		var returnValue = new List<string>();
		var expectations = new IInterfaceGenericPropertyGetAndInitOfintCreateExpectations();
		expectations.Properties.Getters.Values().ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Values;

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	[RockMake<IInterfaceGenericProperty<int>>]
	public static void MakeUsingGenericType()
	{
		var mock = new IInterfaceGenericPropertyOfintMakeExpectations().Instance();
		var value = mock.Values;

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	[RockMake<IInterfaceGenericPropertyGetAndInit<int>>]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = new IInterfaceGenericPropertyGetAndInitOfintMakeExpectations().Instance(null);
		var value = mock.Values;

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	[RockCreate<IInterfaceGenericProperty<int>>]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var expectations = new IInterfaceGenericPropertyOfintCreateExpectations();
		expectations.Properties.Getters.Data().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.Data;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockCreate<IInterfaceGenericPropertyGetAndInit<int>>]
	public static void CreateUsingGenericTypeParameterWithInit()
	{
		var returnValue = 3;
		var expectations = new IInterfaceGenericPropertyGetAndInitOfintCreateExpectations();
		expectations.Properties.Getters.Data().ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Data;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockMake<IInterfaceGenericProperty<int>>]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = new IInterfaceGenericPropertyOfintMakeExpectations().Instance();
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<IInterfaceGenericPropertyGetAndInit<int>>]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = new IInterfaceGenericPropertyGetAndInitOfintMakeExpectations().Instance(null);
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}
}