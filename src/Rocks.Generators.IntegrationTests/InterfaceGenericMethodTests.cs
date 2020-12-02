using NUnit.Framework;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;

namespace Rocks.IntegrationTests
{
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
		public static void MockUsingGenericType()
		{
			var rock = Rock.Create<IInterfaceGenericMethod<int>>();
			rock.Methods().Foo(Arg.Any<List<string>>());

			var chunk = rock.Instance();
			chunk.Foo(new List<string>());

			rock.Verify();
		}

		[Test]
		public static void MockWithGenericTypeParameter()
		{
			var rock = Rock.Create<IInterfaceGenericMethod<int>>();
			rock.Methods().Quux(Arg.Any<int>());

			var chunk = rock.Instance();
			chunk.Quux(3);

			rock.Verify();
		}

		[Test]
		public static void MockWithGenericParameterType()
		{
			var rock = Rock.Create<IInterfaceGenericMethod<int>>();
			rock.Methods().Bar(Arg.Any<int>());

			var chunk = rock.Instance();
			chunk.Bar(3);

			rock.Verify();
		}

		[Test]
		public static void MockWithGenericParameterTypeThatDoesNotMatch()
		{
			var rock = Rock.Create<IInterfaceGenericMethod<int>>();
			rock.Methods().Bar(Arg.Any<int>());

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.Bar("3"), Throws.TypeOf<ExpectationException>());
			});
		}

		[Test]
		public static void MockUsingGenericTypeAsReturn()
		{
			var returnValue = new List<string>();
			var rock = Rock.Create<IInterfaceGenericMethod<int>>();
			rock.Methods().FooReturn().Returns(returnValue);

			var chunk = rock.Instance();
			var value = chunk.FooReturn();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.SameAs(returnValue));
			});
		}

		[Test]
		public static void MockWithGenericTypeParameterAsReturn()
		{
			var returnValue = 3;
			var rock = Rock.Create<IInterfaceGenericMethod<int>>();
			rock.Methods().QuuxReturn().Returns(returnValue);

			var chunk = rock.Instance();
			var value = chunk.QuuxReturn();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
			});
		}

		[Test]
		public static void MockWithGenericParameterTypeAsReturn()
		{
			var returnValue = 3;
			var rock = Rock.Create<IInterfaceGenericMethod<int>>();
			rock.Methods().BarReturn<int>().Returns(returnValue);

			var chunk = rock.Instance();
			var value = chunk.BarReturn<int>();

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
			});
		}

		[Test]
		public static void MockWithGenericParameterTypeAsReturnThatDoesNotMatch()
		{
			var returnValue = 3;
			var rock = Rock.Create<IInterfaceGenericMethod<int>>();
			rock.Methods().BarReturn<int>().Returns(returnValue);

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.BarReturn<string>(), Throws.TypeOf<InvalidCastException>());
			});
		}

		[Test]
		public static void MockWithNullableGenericParameterTypes()
		{
			var returnValue = "c";
			var rock = Rock.Create<IInterfaceGenericMethod<int>>();
			rock.Methods().NullableValues<string>("b").Returns(returnValue);

			var chunk = rock.Instance();
			var value = chunk.NullableValues("b");

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
			});
		}
	}
}