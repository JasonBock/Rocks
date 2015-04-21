using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleAction5ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleAction5ArgumentTests>();
			rock.HandleAction(_ => _.Target(1, 2, 3, 4, 5));

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5);

			rock.Verify();
		}

		[Test]
		public void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleAction5ArgumentTests>();
			rock.HandleAction(_ => _.Target(1, 2, 3, 4, 5))
				.Raises(nameof(IHandleAction5ArgumentTests.TargetEvent), EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => wasEventRaised = true;
			chunk.Target(1, 2, 3, 4, 5);

			Assert.IsTrue(wasEventRaised);
			rock.Verify();
		}

		[Test]
		public void MakeWithHandler()
		{
			var argumentA = 0;
			var argumentB = 0;
			var argumentC = 0;
			var argumentD = 0;
			var argumentE = 0;

			var rock = Rock.Create<IHandleAction5ArgumentTests>();
			rock.HandleAction<int, int, int, int, int>(_ => _.Target(1, 2, 3, 4, 5),
				(a, b, c, d, e) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; });

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(5, argumentE, nameof(argumentE));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction5ArgumentTests>();
			rock.HandleAction(_ => _.Target(1, 2, 3, 4, 5), 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5);
			chunk.Target(1, 2, 3, 4, 5);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;
			var argumentB = 0;
			var argumentC = 0;
			var argumentD = 0;
			var argumentE = 0;

			var rock = Rock.Create<IHandleAction5ArgumentTests>();
			rock.HandleAction<int, int, int, int, int>(_ => _.Target(1, 2, 3, 4, 5),
				(a, b, c, d, e) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; }, 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(5, argumentE, nameof(argumentE));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			argumentE = 0;
			chunk.Target(1, 2, 3, 4, 5);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(5, argumentE, nameof(argumentE));

			rock.Verify();
		}
	}

	public interface IHandleAction5ArgumentTests
	{
		event EventHandler TargetEvent;
		void Target(int a, int b, int c, int d, int e);
	}
}