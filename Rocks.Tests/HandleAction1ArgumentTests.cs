using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleAction1ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.Handle(_ => _.Target(1));

			var chunk = rock.Make();
			chunk.Target(1);

			rock.Verify();
		}

		[Test]
		public void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.Handle(_ => _.Target(1))
				.Raises(nameof(IHandleAction1ArgumentTests.TargetEvent), EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => wasEventRaised = true;
			chunk.Target(1);

			Assert.IsTrue(wasEventRaised);
			rock.Verify();
		}

		[Test]
		public void MakeWithHandler()
		{
			var argumentA = 0;

			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.Handle<int>(_ => _.Target(1),
				a => argumentA = a);

			var chunk = rock.Make();
			chunk.Target(1);
			Assert.AreEqual(1, argumentA, nameof(argumentA));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.Handle(_ => _.Target(1), 2);

			var chunk = rock.Make();
			chunk.Target(1);
			chunk.Target(1);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;

			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.Handle<int>(_ => _.Target(1),
				a => argumentA = a, 2);

			var chunk = rock.Make();
			chunk.Target(1);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			argumentA = 0;
			chunk.Target(1);
			Assert.AreEqual(1, argumentA, nameof(argumentA));

			rock.Verify();
		}
	}

	public interface IHandleAction1ArgumentTests
	{
		event EventHandler TargetEvent;
		void Target(int a);
	}
}