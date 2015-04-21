﻿using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleAction12ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleAction12ArgumentTests>();
			rock.HandleAction(_ => _.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

			rock.Verify();
		}

		[Test]
		public void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleAction12ArgumentTests>();
			rock.HandleAction(_ => _.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12))
				.Raises(nameof(IHandleAction12ArgumentTests.TargetEvent), EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => wasEventRaised = true;
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

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
			var argumentG = 0;
			var argumentH = 0;
			var argumentI = 0;
			var argumentJ = 0;
			var argumentK = 0;
			var argumentL = 0;

			var rock = Rock.Create<IHandleAction12ArgumentTests>();
			rock.HandleAction<int, int, int, int, int, int, int, int, int, int, int, int>(_ => _.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12),
				(a, b, c, d, e, f, g, h, i, j, k, l) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; argumentG = g; argumentH = h; argumentI = i; argumentJ = j; argumentK = k; argumentL = l; });

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(5, argumentE, nameof(argumentE));
			Assert.AreEqual(6, argumentF, nameof(argumentF));
			Assert.AreEqual(7, argumentG, nameof(argumentG));
			Assert.AreEqual(8, argumentH, nameof(argumentH));
			Assert.AreEqual(9, argumentI, nameof(argumentI));
			Assert.AreEqual(10, argumentJ, nameof(argumentJ));
			Assert.AreEqual(11, argumentK, nameof(argumentK));
			Assert.AreEqual(12, argumentL, nameof(argumentL));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction12ArgumentTests>();
			rock.HandleAction(_ => _.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12), 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

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
			var argumentJ = 0;
			var argumentK = 0;
			var argumentL = 0;

			var rock = Rock.Create<IHandleAction12ArgumentTests>();
			rock.HandleAction<int, int, int, int, int, int, int, int, int, int, int, int>(_ => _.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12),
				(a, b, c, d, e, f, g, h, i, j, k, l) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; argumentG = g; argumentH = h; argumentI = i; argumentJ = j; argumentK = k; argumentL = l; }, 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(5, argumentE, nameof(argumentE));
			Assert.AreEqual(6, argumentF, nameof(argumentF));
			Assert.AreEqual(7, argumentG, nameof(argumentG));
			Assert.AreEqual(8, argumentH, nameof(argumentH));
			Assert.AreEqual(9, argumentI, nameof(argumentI));
			Assert.AreEqual(10, argumentJ, nameof(argumentJ));
			Assert.AreEqual(11, argumentK, nameof(argumentK));
			Assert.AreEqual(12, argumentL, nameof(argumentL));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			argumentE = 0;
			argumentF = 0;
			argumentG = 0;
			argumentH = 0;
			argumentI = 0;
			argumentJ = 0;
			argumentK = 0;
			argumentL = 0;
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(5, argumentE, nameof(argumentE));
			Assert.AreEqual(6, argumentF, nameof(argumentF));
			Assert.AreEqual(7, argumentG, nameof(argumentG));
			Assert.AreEqual(8, argumentH, nameof(argumentH));
			Assert.AreEqual(9, argumentI, nameof(argumentI));
			Assert.AreEqual(10, argumentJ, nameof(argumentJ));
			Assert.AreEqual(11, argumentK, nameof(argumentK));
			Assert.AreEqual(12, argumentL, nameof(argumentL));

			rock.Verify();
		}
	}

	public interface IHandleAction12ArgumentTests
	{
		event EventHandler TargetEvent;
		void Target(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l);
	}
}