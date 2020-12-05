using NUnit.Framework;
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

	public static class RefAndOutTests
	{
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