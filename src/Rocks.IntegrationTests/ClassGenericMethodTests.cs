using NUnit.Framework;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;

namespace Rocks.IntegrationTests
{
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
			var rock = Rock.Create<ClassGenericMethod<int>>();
			rock.Methods().Foo(Arg.Any<List<string>>());

			var chunk = rock.Instance();
			chunk.Foo(new List<string>());

			rock.Verify();
		}

		[Test]
		public static void MakeUsingGenericType()
		{
			var chunk = Rock.Make<ClassGenericMethod<int>>().Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.Foo(new List<string>()), Throws.Nothing);
			});
		}

		[Test]
		public static void CreateWithGenericTypeParameter()
		{
			var rock = Rock.Create<ClassGenericMethod<int>>();
			rock.Methods().Quux(Arg.Any<int>());

			var chunk = rock.Instance();
			chunk.Quux(3);

			rock.Verify();
		}

		[Test]
		public static void MakeWithGenericTypeParameter()
		{
			var chunk = Rock.Make<ClassGenericMethod<int>>().Instance();
			
			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.Quux(3), Throws.Nothing);
			});
		}

		[Test]
		public static void CreateWithGenericParameterType()
		{
			var rock = Rock.Create<ClassGenericMethod<int>>();
			rock.Methods().Bar(Arg.Any<int>());

			var chunk = rock.Instance();
			chunk.Bar(3);

			rock.Verify();
		}

		[Test]
		public static void MakeWithGenericParameterType()
		{
			var chunk = Rock.Make<ClassGenericMethod<int>>().Instance();
			
			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.Bar(3), Throws.Nothing);
			});
		}

		[Test]
		public static void CreateWithGenericParameterTypeThatDoesNotMatch()
		{
			var rock = Rock.Create<ClassGenericMethod<int>>();
			rock.Methods().Bar(Arg.Any<int>());

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.Bar("3"), Throws.TypeOf<ExpectationException>());
			});
		}

		[Test]
		public static void CreateUsingGenericTypeAsReturn()
		{
			var returnValue = new List<string>();
			var rock = Rock.Create<ClassGenericMethod<int>>();
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
		public static void MakeUsingGenericTypeAsReturn()
		{
			var chunk = Rock.Make<ClassGenericMethod<int>>().Instance();
			var value = chunk.FooReturn();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.SameAs(default(List<string>)));
			});
		}

		[Test]
		public static void CreateWithGenericTypeParameterAsReturn()
		{
			var returnValue = 3;
			var rock = Rock.Create<ClassGenericMethod<int>>();
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
		public static void MakeWithGenericTypeParameterAsReturn()
		{
			var chunk = Rock.Make<ClassGenericMethod<int>>().Instance();
			var value = chunk.QuuxReturn();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}

		[Test]
		public static void CreateWithGenericParameterTypeAsReturn()
		{
			var returnValue = 3;
			var rock = Rock.Create<ClassGenericMethod<int>>();
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
		public static void MakeWithGenericParameterTypeAsReturn()
		{
			var chunk = Rock.Make<ClassGenericMethod<int>>().Instance();
			var value = chunk.BarReturn<int>();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
			});
		}

		[Test]
		public static void CreateWithGenericParameterTypeAsReturnThatDoesNotMatch()
		{
			var returnValue = 3;
			var rock = Rock.Create<ClassGenericMethod<int>>();
			rock.Methods().BarReturn<int>().Returns(returnValue);

			var chunk = rock.Instance();

			Assert.Multiple(() =>
			{
				Assert.That(() => chunk.BarReturn<string>(), Throws.TypeOf<InvalidCastException>());
			});
		}

		[Test]
		public static void CreateWithNullableGenericParameterTypes()
		{
			var returnValue = "c";
			var rock = Rock.Create<ClassGenericMethod<int>>();
			rock.Methods().NullableValues<string>("b").Returns(returnValue);

			var chunk = rock.Instance();
			var value = chunk.NullableValues("b");

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
			});
		}

		[Test]
		public static void MakeWithNullableGenericParameterTypes()
		{
			var chunk = Rock.Make<ClassGenericMethod<int>>().Instance();
			var value = chunk.NullableValues("b");

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(string)));
			});
		}
	}
}