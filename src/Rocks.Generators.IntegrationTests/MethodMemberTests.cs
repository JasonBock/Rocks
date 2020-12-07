﻿using NUnit.Framework;
using System.Globalization;

namespace Rocks.IntegrationTests
{
	public interface IHaveRefAndOut
	{
		void RefArgument(ref int a);
		void RefArgumentsWithGenerics<T1, T2>(T1 a, ref T2 b);
		void OutArgument(out int a);
		void OutArgumentsWithGenerics<T1, T2>(T1 a, out T2 b);
	}

	public interface IHaveRefReturn
	{
		ref int MethodRefReturn();
		ref int PropertyRefReturn { get; }
		ref int this[int a] { get; }
		ref readonly int MethodRefReadonlyReturn();
		ref readonly int PropertyRefReadonlyReturn { get; }
		ref readonly int this[string a] { get; }
	}

	public interface IHaveIn
	{
		void InArgument(in int a);
		int this[in int a] { get; }
	}

	public static class MethodMemberTests
	{
		[Test]
		public static void MockMethodWithRefReturn()
		{
			var rock = Rock.Create<IHaveRefReturn>();
			rock.Methods().MethodRefReturn().Returns(3);

			var chunk = rock.Instance();
			ref var value = ref chunk.MethodRefReturn();

			rock.Verify();
			Assert.That(value, Is.EqualTo(3));
		}

		[Test]
		public static void MockPropertyWithRefReturn()
		{
			var rock = Rock.Create<IHaveRefReturn>();
			rock.Properties().Getters().PropertyRefReturn().Returns(3);

			var chunk = rock.Instance();
			ref var value = ref chunk.PropertyRefReturn;

			rock.Verify();
			Assert.That(value, Is.EqualTo(3));
		}

		[Test]
		public static void MockIndexerWithRefReturn()
		{
			var rock = Rock.Create<IHaveRefReturn>();
			rock.Indexers().Getters().This(3).Returns(4);

			var chunk = rock.Instance();
			ref var value = ref chunk[3];

			rock.Verify();
			Assert.That(value, Is.EqualTo(4));
		}

		[Test]
		public static void MockMethodWithRefReadonlyReturn()
		{
			var rock = Rock.Create<IHaveRefReturn>();
			rock.Methods().MethodRefReadonlyReturn().Returns(3);

			var chunk = rock.Instance();
			var value = chunk.MethodRefReadonlyReturn();

			rock.Verify();
			Assert.That(value, Is.EqualTo(3));
		}

		[Test]
		public static void MockPropertyWithRefReadonlyReturn()
		{
			var rock = Rock.Create<IHaveRefReturn>();
			rock.Properties().Getters().PropertyRefReadonlyReturn().Returns(3);

			var chunk = rock.Instance();
			var value = chunk.PropertyRefReadonlyReturn;

			rock.Verify();
			Assert.That(value, Is.EqualTo(3));
		}

		[Test]
		public static void MockIndexerWithRefReadonlyReturn()
		{
			var rock = Rock.Create<IHaveRefReturn>();
			rock.Indexers().Getters().This("b").Returns(4);

			var chunk = rock.Instance();
			var value = chunk["b"];

			rock.Verify();
			Assert.That(value, Is.EqualTo(4));
		}

		[Test]
		public static void MockMembersWithInParameters()
		{
			var rock = Rock.Create<IHaveIn>();
			rock.Methods().InArgument(3);
			rock.Indexers().Getters().This(4).Returns(5);

			var chunk = rock.Instance();
			chunk.InArgument(3);
			var value = chunk[4];

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(5));
			});
		}

		[Test]
		public static void MockMemberWithOutParameter()
		{
			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Methods().OutArgument(3);

			var chunk = rock.Instance();
			chunk.OutArgument(out var value);

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(0));
			});
		}

		[Test]
		public static void MockMemberWithOutParameterAndCallback()
		{
			static void OutArgumentCallback(out int a) => a = 4;

			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Methods().OutArgument(3).Callback(OutArgumentCallback);

			var chunk = rock.Instance();
			chunk.OutArgument(out var value);

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(4));
			});
		}

		[Test]
		public static void MockMemberWithOutParameterAndGenerics()
		{
			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Methods().OutArgumentsWithGenerics<int, string>(3, "b");

			var chunk = rock.Instance();
			chunk.OutArgumentsWithGenerics<int, string>(3, out var value);

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.Null);
			});
		}

		[Test]
		public static void MockMemberWithOutParameterAndGenericsAndCallback()
		{
			static void OutArgumentsWithGenericsCallback(int a, out string b) =>
				b = a.ToString(CultureInfo.CurrentCulture);

			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Methods().OutArgumentsWithGenerics<int, string>(3, "b").Callback(OutArgumentsWithGenericsCallback);

			var chunk = rock.Instance();
			chunk.OutArgumentsWithGenerics<int, string>(3, out var value);

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo("3"));
			});
		}

		[Test]
		public static void MockMemberWithRefParameter()
		{
			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Methods().RefArgument(3);

			var chunk = rock.Instance();
			var value = 3;
			chunk.RefArgument(ref value);

			rock.Verify();
		}

		[Test]
		public static void MockMemberWithRefParameterAndCallback()
		{
			static void RefArgumentCallback(ref int a) => a = 4;

			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Methods().RefArgument(3).Callback(RefArgumentCallback);

			var chunk = rock.Instance();
			var value = 3;
			chunk.RefArgument(ref value);

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(4));
			});
		}

		[Test]
		public static void MockMemberWithRefParameterAndGenerics()
		{
			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Methods().RefArgumentsWithGenerics<int, string>(3, "b");

			var chunk = rock.Instance();
			var value = "b";
			chunk.RefArgumentsWithGenerics(3, ref value);

			rock.Verify();
		}

		[Test]
		public static void MockMemberWithRefParameterAndGenericsAndCallback()
		{
			static void RefArgumentsWithGenericsCallback(int a, ref string b) =>
				b = a.ToString(CultureInfo.CurrentCulture);

			var rock = Rock.Create<IHaveRefAndOut>();
			rock.Methods().RefArgumentsWithGenerics<int, string>(3, "b").Callback(RefArgumentsWithGenericsCallback);

			var chunk = rock.Instance();
			var value = "b";
			chunk.RefArgumentsWithGenerics(3, ref value);

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo("3"));
			});
		}
	}
}