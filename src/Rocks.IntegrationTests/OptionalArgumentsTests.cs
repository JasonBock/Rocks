using NUnit.Framework;

namespace Rocks.IntegrationTests
{
	public interface IHaveOptionalArguments
	{
		void Foo(int a, string b = "b", double c = 3.2);
		int this[int a, string b = "b"] { get; }
	}

	public interface IHaveOptionalStructDefaultArgument
	{
		void Foo(OptionalDefault value = default);
	}

	public struct OptionalDefault { }

	public static class OptionalArgumentsTests
	{
		[Test]
		public static void CreateMembersWithOptionalDefaultStructArgument()
		{
			var rock = Rock.Create<IHaveOptionalStructDefaultArgument>();
			rock.Methods().Foo(Arg.IsDefault<OptionalDefault>());

			var chunk = rock.Instance();
			chunk.Foo();

			rock.Verify();
		}

		[Test]
		public static void CreateMembersWithOptionalArgumentsSpecified()
		{
			var returnValue = 3;
			var rock = Rock.Create<IHaveOptionalArguments>();
			rock.Methods().Foo(1, "b", 3.2);
			rock.Indexers().Getters().This(1, "b").Returns(returnValue);

			var chunk = rock.Instance();
			chunk.Foo(1);
			var value = chunk[1];

			rock.Verify();

			Assert.That(value, Is.EqualTo(returnValue));
		}

		[Test]
		public static void MakeMembersWithOptionalArguments()
		{
			var chunk = Rock.Make<IHaveOptionalArguments>().Instance();
			var value = chunk[1];

			Assert.Multiple(() =>
			{
				Assert.That(value, Is.EqualTo(default(int)));
				Assert.That(() => chunk.Foo(1), Throws.Nothing);
			});
		}

		[Test]
		public static void CreateMembersWithOptionalArgumentsNotSpecified()
		{
			var returnValue = 3;
			var rock = Rock.Create<IHaveOptionalArguments>();
			rock.Methods().Foo(1, Arg.IsDefault<string>(), Arg.IsDefault<double>());
			rock.Indexers().Getters().This(1, Arg.IsDefault<string>()).Returns(returnValue);

			var chunk = rock.Instance();
			chunk.Foo(1);
			var value = chunk[1];

			rock.Verify();

			Assert.That(value, Is.EqualTo(returnValue));
		}
	}
}