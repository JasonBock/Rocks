using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests;

public class ClassGenericMethod<T>
{
	public virtual void Foo(List<string> values) { }
	public virtual void Quux(T value) { }
	public virtual void Bar<TParam>(TParam value) { }
	public virtual List<string>? FooReturn() => default;
	public virtual T? QuuxReturn() => default;
	public virtual TReturn BarReturn<TReturn>() => default!;
	public virtual TData? NullableValues<TData>(TData? data) => default!;
}

public static class ClassGenericMethodTests
{
	[Test]
	[RockCreate<ClassGenericMethod<int>>]
	public static void CreateUsingGenericType()
	{
		var expectations = new ClassGenericMethodOfintCreateExpectations();
		expectations.Methods.Foo(Arg.Any<List<string>>());

		var mock = expectations.Instance();
		mock.Foo([]);

		expectations.Verify();
	}

	[Test]
	[RockMake<ClassGenericMethod<int>>]
	public static void MakeUsingGenericType()
	{
		var mock = new ClassGenericMethodOfintMakeExpectations().Instance();

		Assert.That(() => mock.Foo([]), Throws.Nothing);
	}

	[Test]
	[RockCreate<ClassGenericMethod<int>>]
	public static void CreateWithGenericTypeParameter()
	{
		var expectations = new ClassGenericMethodOfintCreateExpectations();
		expectations.Methods.Quux(Arg.Any<int>());

		var mock = expectations.Instance();
		mock.Quux(3);

		expectations.Verify();
	}

	[Test]
	[RockMake<ClassGenericMethod<int>>]
	public static void MakeWithGenericTypeParameter()
	{
		var mock = new ClassGenericMethodOfintMakeExpectations().Instance();

		Assert.That(() => mock.Quux(3), Throws.Nothing);
	}

	[Test]
	[RockCreate<ClassGenericMethod<int>>]
	public static void CreateWithGenericParameterType()
	{
		var expectations = new ClassGenericMethodOfintCreateExpectations();
		expectations.Methods.Bar(Arg.Any<int>());

		var mock = expectations.Instance();
		mock.Bar(3);

		expectations.Verify();
	}

	[Test]
	[RockMake<ClassGenericMethod<int>>]
	public static void MakeWithGenericParameterType()
	{
		var mock = new ClassGenericMethodOfintMakeExpectations().Instance();

		Assert.That(() => mock.Bar(3), Throws.Nothing);
	}

	[Test]
	[RockCreate<ClassGenericMethod<int>>]
	public static void CreateWithGenericParameterTypeThatDoesNotMatch()
	{
		var expectations = new ClassGenericMethodOfintCreateExpectations();
		expectations.Methods.Bar(Arg.Any<int>());

		var mock = expectations.Instance();

		Assert.That(() => mock.Bar("3"), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	[RockCreate<ClassGenericMethod<int>>]
	public static void CreateUsingGenericTypeAsReturn()
	{
		var returnValue = new List<string>();
		var expectations = new ClassGenericMethodOfintCreateExpectations();
		expectations.Methods.FooReturn().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.FooReturn();

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	[RockMake<ClassGenericMethod<int>>]
	public static void MakeUsingGenericTypeAsReturn()
	{
		var mock = new ClassGenericMethodOfintMakeExpectations().Instance();
		var value = mock.FooReturn();

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	[RockCreate<ClassGenericMethod<int>>]
	public static void CreateWithGenericTypeParameterAsReturn()
	{
		var returnValue = 3;
		var expectations = new ClassGenericMethodOfintCreateExpectations();
		expectations.Methods.QuuxReturn().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.QuuxReturn();

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockMake<ClassGenericMethod<int>>]
	public static void MakeWithGenericTypeParameterAsReturn()
	{
		var mock = new ClassGenericMethodOfintMakeExpectations().Instance();
		var value = mock.QuuxReturn();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<ClassGenericMethod<int>>]
	public static void CreateWithGenericParameterTypeAsReturn()
	{
		var returnValue = 3;
		var expectations = new ClassGenericMethodOfintCreateExpectations();
		expectations.Methods.BarReturn<int>().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.BarReturn<int>();

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockMake<ClassGenericMethod<int>>]
	public static void MakeWithGenericParameterTypeAsReturn()
	{
		var mock = new ClassGenericMethodOfintMakeExpectations().Instance();
		var value = mock.BarReturn<int>();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<ClassGenericMethod<int>>]
	public static void CreateWithGenericParameterTypeAsReturnThatDoesNotMatch()
	{
		var returnValue = 3;
		var expectations = new ClassGenericMethodOfintCreateExpectations();
		expectations.Methods.BarReturn<int>().ReturnValue(returnValue);

		var mock = expectations.Instance();

		Assert.That(mock.BarReturn<string>, Throws.TypeOf<ExpectationException>());
	}

	[Test]
	[RockCreate<ClassGenericMethod<int>>]
	public static void CreateWithNullableGenericParameterTypes()
	{
		var returnValue = "c";
		var expectations = new ClassGenericMethodOfintCreateExpectations();
		expectations.Methods.NullableValues<string>("b").ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.NullableValues("b");

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockMake<ClassGenericMethod<int>>]
	public static void MakeWithNullableGenericParameterTypes()
	{
		var mock = new ClassGenericMethodOfintMakeExpectations().Instance();
		var value = mock.NullableValues("b");

		Assert.That(value, Is.EqualTo(default(string)));
	}
}