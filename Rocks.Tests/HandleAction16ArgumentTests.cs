using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class HandleAction16ArgumentTests
	{
		[Test]
		public static void Make()
		{
			var rock = Rock.Create<IHandleAction16ArgumentTests>();
			rock.Handle(_ => _.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

			rock.Verify();
		}

		[Test]
		public static void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleAction16ArgumentTests>();
			rock.Handle(_ => _.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16))
				.Raises(nameof(IHandleAction16ArgumentTests.TargetEvent), EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => wasEventRaised = true;
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

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
			var argumentE = 0;
			var argumentF = 0;
			var argumentG = 0;
			var argumentH = 0;
			var argumentI = 0;
			var argumentJ = 0;
			var argumentK = 0;
			var argumentL = 0;
			var argumentM = 0;
			var argumentN = 0;
			var argumentO = 0;
			var argumentP = 0;

			var rock = Rock.Create<IHandleAction16ArgumentTests>();
			rock.Handle<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(_ => _.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16),
				(a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; argumentG = g; argumentH = h; argumentI = i; argumentJ = j; argumentK = k; argumentL = l; argumentM = m; argumentN = n; argumentO = o; argumentP = p; });

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(4), nameof(argumentD));
			Assert.That(argumentE, Is.EqualTo(5), nameof(argumentE));
			Assert.That(argumentF, Is.EqualTo(6), nameof(argumentF));
			Assert.That(argumentG, Is.EqualTo(7), nameof(argumentG));
			Assert.That(argumentH, Is.EqualTo(8), nameof(argumentH));
			Assert.That(argumentI, Is.EqualTo(9), nameof(argumentI));
			Assert.That(argumentJ, Is.EqualTo(10), nameof(argumentJ));
			Assert.That(argumentK, Is.EqualTo(11), nameof(argumentK));
			Assert.That(argumentL, Is.EqualTo(12), nameof(argumentL));
			Assert.That(argumentM, Is.EqualTo(13), nameof(argumentM));
			Assert.That(argumentN, Is.EqualTo(14), nameof(argumentN));
			Assert.That(argumentO, Is.EqualTo(15), nameof(argumentO));
			Assert.That(argumentP, Is.EqualTo(16), nameof(argumentP));

			rock.Verify();
		}

		[Test]
		public static void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction16ArgumentTests>();
			rock.Handle(_ => _.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16), 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

			rock.Verify();
		}

		[Test]
		public static void MakeWithHandlerAndExpectedCallCount()
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
			var argumentM = 0;
			var argumentN = 0;
			var argumentO = 0;
			var argumentP = 0;

			var rock = Rock.Create<IHandleAction16ArgumentTests>();
			rock.Handle<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(_ => _.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16),
				(a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; argumentG = g; argumentH = h; argumentI = i; argumentJ = j; argumentK = k; argumentL = l; argumentM = m; argumentN = n; argumentO = o; argumentP = p; }, 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(4), nameof(argumentD));
			Assert.That(argumentE, Is.EqualTo(5), nameof(argumentE));
			Assert.That(argumentF, Is.EqualTo(6), nameof(argumentF));
			Assert.That(argumentG, Is.EqualTo(7), nameof(argumentG));
			Assert.That(argumentH, Is.EqualTo(8), nameof(argumentH));
			Assert.That(argumentI, Is.EqualTo(9), nameof(argumentI));
			Assert.That(argumentJ, Is.EqualTo(10), nameof(argumentJ));
			Assert.That(argumentK, Is.EqualTo(11), nameof(argumentK));
			Assert.That(argumentL, Is.EqualTo(12), nameof(argumentL));
			Assert.That(argumentM, Is.EqualTo(13), nameof(argumentM));
			Assert.That(argumentN, Is.EqualTo(14), nameof(argumentN));
			Assert.That(argumentO, Is.EqualTo(15), nameof(argumentO));
			Assert.That(argumentP, Is.EqualTo(16), nameof(argumentP));
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
			argumentM = 0;
			argumentN = 0;
			argumentO = 0;
			argumentP = 0;
			chunk.Target(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(4), nameof(argumentD));
			Assert.That(argumentE, Is.EqualTo(5), nameof(argumentE));
			Assert.That(argumentF, Is.EqualTo(6), nameof(argumentF));
			Assert.That(argumentG, Is.EqualTo(7), nameof(argumentG));
			Assert.That(argumentH, Is.EqualTo(8), nameof(argumentH));
			Assert.That(argumentI, Is.EqualTo(9), nameof(argumentI));
			Assert.That(argumentJ, Is.EqualTo(10), nameof(argumentJ));
			Assert.That(argumentK, Is.EqualTo(11), nameof(argumentK));
			Assert.That(argumentL, Is.EqualTo(12), nameof(argumentL));
			Assert.That(argumentM, Is.EqualTo(13), nameof(argumentM));
			Assert.That(argumentN, Is.EqualTo(14), nameof(argumentN));
			Assert.That(argumentO, Is.EqualTo(15), nameof(argumentO));
			Assert.That(argumentP, Is.EqualTo(16), nameof(argumentP));

			rock.Verify();
		}
	}

	public interface IHandleAction16ArgumentTests
	{
		event EventHandler TargetEvent;
		void Target(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l, int m, int n, int o, int p);
	}
}