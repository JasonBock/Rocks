using NUnit.Framework;

namespace Rocks.Analysis.IntegrationTests.ClassGenericPropertyTestTypes;

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
		var expectations = new ClassGenericPropertyCreateExpectations<int>();
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
		var expectations = new ClassGenericPropertyGetAndInitCreateExpectations<int>();
		expectations.Properties.Getters.Values().ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Values;

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = new ClassGenericPropertyMakeExpectations<int>().Instance();
		var value = mock.Values;

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	public static void MakeUsingGenericTypeWithInit()
	{
		var mock = new ClassGenericPropertyGetAndInitMakeExpectations<int>().Instance(null);
		var value = mock.Values;

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	public static void CreateUsingGenericTypeParameter()
	{
		var returnValue = 3;
		var expectations = new ClassGenericPropertyCreateExpectations<int>();
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
		var expectations = new ClassGenericPropertyGetAndInitCreateExpectations<int>();
		expectations.Properties.Getters.Data().ReturnValue(returnValue);

		var mock = expectations.Instance(null);
		var value = mock.Data;

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeParameter()
	{
		var mock = new ClassGenericPropertyMakeExpectations<int>().Instance();
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void MakeUsingGenericTypeParameterWithInit()
	{
		var mock = new ClassGenericPropertyGetAndInitMakeExpectations<int>().Instance(null);
		var value = mock.Data;

		Assert.That(value, Is.EqualTo(default(int)));
	}
}