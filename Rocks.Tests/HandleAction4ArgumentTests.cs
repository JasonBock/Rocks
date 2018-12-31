using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class HandleAction4ArgumentTests
	{
		[Test]
		public static void Make()
		{
			var rock = Rock.Create<IHandleAction4ArgumentTests>();
			rock.Handle(_ => _.Target(1, 2, 3, 4));

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4);

			rock.Verify();
		}

		[Test]
		public static void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleAction4ArgumentTests>();
			rock.Handle(_ => _.Target(1, 2, 3, 4))
				.Raises(nameof(IHandleAction4ArgumentTests.TargetEvent), EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => wasEventRaised = true;
			chunk.Target(1, 2, 3, 4);

			Assert.That(wasEventRaised, Is.True);
			rock.Verify();
		}

		[Test]
		public static void MakeWithHandler()
		{
			var argumentA = 0;
			var argumentB = 0;
			var argumentC = 0;
			var argumentD = 0;

			var rock = Rock.Create<IHandleAction4ArgumentTests>();
			rock.Handle<int, int, int, int>(_ => _.Target(1, 2, 3, 4),
				(a, b, c, d) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; });

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4);
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(4), nameof(argumentD));

			rock.Verify();
		}

		[Test]
		public static void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction4ArgumentTests>();
			rock.Handle(_ => _.Target(1, 2, 3, 4), 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4);
			chunk.Target(1, 2, 3, 4);

			rock.Verify();
		}

		[Test]
		public static void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;
			var argumentB = 0;
			var argumentC = 0;
			var argumentD = 0;

			var rock = Rock.Create<IHandleAction4ArgumentTests>();
			rock.Handle<int, int, int, int>(_ => _.Target(1, 2, 3, 4),
				(a, b, c, d) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; }, 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4);
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(4), nameof(argumentD));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			chunk.Target(1, 2, 3, 4);
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(4), nameof(argumentD));

			rock.Verify();
		}
	}

	public interface IHandleAction4ArgumentTests
	{
		event EventHandler TargetEvent;
		void Target(int a, int b, int c, int d);
	}
}