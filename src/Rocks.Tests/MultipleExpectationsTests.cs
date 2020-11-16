using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Tests
{
	public static class MultipleExpectationsTests
	{
		[Test]
		public static void HandleMultiple()
		{
			var rock = Rock.Create<IMultiple>();
			rock.Handle(_ => _.Target("a", 44));
			rock.Handle(_ => _.Target("b", "44"));
			rock.Handle(_ => _.Target("a", "44"));

			var chunk = rock.Make();
			chunk.Target("a", "44");
			chunk.Target("b", "44");
			chunk.Target("a", 44);

			rock.Verify();
		}

		[Test]
		public static void HandleMultipleWithDifferentExpectedCallCounts()
		{
			var rock = Rock.Create<IMultiple>();
			rock.Handle(_ => _.Target("a", 44), 2);
			rock.Handle(_ => _.Target("b", "44"), 3);
			rock.Handle(_ => _.Target("a", "44"), 4);

			var chunk = rock.Make();
			chunk.Target("a", 44);
			chunk.Target("a", 44);
			chunk.Target("b", "44");
			chunk.Target("b", "44");
			chunk.Target("b", "44");
			chunk.Target("a", "44");
			chunk.Target("a", "44");
			chunk.Target("a", "44");
			chunk.Target("a", "44");

			rock.Verify();
		}

		[Test]
		public static void HandleMultipleAndOneIsMissed()
		{
			var rock = Rock.Create<IMultiple>();
			rock.Handle(_ => _.Target("a", 44));
			rock.Handle(_ => _.Target("b", "44"));
			rock.Handle(_ => _.Target("a", "44"));

			var chunk = rock.Make();
			chunk.Target("a", "44");
			chunk.Target("a", 44);

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void HandleMultipleAndCallIsIncorrect()
		{
			var rock = Rock.Create<IMultiple>();
			rock.Handle(_ => _.Target("a", 44));
			rock.Handle(_ => _.Target("b", "44"));
			rock.Handle(_ => _.Target("a", "44"));

			var chunk = rock.Make();
			Assert.That(() => chunk.Target("c", "44"), Throws.TypeOf<ExpectationException>());
		}
	}

	public interface IMultiple
	{
		void Target(string a, object b);
	}
}
