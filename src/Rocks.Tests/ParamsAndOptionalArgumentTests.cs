using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class ParamsAndOptionalArgumentTests
	{
		[Test]
		public static void HandleParams()
		{
			var argumentA = default(int);
			var argumentB = default(string[]);

			var rock = Rock.Create<IParamsAndOptionalArgumentTests>();
			rock.Handle<int, string[]>(_ => _.HasParams(44, "a", "b"),
				(a, b) => { argumentA = a; argumentB = b; });

			var chunk = rock.Make();
			chunk.HasParams(44, "a", "b");

			Assert.That(argumentA, Is.EqualTo(44), nameof(argumentA));
			Assert.That(argumentB!.Length, Is.EqualTo(2), nameof(argumentB.Length));
			Assert.That(argumentB[0], Is.EqualTo("a"), nameof(argumentB));
			Assert.That(argumentB[1], Is.EqualTo("b"), nameof(argumentB));

			rock.Verify();
		}

		[Test]
		public static void HandleOptionalWithDefaultValues()
		{
			var argumentA = default(int);
			var argumentB = default(Guid);
			var argumentC = default(string);
			var argumentD = default(double);

			var rock = Rock.Create<IParamsAndOptionalArgumentTests>();
			rock.Handle<int, Guid, string, double>(_ => _.HasOptionals(44, Guid.Empty, "c", 44),
				(a, b, c, d) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; });

			var chunk = rock.Make();
			chunk.HasOptionals(44, Guid.Empty);

			Assert.That(argumentA, Is.EqualTo(44), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(Guid.Empty), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo("c"), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(44d), nameof(argumentD));

			rock.Verify();
		}

		[Test]
		public static void HandleOptionalWithSpecifiedValues()
		{
			var argumentA = default(int);
			var argumentB = default(Guid);
			var argumentC = default(string);
			var argumentD = default(double);

			var rock = Rock.Create<IParamsAndOptionalArgumentTests>();
			rock.Handle<int, Guid, string, double>(_ => _.HasOptionals(44, Guid.Empty, "d", 55),
				(a, b, c, d) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; });

			var chunk = rock.Make();
			chunk.HasOptionals(44, Guid.Empty, "d", 55);

			Assert.That(argumentA, Is.EqualTo(44), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(Guid.Empty), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo("d"), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(55d), nameof(argumentD));

			rock.Verify();
		}
	}

	public interface IParamsAndOptionalArgumentTests
	{
		void HasParams(int a, params string[] b);
		void HasOptionals(int a, Guid b, string c = "c", double d = 44);
	}
}
