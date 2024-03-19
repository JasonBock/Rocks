using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.IntegrationTests;

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
	[RockCreate<IRequest<object>>]
	public static async Task CreateWhenMatchesOccurAsync()
	{
		var requestId = Guid.NewGuid();
		var values = new object();
		var message = new object();
		var result = new object();

		var expectations = new IRequestOfobjectCreateExpectations();
		expectations.Methods.Send(requestId, values).ReturnValue(Task.FromResult(result));
		expectations.ExplicitMethodsForIRequestOfObject.Send(requestId, message).ReturnValue(Task.CompletedTask);

		var mock = expectations.Instance();
		var sendResult = await mock.Send(requestId, values: values).ConfigureAwait(false);
		await mock.Send(requestId, message: message).ConfigureAwait(false);

		Assert.That(sendResult, Is.SameAs(result));

		expectations.Verify();
	}

	[Test]
	[RockCreate<IInterfaceGenericMethod<int>>]
	public static void CreateUsingGenericType()
	{
		var expectations = new IInterfaceGenericMethodOfintCreateExpectations();
		expectations.Methods.Foo(Arg.Any<List<string>>());

		var mock = expectations.Instance();
		mock.Foo([]);

		expectations.Verify();
	}

	[Test]
	[RockMake<IInterfaceGenericMethod<int>>]
	public static void MakeUsingGenericType()
	{
		var mock = new IInterfaceGenericMethodOfintMakeExpectations().Instance();

		Assert.That(() => mock.Foo([]), Throws.Nothing);
	}

	[Test]
	[RockCreate<IInterfaceGenericMethod<int>>]
	public static void CreateWithGenericTypeParameter()
	{
		var expectations = new IInterfaceGenericMethodOfintCreateExpectations();
		expectations.Methods.Quux(Arg.Any<int>());

		var mock = expectations.Instance();
		mock.Quux(3);

		expectations.Verify();
	}

	[Test]
	[RockMake<IInterfaceGenericMethod<int>>]
	public static void MakeWithGenericTypeParameter()
	{
		var mock = new IInterfaceGenericMethodOfintMakeExpectations().Instance();

		Assert.That(() => mock.Quux(3), Throws.Nothing);
	}

	[Test]
	[RockCreate<IInterfaceGenericMethod<int>>]
	public static void CreateWithGenericParameterType()
	{
		var expectations = new IInterfaceGenericMethodOfintCreateExpectations();
		expectations.Methods.Bar(Arg.Any<int>());

		var mock = expectations.Instance();
		mock.Bar(3);

		expectations.Verify();
	}

	[Test]
	[RockMake<IInterfaceGenericMethod<int>>]
	public static void MakeWithGenericParameterType()
	{
		var mock = new IInterfaceGenericMethodOfintMakeExpectations().Instance();

		Assert.That(() => mock.Bar(3), Throws.Nothing);
	}

	[Test]
	[RockCreate<IInterfaceGenericMethod<int>>]
	public static void CreateWithGenericParameterTypeThatDoesNotMatch()
	{
		var expectations = new IInterfaceGenericMethodOfintCreateExpectations();
		expectations.Methods.Bar(Arg.Any<int>());

		var mock = expectations.Instance();

		Assert.That(() => mock.Bar("3"), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	[RockCreate<IInterfaceGenericMethod<int>>]
	public static void CreateUsingGenericTypeAsReturn()
	{
		var returnValue = new List<string>();
		var expectations = new IInterfaceGenericMethodOfintCreateExpectations();
		expectations.Methods.FooReturn().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.FooReturn();

		expectations.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	[RockMake<IInterfaceGenericMethod<int>>]
	public static void MakeUsingGenericTypeAsReturn()
	{
		var mock = new IInterfaceGenericMethodOfintMakeExpectations().Instance();
		var value = mock.FooReturn();

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	[RockCreate<IInterfaceGenericMethod<int>>]
	public static void CreateWithGenericTypeParameterAsReturn()
	{
		var returnValue = 3;
		var expectations = new IInterfaceGenericMethodOfintCreateExpectations();
		expectations.Methods.QuuxReturn().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.QuuxReturn();

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockMake<IInterfaceGenericMethod<int>>]
	public static void MakeWithGenericTypeParameterAsReturn()
	{
		var mock = new IInterfaceGenericMethodOfintMakeExpectations().Instance();
		var value = mock.QuuxReturn();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<IInterfaceGenericMethod<int>>]
	public static void CreateWithGenericParameterTypeAsReturn()
	{
		var returnValue = 3;
		var expectations = new IInterfaceGenericMethodOfintCreateExpectations();
		expectations.Methods.BarReturn<int>().ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.BarReturn<int>();

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockMake<IInterfaceGenericMethod<int>>]
	public static void MakeWithGenericParameterTypeAsReturn()
	{
		var mock = new IInterfaceGenericMethodOfintMakeExpectations().Instance();
		var value = mock.BarReturn<int>();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	[RockCreate<IInterfaceGenericMethod<int>>]
	public static void CreateWithGenericParameterTypeAsReturnThatDoesNotMatch()
	{
		var returnValue = 3;
		var expectations = new IInterfaceGenericMethodOfintCreateExpectations();
		expectations.Methods.BarReturn<int>().ReturnValue(returnValue);

		var mock = expectations.Instance();

		Assert.That(mock.BarReturn<string>, Throws.TypeOf<ExpectationException>());
	}

	[Test]
	[RockCreate<IInterfaceGenericMethod<int>>]
	public static void CreateWithNullableGenericParameterTypes()
	{
		var returnValue = "c";
		var expectations = new IInterfaceGenericMethodOfintCreateExpectations();
		expectations.Methods.NullableValues<string>("b").ReturnValue(returnValue);

		var mock = expectations.Instance();
		var value = mock.NullableValues("b");

		expectations.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	[RockMake<IInterfaceGenericMethod<int>>]
	public static void MakeWithNullableGenericParameterTypes()
	{
		var mock = new IInterfaceGenericMethodOfintMakeExpectations().Instance();
		var value = mock.NullableValues("b");

		Assert.That(value, Is.EqualTo(default(string)));
	}
}