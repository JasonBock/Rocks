using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class HandleAction2ArgumentTests
	{
		[Test]
		public static void Make()
		{
			var rock = Rock.Create<IHandleAction2ArgumentTests>();
			rock.Handle(_ => _.Target(1, 2));

			var chunk = rock.Make();
			chunk.Target(1, 2);

			rock.Verify();
		}

		[Test]
		public static void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleAction2ArgumentTests>();
			rock.Handle(_ => _.Target(1, 2))
				.Raises(nameof(IHandleAction2ArgumentTests.TargetEvent), EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => wasEventRaised = true;
			chunk.Target(1, 2);

			Assert.That(wasEventRaised, Is.True);
			rock.Verify();
		}

		[Test]
		public static void MakeWithHandler()
		{
			var argumentA = 0;
			var argumentB = 0;

			var rock = Rock.Create<IHandleAction2ArgumentTests>();
			rock.Handle<int, int>(_ => _.Target(1, 2),
				(a, b) => { argumentA = a; argumentB = b; });

			var chunk = rock.Make();
			chunk.Target(1, 2);
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));

			rock.Verify();
		}

		[Test]
		public static void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction2ArgumentTests>();
			rock.Handle(_ => _.Target(1, 2), 2);

			var chunk = rock.Make();
			chunk.Target(1, 2);
			chunk.Target(1, 2);

			rock.Verify();
		}

		[Test]
		public static void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;
			var argumentB = 0;

			var rock = Rock.Create<IHandleAction2ArgumentTests>();
			rock.Handle<int, int>(_ => _.Target(1, 2),
				(a, b) => { argumentA = a; argumentB = b; }, 2);

			var chunk = rock.Make();
			chunk.Target(1, 2);
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			argumentA = 0;
			argumentB = 0;
			chunk.Target(1, 2);
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));

			rock.Verify();
		}
	}

	public interface IHandleAction2ArgumentTests
	{
		event EventHandler TargetEvent;
		void Target(int a, int b);
	}
}