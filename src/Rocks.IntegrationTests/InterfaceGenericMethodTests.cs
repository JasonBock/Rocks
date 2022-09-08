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

public static class InterfaceGenericMethodTests
{
	[Test]
	public static void CreateUsingGenericType()
	{
		var rock = Rock.Create<IInterfaceGenericMethod<int>>();
		rock.Methods().Foo(Arg.Any<List<string>>());

		var chunk = rock.Instance();
		chunk.Foo(new List<string>());

		rock.Verify();
	}

	[Test]
	public static void MakeUsingGenericType()
	{
		var chunk = Rock.Make<IInterfaceGenericMethod<int>>().Instance();

		Assert.That(() => chunk.Foo(new List<string>()), Throws.Nothing);
	}

	[Test]
	public static void CreateWithGenericTypeParameter()
	{
		var rock = Rock.Create<IInterfaceGenericMethod<int>>();
		rock.Methods().Quux(Arg.Any<int>());

		var chunk = rock.Instance();
		chunk.Quux(3);

		rock.Verify();
	}

	[Test]
	public static void MakeWithGenericTypeParameter()
	{
		var chunk = Rock.Make<IInterfaceGenericMethod<int>>().Instance();

		Assert.That(() => chunk.Quux(3), Throws.Nothing);
	}

	[Test]
	public static void CreateWithGenericParameterType()
	{
		var rock = Rock.Create<IInterfaceGenericMethod<int>>();
		rock.Methods().Bar(Arg.Any<int>());

		var chunk = rock.Instance();
		chunk.Bar(3);

		rock.Verify();
	}

	[Test]
	public static void MakeWithGenericParameterType()
	{
		var chunk = Rock.Make<IInterfaceGenericMethod<int>>().Instance();

		Assert.That(() => chunk.Bar(3), Throws.Nothing);
	}

	[Test]
	public static void CreateWithGenericParameterTypeThatDoesNotMatch()
	{
		var rock = Rock.Create<IInterfaceGenericMethod<int>>();
		rock.Methods().Bar(Arg.Any<int>());

		var chunk = rock.Instance();

		Assert.That(() => chunk.Bar("3"), Throws.TypeOf<ExpectationException>());
	}

	[Test]
	public static void CreateUsingGenericTypeAsReturn()
	{
		var returnValue = new List<string>();
		var rock = Rock.Create<IInterfaceGenericMethod<int>>();
		rock.Methods().FooReturn().Returns(returnValue);

		var chunk = rock.Instance();
		var value = chunk.FooReturn();

		rock.Verify();

		Assert.That(value, Is.SameAs(returnValue));
	}

	[Test]
	public static void MakeUsingGenericTypeAsReturn()
	{
		var chunk = Rock.Make<IInterfaceGenericMethod<int>>().Instance();
		var value = chunk.FooReturn();

		Assert.That(value, Is.SameAs(default(List<string>)));
	}

	[Test]
	public static void CreateWithGenericTypeParameterAsReturn()
	{
		var returnValue = 3;
		var rock = Rock.Create<IInterfaceGenericMethod<int>>();
		rock.Methods().QuuxReturn().Returns(returnValue);

		var chunk = rock.Instance();
		var value = chunk.QuuxReturn();

		rock.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeWithGenericTypeParameterAsReturn()
	{
		var chunk = Rock.Make<IInterfaceGenericMethod<int>>().Instance();
		var value = chunk.QuuxReturn();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithGenericParameterTypeAsReturn()
	{
		var returnValue = 3;
		var rock = Rock.Create<IInterfaceGenericMethod<int>>();
		rock.Methods().BarReturn<int>().Returns(returnValue);

		var chunk = rock.Instance();
		var value = chunk.BarReturn<int>();

		rock.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeWithGenericParameterTypeAsReturn()
	{
		var chunk = Rock.Make<IInterfaceGenericMethod<int>>().Instance();
		var value = chunk.BarReturn<int>();

		Assert.That(value, Is.EqualTo(default(int)));
	}

	[Test]
	public static void CreateWithGenericParameterTypeAsReturnThatDoesNotMatch()
	{
		var returnValue = 3;
		var rock = Rock.Create<IInterfaceGenericMethod<int>>();
		rock.Methods().BarReturn<int>().Returns(returnValue);

		var chunk = rock.Instance();

		Assert.That(chunk.BarReturn<string>, Throws.TypeOf<InvalidCastException>());
	}

	[Test]
	public static void CreateWithNullableGenericParameterTypes()
	{
		var returnValue = "c";
		var rock = Rock.Create<IInterfaceGenericMethod<int>>();
		rock.Methods().NullableValues<string>("b").Returns(returnValue);

		var chunk = rock.Instance();
		var value = chunk.NullableValues("b");

		rock.Verify();

		Assert.That(value, Is.EqualTo(returnValue));
	}

	[Test]
	public static void MakeWithNullableGenericParameterTypes()
	{
		var chunk = Rock.Make<IInterfaceGenericMethod<int>>().Instance();
		var value = chunk.NullableValues("b");

		Assert.That(value, Is.EqualTo(default(string)));
	}
}