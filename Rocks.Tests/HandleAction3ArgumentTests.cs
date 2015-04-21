using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleAction3ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleAction3ArgumentTests>();
			rock.HandleAction(_ => _.Target(1, 2, 3));

			var chunk = rock.Make();
			chunk.Target(1, 2, 3);

			rock.Verify();
		}

		[Test]
		public void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleAction3ArgumentTests>();
			rock.HandleAction(_ => _.Target(1, 2, 3))
				.Raises(nameof(IHandleAction3ArgumentTests.TargetEvent), EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => wasEventRaised = true;
			chunk.Target(1, 2, 3);

			Assert.IsTrue(wasEventRaised);
			rock.Verify();
		}

		[Test]
		public void MakeWithHandler()
		{
			var argumentA = 0;
			var argumentB = 0;
			var argumentC = 0;

			var rock = Rock.Create<IHandleAction3ArgumentTests>();
			rock.HandleAction<int, int, int>(_ => _.Target(1, 2, 3),
				(a, b, c) => { argumentA = a; argumentB = b; argumentC = c; });

			var chunk = rock.Make();
			chunk.Target(1, 2, 3);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction3ArgumentTests>();
			rock.HandleAction(_ => _.Target(1, 2, 3), 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3);
			chunk.Target(1, 2, 3);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;
			var argumentB = 0;
			var argumentC = 0;

			var rock = Rock.Create<IHandleAction3ArgumentTests>();
			rock.HandleAction<int, int, int>(_ => _.Target(1, 2, 3),
				(a, b, c) => { argumentA = a; argumentB = b; argumentC = c; }, 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			chunk.Target(1, 2, 3);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));

			rock.Verify();
		}
	}

	public interface IHandleAction3ArgumentTests
	{
		event EventHandler TargetEvent;
		void Target(int a, int b, int c);
	}
}