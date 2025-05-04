using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.InterfaceGenericPropertyTestTypes;

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
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceGenericPropertyCreateExpectations<int>>();
		expectations.Properties.Getters.Values().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.Values;

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeWithInit()
	{
		var returnValue = new List<string>();
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceGenericPropertyGetAndInitCreateExpectations<int>>();
		expectations.Properties.Getters.Values().ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Values;

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = new IInterfaceGenericPropertyMakeExpectations<int>().Instance();
		var value = mock.Values;

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = new IInterfaceGenericPropertyGetAndInitMakeExpectations<int>().Instance(null);
		var value = mock.Values;

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceGenericPropertyCreateExpectations<int>>();
		expectations.Properties.Getters.Data().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void CreateUsingGenericTypeParameterWithInit()
	{
		var returnValue = 3;
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceGenericPropertyGetAndInitCreateExpectations<int>>();
		expectations.Properties.Getters.Data().ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = new IInterfaceGenericPropertyMakeExpectations<int>().Instance();
		var value = mock.Data;

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = new IInterfaceGenericPropertyGetAndInitMakeExpectations<int>().Instance(null);
		var value = mock.Data;

		Assert.That(value, Is.Default);
	}
}