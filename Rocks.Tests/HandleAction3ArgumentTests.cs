using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class HandleAction3ArgumentTests
	{
		[Test]
		public static void Make()
		{
			var rock = Rock.Create<IHandleAction3ArgumentTests>();
			rock.Handle(_ => _.Target(1, 2, 3));

			var chunk = rock.Make();
			chunk.Target(1, 2, 3);

			rock.Verify();
		}

		[Test]
		public static void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleAction3ArgumentTests>();
			rock.Handle(_ => _.Target(1, 2, 3))
				.Raises(nameof(IHandleAction3ArgumentTests.TargetEvent), EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => wasEventRaised = true;
			chunk.Target(1, 2, 3);

			Assert.That(wasEventRaised, Is.True);
			rock.Verify();
		}

		[Test]
		public static void MakeWithHandler()
		{
			var argumentA = 0;
			var argumentB = 0;
			var argumentC = 0;

			var rock = Rock.Create<IHandleAction3ArgumentTests>();
			rock.Handle<int, int, int>(_ => _.Target(1, 2, 3),
				(a, b, c) => { argumentA = a; argumentB = b; argumentC = c; });

			var chunk = rock.Make();
			chunk.Target(1, 2, 3);
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));

			rock.Verify();
		}

		[Test]
		public static void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction3ArgumentTests>();
			rock.Handle(_ => _.Target(1, 2, 3), 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3);
			chunk.Target(1, 2, 3);

			rock.Verify();
		}

		[Test]
		public static void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;
			var argumentB = 0;
			var argumentC = 0;

			var rock = Rock.Create<IHandleAction3ArgumentTests>();
			rock.Handle<int, int, int>(_ => _.Target(1, 2, 3),
				(a, b, c) => { argumentA = a; argumentB = b; argumentC = c; }, 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3);
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			chunk.Target(1, 2, 3);
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));

			rock.Verify();
		}
	}

	public interface IHandleAction3ArgumentTests
	{
		event EventHandler TargetEvent;
		void Target(int a, int b, int c);
	}
}