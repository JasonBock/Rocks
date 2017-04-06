using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class MultipleExpectationsTests
	{
		[Test]
		public void HandleMultiple()
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
		public void HandleMultipleWithDifferentExpectedCallCounts()
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
		public void HandleMultipleAndOneIsMissed()
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
		public void HandleMultipleAndCallIsIncorrect()
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
