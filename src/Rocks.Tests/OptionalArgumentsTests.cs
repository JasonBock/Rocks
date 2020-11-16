using NUnit.Framework;
using Rocks.Exceptions;
using System;

namespace Rocks.Tests
{
	public static class OptionalArgumentsTests
	{
		[Test]
		public static void MakeWhenMethodHasOptionalArgumentsAndDefaultValuesAreSpecified()
		{
			var rock = Rock.Create<IHaveOptionalArguments>();
			rock.Handle(_ => _.Target(22, Arg.IsDefault<string>(), 44L, false));

			var chunk = rock.Make();
			chunk.Target(22);

			rock.Verify();
		}

		[Test]
		public static void MakeWhenMethodHasOptionalArgumentsAndDefaultValuesAreNotSpecified()
		{
			var rock = Rock.Create<IHaveOptionalArguments>();
			rock.Handle(_ => _.Target(22, Arg.IsDefault<string>(), Arg.IsDefault<long>(), Arg.IsDefault<bool>()));

			var chunk = rock.Make();
			chunk.Target(22);

			rock.Verify();
		}

		[Test]
		public static void MakeWhenMethodHasOptionalArgumentsAndDefaultValuesAreNotSpecifiedAndDifferentValuesAreUsedAtCallSite()
		{
			var rock = Rock.Create<IHaveOptionalArguments>();
			rock.Handle(_ => _.Target(22, Arg.IsDefault<string>(), Arg.IsDefault<long>(), Arg.IsDefault<bool>()));

			var chunk = rock.Make();
			Assert.That(() => chunk.Target(22, "b", 43), Throws.TypeOf<ExpectationException>());
		}

		[Test]
		public static void MakeWhenMethodHasNoOptionalArgumentsAndIsDefaultIsUsed()
		{
			var rock = Rock.Create<IHaveOptionalArguments>();
			Assert.That(
				() => rock.Handle(_ => _.TargetWithNoOptionalArguments(Arg.IsDefault<int>())),
				Throws.TypeOf<ExpectationException>());
		}

		[Test]
		public static void MakeWhenMethodHasOptionalStructArgumentSetToDefault()
		{
			var g = Guid.NewGuid();
			var rock = Rock.Create<IHaveOptionalArguments>();
			rock.Handle(_ => _.TargetWithOptionalStructAsDefault(g));

			var chunk = rock.Make();
			chunk.TargetWithOptionalStructAsDefault(g);

			rock.Verify();
		}
	}

	public interface IHaveOptionalArguments
	{
		void Target(int a, string b = "b", long c = 44, bool d = false);
		void TargetWithNoOptionalArguments(int a);
		void TargetWithOptionalStructAsDefault(Guid g = default);
	}
}
