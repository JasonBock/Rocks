using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Rocks.IntegrationTests
{
	public interface IAmAsynchronous
	{
		Task FooAsync();
		Task<int> FooReturnAsync();
	}

	public interface IHaveOptionalArguments
	{
		void Foo(int a, string b = "b", double c = 3.2);
		int this[int a, string b = "b"] { get; }
	}

	public interface IHaveParams
	{
		void Foo(int a, params string[] b);
		int this[int a, params string[] b] { get; }
	}

	public static class MemberScenarioTests
	{
		[Test]
		public static async Task MockAsynchronousMethods()
		{
			const int returnValue = 3;
			var rock = Rock.Create<IAmAsynchronous>();
			rock.Methods().FooAsync().Returns(Task.CompletedTask);
			rock.Methods().FooReturnAsync().Returns(Task.FromResult(returnValue));

			var chunk = rock.Instance();
			await chunk.FooAsync().ConfigureAwait(false);
			var value = await chunk.FooReturnAsync().ConfigureAwait(false);

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
			});
		}

		[Test]
		public static void MockMembersWithOptionalArgumentsSpecified()
		{
			var returnValue = 3;
			var rock = Rock.Create<IHaveOptionalArguments>();
			rock.Methods().Foo(1, "b", 3.2);
			rock.Indexers().Getters().This(1, "b").Returns(returnValue);

			var chunk = rock.Instance();
			chunk.Foo(1);
			var value = chunk[1];

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
			});
		}

		[Test]
		public static void MockMembersWithOptionalArgumentsNotSpecified()
		{
			var returnValue = 3;
			var rock = Rock.Create<IHaveOptionalArguments>();
			rock.Methods().Foo(1, Arg.IsDefault<string>(), Arg.IsDefault<double>());
			rock.Indexers().Getters().This(1, Arg.IsDefault<string>()).Returns(returnValue);

			var chunk = rock.Instance();
			chunk.Foo(1);
			var value = chunk[1];

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
			});
		}

		[Test]
		public static void MockMembersWithParamsArgumentsSpecified()
		{
			var returnValue = 3;
			var rock = Rock.Create<IHaveParams>();
			rock.Methods().Foo(1, new[] { "b" });
			rock.Indexers().Getters().This(1, new[] { "b" }).Returns(returnValue);

			var chunk = rock.Instance();
			chunk.Foo(1, "b");
			var value = chunk[1, "b"];

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
			});
		}

		[Test]
		public static void MockMembersWithParamsArgumentsNotSpecified()
		{
			var returnValue = 3;
			var rock = Rock.Create<IHaveParams>();
			rock.Methods().Foo(1, Array.Empty<string>());
			rock.Indexers().Getters().This(1, Array.Empty<string>()).Returns(returnValue);

			var chunk = rock.Instance();
			chunk.Foo(1);
			var value = chunk[1];

			rock.Verify();

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(returnValue));
			});
		}
	}
}