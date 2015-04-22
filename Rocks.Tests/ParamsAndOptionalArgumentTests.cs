using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class ParamsAndOptionalArgumentTests
	{
		[Test]
		public void HandleParams()
		{
			var argumentA = default(int);
			var argumentB = default(string[]);

			var rock = Rock.Create<IParamsAndOptionalArgumentTests>();
			rock.Handle<int, string[]>(_ => _.HasParams(44, "a", "b"),
				(a, b) => { argumentA = a; argumentB = b; });

			var chunk = rock.Make();
			chunk.HasParams(44, "a", "b");

			Assert.AreEqual(44, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB.Length, nameof(argumentB.Length));
			Assert.AreEqual("a", argumentB[0], nameof(argumentB));
			Assert.AreEqual("b", argumentB[1], nameof(argumentB));

			rock.Verify();
		}

		[Test]
		public void HandleOptionalWithDefaultValues()
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

			Assert.AreEqual(44, argumentA, nameof(argumentA));
			Assert.AreEqual(Guid.Empty, argumentB, nameof(argumentB));
			Assert.AreEqual("c", argumentC, nameof(argumentC));
			Assert.AreEqual(44d, argumentD, nameof(argumentD));

			rock.Verify();
		}

		[Test]
		public void HandleOptionalWithSpecifiedValues()
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

			Assert.AreEqual(44, argumentA, nameof(argumentA));
			Assert.AreEqual(Guid.Empty, argumentB, nameof(argumentB));
			Assert.AreEqual("d", argumentC, nameof(argumentC));
			Assert.AreEqual(55d, argumentD, nameof(argumentD));

			rock.Verify();
		}
	}

	public interface IParamsAndOptionalArgumentTests
	{
		void HasParams(int a, params string[] b);
		void HasOptionals(int a, Guid b, string c = "c", double d = 44);
	}
}
