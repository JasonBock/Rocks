using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests;

public abstract class AbstractClassGenericMethod<T>
{
	public abstract void Foo(List<string> values);
	public abstract void Quux(T value);
	public abstract void Bar<TParam>(TParam value);
	public abstract List<string> FooReturn();
	public abstract T QuuxReturn();
	public abstract TReturn BarReturn<TReturn>();
	public abstract TData? NullableValues<TData>(TData? data);
}

[RockCreate<AbstractClassGenericMethod<int>>]
[RockMake<AbstractClassGenericMethod<int>>]
public static class AbstractClassGenericMethodTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		var expectations = new AbstractClassGenericMethodOfintCreateExpectations();
		expectations.Methods.Foo(Arg.Any<List<string>>());

		var mock = expectations.Instance();
		mock.Foo([]);

		expectations.Verify();
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = new AbstractClassGenericMethodOfintMakeExpectations().Instance();
		mock.Foo([]);
	}

	[Test]
	public static void CreateWithGenericTypeParameter()
	{
		var expectations = new AbstractClassGenericMethodOfintCreateExpectations();
		expectations.Methods.Quux(Arg.Any<int>());

		var mock = expectations.Instance();
		mock.Quux(3);

		expectations.Verify();
	}

	[Test]
	public static void MakeWithGenericTypeParameter()
	{
		var mock = new AbstractClassGenericMethodOfintMakeExpectations().Instance();
		mock.Quux(3);
	}

	[Test]
	public static void CreateWithGenericParameterType()
	{
		var expectations = new AbstractClassGenericMethodOfintCreateExpectations();
		expectations.Methods.Bar(Arg.Any<int>());

		var mock = expectations.Instance();
		mock.Bar(3);

		expectations.Verify();
	}

	[Test]
	public static void MakeWithGenericParameterType()
	{
		var mock = new AbstractClassGenericMethodOfintMakeExpectations().Instance();
		mock.Bar(3);
	}

	[Test]
	public static void CreateWithGenericParameterTypeThatDoesNotMatch()
	{
		var expectations = new AbstractClassGenericMethodOfintCreateExpectations();
		expectations.Methods.Bar(Arg.Any<int>());

		var mock = expectations.Instance();

		Assert.That(() => mock.Bar("3"), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateUsingGenericTypeAsReturn()
	{
		var returnValue = new List<string>();
		var expectations = new AbstractClassGenericMethodOfintCreateExpectations();
		expectations.Methods.FooReturn().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.FooReturn();

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeAsReturn()
	{
		var mock = new AbstractClassGenericMethodOfintMakeExpectations().Instance();
		var value = mock.FooReturn();

		Assert.That(value, Is.EqualTo(default(List<string>)));
	}

	[Test]
	public static void CreateWithGenericTypeParameterAsReturn()
	{
		var returnValue = 3;
		var expectations = new AbstractClassGenericMethodOfintCreateExpectations();
		expectations.Methods.QuuxReturn().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.QuuxReturn();

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeWithGenericTypeParameterAsReturn()
	{
		var mock = new AbstractClassGenericMethodOfintMakeExpectations().Instance();
		var value = mock.QuuxReturn();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithGenericParameterTypeAsReturn()
	{
		var returnValue = 3;
		var expectations = new AbstractClassGenericMethodOfintCreateExpectations();
		expectations.Methods.BarReturn<int>().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.BarReturn<int>();

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeWithGenericParameterTypeAsReturn()
	{
		var mock = new AbstractClassGenericMethodOfintMakeExpectations().Instance();
		var value = mock.BarReturn<int>();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithGenericParameterTypeAsReturnThatDoesNotMatch()
	{
		var returnValue = 3;
		var expectations = new AbstractClassGenericMethodOfintCreateExpectations();
		expectations.Methods.BarReturn<int>().ReturnValue(returnValue);

		var mock = expectations.Instance();

		Assert.That(mock.BarReturn<string>, Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateWithNullableGenericParameterTypes()
	{
		var returnValue = "c";
		var expectations = new AbstractClassGenericMethodOfintCreateExpectations();
		expectations.Methods.NullableValues<string>("b").ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.NullableValues("b");

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeWithNullableGenericParameterTypes()
	{
		var mock = new AbstractClassGenericMethodOfintMakeExpectations().Instance();
		var value = mock.NullableValues("b");

		Assert.That(value, Is.EqualTo(default(string?)));
	}
}