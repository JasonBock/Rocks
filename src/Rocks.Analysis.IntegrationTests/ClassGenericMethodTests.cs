using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Analysis.IntegrationTests.ClassGenericMethodTestTypes;

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
	public static void CreateUsingGenericType()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassGenericMethodCreateExpectations<int>>();
		expectations.Methods.Foo(Arg.Any<List<string>>());

		var mock = expectations.Instance();
		mock.Foo([]);
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = new ClassGenericMethodMakeExpectations<int>().Instance();

		Assert.That(() => mock.Foo([]), Throws.Nothing);
	}

	[Test]
	public static void CreateWithGenericTypeParameter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassGenericMethodCreateExpectations<int>>();
		expectations.Methods.Quux(Arg.Any<int>());

		var mock = expectations.Instance();
		mock.Quux(3);
	}

	[Test]
	public static void MakeWithGenericTypeParameter()
	{
		var mock = new ClassGenericMethodMakeExpectations<int>().Instance();

		Assert.That(() => mock.Quux(3), Throws.Nothing);
	}

	[Test]
	public static void CreateWithGenericParameterType()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<ClassGenericMethodCreateExpectations<int>>();
		expectations.Methods.Bar(Arg.Any<int>());

		var mock = expectations.Instance();
		mock.Bar(3);
	}

	[Test]
	public static void MakeWithGenericParameterType()
	{
		var mock = new ClassGenericMethodMakeExpectations<int>().Instance();

		Assert.That(() => mock.Bar(3), Throws.Nothing);
	}

	[Test]
	public static void CreateWithGenericParameterTypeThatDoesNotMatch()
	{
		var expectations = new ClassGenericMethodCreateExpectations<int>();
		expectations.Methods.Bar(Arg.Any<int>());

		var mock = expectations.Instance();

		Assert.That(() => mock.Bar("3"), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateUsingGenericTypeAsReturn()
	{
		var returnValue = new List<string>();
		using var context = new RockContext(); 
		var expectations = context.Create<ClassGenericMethodCreateExpectations<int>>();
		expectations.Methods.FooReturn().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.FooReturn();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeAsReturn()
	{
		var mock = new ClassGenericMethodMakeExpectations<int>().Instance();
		var value = mock.FooReturn();

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	public static void CreateWithGenericTypeParameterAsReturn()
	{
		var returnValue = 3;
		using var context = new RockContext(); 
		var expectations = context.Create<ClassGenericMethodCreateExpectations<int>>();
		expectations.Methods.QuuxReturn().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.QuuxReturn();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeWithGenericTypeParameterAsReturn()
	{
		var mock = new ClassGenericMethodMakeExpectations<int>().Instance();
		var value = mock.QuuxReturn();

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithGenericParameterTypeAsReturn()
	{
		var returnValue = 3;
		using var context = new RockContext(); 
		var expectations = context.Create<ClassGenericMethodCreateExpectations<int>>();
		expectations.Methods.BarReturn<int>().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.BarReturn<int>();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeWithGenericParameterTypeAsReturn()
	{
		var mock = new ClassGenericMethodMakeExpectations<int>().Instance();
		var value = mock.BarReturn<int>();

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithGenericParameterTypeAsReturnThatDoesNotMatch()
	{
		var returnValue = 3;
		var expectations = new ClassGenericMethodCreateExpectations<int>();
		expectations.Methods.BarReturn<int>().ReturnValue(returnValue);

		var mock = expectations.Instance();

		Assert.That(mock.BarReturn<string>, Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateWithNullableGenericParameterTypes()
	{
		var returnValue = "c";
		using var context = new RockContext(); 
		var expectations = context.Create<ClassGenericMethodCreateExpectations<int>>();
		expectations.Methods.NullableValues<string>("b").ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.NullableValues("b");

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeWithNullableGenericParameterTypes()
	{
		var mock = new ClassGenericMethodMakeExpectations<int>().Instance();
		var value = mock.NullableValues("b");

		Assert.That(value, Is.Null);
	}
}