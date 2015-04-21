using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleAction6ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleAction6ArgumentTests>();
			rock.HandleAction(_ => _.Target(1, 2, 3, 4, 5, 6));

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5, 6);

			rock.Verify();
		}

		[Test]
		public void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleAction6ArgumentTests>();
			var adornment = rock.HandleAction(_ => _.Target(1, 2, 3, 4, 5, 6));
			adornment.Raises(nameof(IHandleAction6ArgumentTests.TargetEvent), EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => wasEventRaised = true;
			chunk.Target(1, 2, 3, 4, 5, 6);

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
			var argumentF = 0;

			var rock = Rock.Create<IHandleAction6ArgumentTests>();
			rock.HandleAction<int, int, int, int, int, int>(_ => _.Target(1, 2, 3, 4, 5, 6),
				(a, b, c, d, e, f) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; });

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5, 6);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(5, argumentE, nameof(argumentE));
			Assert.AreEqual(6, argumentF, nameof(argumentF));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction6ArgumentTests>();
			rock.HandleAction(_ => _.Target(1, 2, 3, 4, 5, 6), 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5, 6);
			chunk.Target(1, 2, 3, 4, 5, 6);

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
			var argumentF = 0;

			var rock = Rock.Create<IHandleAction6ArgumentTests>();
			rock.HandleAction<int, int, int, int, int, int>(_ => _.Target(1, 2, 3, 4, 5, 6),
				(a, b, c, d, e, f) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; }, 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5, 6);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(5, argumentE, nameof(argumentE));
			Assert.AreEqual(6, argumentF, nameof(argumentF));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			argumentE = 0;
			argumentF = 0;
			chunk.Target(1, 2, 3, 4, 5, 6);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(5, argumentE, nameof(argumentE));
			Assert.AreEqual(6, argumentF, nameof(argumentF));

			rock.Verify();
		}
	}

	public interface IHandleAction6ArgumentTests
	{
		event EventHandler TargetEvent;
		void Target(int a, int b, int c, int d, int e, int f);
	}
}