using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class OptionalArgumentsTests
	{
		[Test]
		public void MakeWhenMethodHasOptionalArgumentsAndDefaultValuesAreSpecified()
		{
			var rock = Rock.Create<IHaveOptionalArguments>();
			rock.Handle(_ => _.Target(22, Arg.IsDefault<string>(), 44L));

			var chunk = rock.Make();
			chunk.Target(22);

			rock.Verify();
		}

		[Test]
		public void MakeWhenMethodHasOptionalArgumentsAndDefaultValuesAreNotSpecified()
		{
			var rock = Rock.Create<IHaveOptionalArguments>();
			rock.Handle(_ => _.Target(22, Arg.IsDefault<string>(), Arg.IsDefault<long>()));

			var chunk = rock.Make();
			chunk.Target(22);

			rock.Verify();
		}

		[Test]
		public void MakeWhenMethodHasOptionalArgumentsAndDefaultValuesAreNotSpecifiedAndDifferentValuesAreUsedAtCallSite()
		{
			var rock = Rock.Create<IHaveOptionalArguments>();
			rock.Handle(_ => _.Target(22, Arg.IsDefault<string>(), Arg.IsDefault<long>()));

			var chunk = rock.Make();
			Assert.Throws<ExpectationException>(() => chunk.Target(22, "b", 43));
		}

		[Test]
		public void MakeWhenMethodHasNoOptionalArgumentsAndIsDefaultIsUsed()
		{
			var rock = Rock.Create<IHaveOptionalArguments>();
			Assert.Throws<ExpectationException>(
				() => rock.Handle(_ => _.TargetWithNoOptionalArguments(Arg.IsDefault<int>())));
		}
	}

	public interface IHaveOptionalArguments
	{
		void Target(int a, string b = "b", long c = 44);
		void TargetWithNoOptionalArguments(int a);
	}
}
