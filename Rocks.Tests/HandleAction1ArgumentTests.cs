using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class HandleAction1ArgumentTests
	{
		[Test]
		public static void Make()
		{
			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.Handle(_ => _.Target(1));
			
			var chunk = rock.Make();
			chunk.Target(1);

			rock.Verify();
		}

		[Test]
		public static void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.Handle(_ => _.Target(1))
				.Raises(nameof(IHandleAction1ArgumentTests.TargetEvent), EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => wasEventRaised = true;
			chunk.Target(1);

			Assert.That(wasEventRaised, Is.True);
			rock.Verify();
		}

		[Test]
		public static void MakeWithHandler()
		{
			var argumentA = 0;

			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.Handle<int>(_ => _.Target(1),
				a => argumentA = a);

			var chunk = rock.Make();
			chunk.Target(1);
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));

			rock.Verify();
		}

		[Test]
		public static void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.Handle(_ => _.Target(1), 2);

			var chunk = rock.Make();
			chunk.Target(1);
			chunk.Target(1);

			rock.Verify();
		}

		[Test]
		public static void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;

			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.Handle<int>(_ => _.Target(1),
				a => argumentA = a, 2);

			var chunk = rock.Make();
			chunk.Target(1);
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			argumentA = 0;
			chunk.Target(1);
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));

			rock.Verify();
		}
	}

	public interface IHandleAction1ArgumentTests
	{
		event EventHandler TargetEvent;
		void Target(int a);
	}
}