using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleAction9ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleAction9ArgumentTests>();
			rock.HandleAction(_ => _.Target(1, 2, 3, 4, 5, 6, 7, 8, 9));

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9);

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
			var argumentG = 0;
			var argumentH = 0;
			var argumentI = 0;

			var rock = Rock.Create<IHandleAction9ArgumentTests>();
			rock.HandleAction<int, int, int, int, int, int, int, int, int>(_ => _.Target(1, 2, 3, 4, 5, 6, 7, 8, 9),
				(a, b, c, d, e, f, g, h, i) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; argumentG = g; argumentH = h; argumentI = i; });

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(5, argumentE, nameof(argumentE));
			Assert.AreEqual(6, argumentF, nameof(argumentF));
			Assert.AreEqual(7, argumentG, nameof(argumentG));
			Assert.AreEqual(8, argumentH, nameof(argumentH));
			Assert.AreEqual(9, argumentI, nameof(argumentI));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction9ArgumentTests>();
			rock.HandleAction(_ => _.Target(1, 2, 3, 4, 5, 6, 7, 8, 9), 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9);
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9);

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
			var argumentG = 0;
			var argumentH = 0;
			var argumentI = 0;

			var rock = Rock.Create<IHandleAction9ArgumentTests>();
			rock.HandleAction<int, int, int, int, int, int, int, int, int>(_ => _.Target(1, 2, 3, 4, 5, 6, 7, 8, 9),
				(a, b, c, d, e, f, g, h, i) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; argumentG = g; argumentH = h; argumentI = i; }, 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(5, argumentE, nameof(argumentE));
			Assert.AreEqual(6, argumentF, nameof(argumentF));
			Assert.AreEqual(7, argumentG, nameof(argumentG));
			Assert.AreEqual(8, argumentH, nameof(argumentH));
			Assert.AreEqual(9, argumentI, nameof(argumentI));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			argumentE = 0;
			argumentF = 0;
			argumentG = 0;
			argumentH = 0;
			argumentI = 0;
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(5, argumentE, nameof(argumentE));
			Assert.AreEqual(6, argumentF, nameof(argumentF));
			Assert.AreEqual(7, argumentG, nameof(argumentG));
			Assert.AreEqual(8, argumentH, nameof(argumentH));
			Assert.AreEqual(9, argumentI, nameof(argumentI));

			rock.Verify();
		}
	}

	public interface IHandleAction9ArgumentTests
	{
		event EventHandler TargetEvent;
      void Target(int a, int b, int c, int d, int e, int f, int g, int h, int i);
	}
}