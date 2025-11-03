using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Analysis.IntegrationTests.InterfaceGenericMethodTestTypes;

public interface IInterfaceGenericMethod<T>
{
	void Foo(List<string> values);
	void Quux(T value);
	void Bar<TParam>(TParam value);
	List<string> FooReturn();
	T QuuxReturn();
	TReturn BarReturn<TReturn>();
	TData? NullableValues<TData>(TData? data);
}

public interface IRequest<T>
	where T : class
{
	Task<T> Send(Guid requestId, object values);
	Task Send(Guid requestId, T message);
}

public static class InterfaceGenericMethodTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceGenericMethodCreateExpectations<int>>();
		expectations.Setups.Foo(Arg.Any<List<string>>());

		var mock = expectations.Instance();
		mock.Foo([]);
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var mock = new IInterfaceGenericMethodMakeExpectations<int>().Instance();

		Assert.That(() => mock.Foo([]), Throws.Nothing);
	}

	[Test]
	public static void CreateWithGenericTypeParameter()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceGenericMethodCreateExpectations<int>>();
		expectations.Setups.Quux(Arg.Any<int>());

		var mock = expectations.Instance();
		mock.Quux(3);
	}

	[Test]
	public static void MakeWithGenericTypeParameter()
	{
		var mock = new IInterfaceGenericMethodMakeExpectations<int>().Instance();

		Assert.That(() => mock.Quux(3), Throws.Nothing);
	}

	[Test]
	public static void CreateWithGenericParameterType()
	{
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceGenericMethodCreateExpectations<int>>();
		expectations.Setups.Bar(Arg.Any<int>());

		var mock = expectations.Instance();
		mock.Bar(3);
	}

	[Test]
	public static void MakeWithGenericParameterType()
	{
		var mock = new IInterfaceGenericMethodMakeExpectations<int>().Instance();

		Assert.That(() => mock.Bar(3), Throws.Nothing);
	}

	[Test]
	public static void CreateWithGenericParameterTypeThatDoesNotMatch()
	{
		var expectations = new IInterfaceGenericMethodCreateExpectations<int>();
		expectations.Setups.Bar(Arg.Any<int>());

		var mock = expectations.Instance();

		Assert.That(() => mock.Bar("3"), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateUsingGenericTypeAsReturn()
	{
		var returnValue = new List<string>();
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceGenericMethodCreateExpectations<int>>();
		expectations.Setups.FooReturn().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.FooReturn();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeAsReturn()
	{
		var mock = new IInterfaceGenericMethodMakeExpectations<int>().Instance();
		var value = mock.FooReturn();

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	public static void CreateWithGenericTypeParameterAsReturn()
	{
		var returnValue = 3;
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceGenericMethodCreateExpectations<int>>();
		expectations.Setups.QuuxReturn().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.QuuxReturn();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeWithGenericTypeParameterAsReturn()
	{
		var mock = new IInterfaceGenericMethodMakeExpectations<int>().Instance();
		var value = mock.QuuxReturn();

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithGenericParameterTypeAsReturn()
	{
		var returnValue = 3;
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceGenericMethodCreateExpectations<int>>();
		expectations.Setups.BarReturn<int>().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.BarReturn<int>();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeWithGenericParameterTypeAsReturn()
	{
		var mock = new IInterfaceGenericMethodMakeExpectations<int>().Instance();
		var value = mock.BarReturn<int>();

		Assert.That(value, Is.Default);
	}

	[Test]
	public static void CreateWithGenericParameterTypeAsReturnThatDoesNotMatch()
	{
		var returnValue = 3;
		var expectations = new IInterfaceGenericMethodCreateExpectations<int>();
		expectations.Setups.BarReturn<int>().ReturnValue(returnValue);

		var mock = expectations.Instance();

		Assert.That(mock.BarReturn<string>, Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateWithNullableGenericParameterTypes()
	{
		var returnValue = "c";
		using var context = new RockContext(); 
		var expectations = context.Create<IInterfaceGenericMethodCreateExpectations<int>>();
		expectations.Setups.NullableValues<string>("b").ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.NullableValues("b");

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeWithNullableGenericParameterTypes()
	{
		var mock = new IInterfaceGenericMethodMakeExpectations<int>().Instance();
		var value = mock.NullableValues("b");

		Assert.That(value, Is.Null);
	}
}