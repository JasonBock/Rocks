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
	[RockCreate<ClassGenericProperty<int>>]
	public static void CreateUsingGenericType()
	{
		var returnValue = new List<string>();
		var expectations = new ClassGenericPropertyOfintCreateExpectations();
		expectations.Properties.Getters.Values().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.Values;

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	[RockCreate<ClassGenericPropertyGetAndInit<int>>]
	public static void CreateUsingGenericTypeWithInit()
	{
		var returnValue = new List<string>();
		var expectations = new ClassGenericPropertyGetAndInitOfintCreateExpectations();
		expectations.Properties.Getters.Values().ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Values;

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	[RockMake<ClassGenericProperty<int>>]
	public static void MakeUsingGenericType()
	{
		var mock = new ClassGenericPropertyOfintMakeExpectations().Instance();
		var value = mock.Values;

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	[RockMake<ClassGenericPropertyGetAndInit<int>>]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = new ClassGenericPropertyGetAndInitOfintMakeExpectations().Instance(null);
		var value = mock.Values;

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	[RockCreate<ClassGenericProperty<int>>]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var expectations = new ClassGenericPropertyOfintCreateExpectations();
		expectations.Properties.Getters.Data().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.Data;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockCreate<ClassGenericPropertyGetAndInit<int>>]
	public static void CreateUsingGenericTypeParameterWithInit()
	{
		var returnValue = 3;
		var expectations = new ClassGenericPropertyGetAndInitOfintCreateExpectations();
		expectations.Properties.Getters.Data().ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Data;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockMake<ClassGenericProperty<int>>]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = new ClassGenericPropertyOfintMakeExpectations().Instance();
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockMake<ClassGenericPropertyGetAndInit<int>>]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = new ClassGenericPropertyGetAndInitOfintMakeExpectations().Instance(null);
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}
}