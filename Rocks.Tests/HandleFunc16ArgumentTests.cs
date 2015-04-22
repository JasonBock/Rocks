using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleFunc16ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleFunc16ArgumentTests>();
			rock.Handle(_ => _.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
			rock.Handle(_ => _.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160));

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
			chunk.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160);

			rock.Verify();
		}

		[Test]
		public void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleFunc16ArgumentTests>();
			var referenceAdornment = rock.Handle(_ => _.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
			referenceAdornment.Raises(nameof(IHandleFunc16ArgumentTests.TargetEvent), EventArgs.Empty);
			var valueAdornment = rock.Handle(_ => _.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160));
			valueAdornment.Raises(nameof(IHandleFunc16ArgumentTests.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
			chunk.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160);

			Assert.AreEqual(2, eventRaisedCount);
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
			var argumentM = 0;
			var argumentN = 0;
			var argumentO = 0;
			var argumentP = 0;
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc16ArgumentTests>();
			rock.Handle<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(_ => _.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16),
				(a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; argumentG = g; argumentH = h; argumentI = i; argumentJ = j; argumentK = k; argumentL = l; argumentM = m; argumentN = n; argumentO = o; argumentP = p; return stringReturnValue; });
			rock.Handle<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(_ => _.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160),
				(a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; argumentG = g; argumentH = h; argumentI = i; argumentJ = j; argumentK = k; argumentL = l; argumentM = m; argumentN = n; argumentO = o; argumentP = p; return intReturnValue; });
			
			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16), nameof(chunk.ReferenceTarget));
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
			Assert.AreEqual(13, argumentM, nameof(argumentM));
			Assert.AreEqual(14, argumentN, nameof(argumentN));
			Assert.AreEqual(15, argumentO, nameof(argumentO));
			Assert.AreEqual(16, argumentP, nameof(argumentP));
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
         Assert.AreEqual(intReturnValue, chunk.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));
			Assert.AreEqual(30, argumentC, nameof(argumentC));
			Assert.AreEqual(40, argumentD, nameof(argumentD));
			Assert.AreEqual(50, argumentE, nameof(argumentE));
			Assert.AreEqual(60, argumentF, nameof(argumentF));
			Assert.AreEqual(70, argumentG, nameof(argumentG));
			Assert.AreEqual(80, argumentH, nameof(argumentH));
			Assert.AreEqual(90, argumentI, nameof(argumentI));
			Assert.AreEqual(100, argumentJ, nameof(argumentJ));
			Assert.AreEqual(110, argumentK, nameof(argumentK));
			Assert.AreEqual(120, argumentL, nameof(argumentL));
			Assert.AreEqual(130, argumentM, nameof(argumentM));
			Assert.AreEqual(140, argumentN, nameof(argumentN));
			Assert.AreEqual(150, argumentO, nameof(argumentO));
			Assert.AreEqual(160, argumentP, nameof(argumentP));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleFunc16ArgumentTests>();
			rock.Handle(_ => _.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16), 2);
			rock.Handle(_ => _.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160), 2);

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
			chunk.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
			chunk.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160);
			chunk.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160);

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
			var argumentM = 0;
			var argumentN = 0;
			var argumentO = 0;
			var argumentP = 0;
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc16ArgumentTests>();
			rock.Handle<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(_ => _.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16),
				(a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; argumentG = g; argumentH = h; argumentI = i; argumentJ = j; argumentK = k; argumentL = l; argumentM = m; argumentN = n; argumentO = o; argumentP = p; return stringReturnValue; }, 2);
			rock.Handle<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(_ => _.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160),
				(a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; argumentG = g; argumentH = h; argumentI = i; argumentJ = j; argumentK = k; argumentL = l; argumentM = m; argumentN = n; argumentO = o; argumentP = p; return intReturnValue; }, 2);

			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16), nameof(chunk.ReferenceTarget));
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
			Assert.AreEqual(13, argumentM, nameof(argumentM));
			Assert.AreEqual(14, argumentN, nameof(argumentN));
			Assert.AreEqual(15, argumentO, nameof(argumentO));
			Assert.AreEqual(16, argumentP, nameof(argumentP));
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
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16), nameof(chunk.ReferenceTarget));
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
			Assert.AreEqual(13, argumentM, nameof(argumentM));
			Assert.AreEqual(14, argumentN, nameof(argumentN));
			Assert.AreEqual(15, argumentO, nameof(argumentO));
			Assert.AreEqual(16, argumentP, nameof(argumentP));
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
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));
			Assert.AreEqual(30, argumentC, nameof(argumentC));
			Assert.AreEqual(40, argumentD, nameof(argumentD));
			Assert.AreEqual(50, argumentE, nameof(argumentE));
			Assert.AreEqual(60, argumentF, nameof(argumentF));
			Assert.AreEqual(70, argumentG, nameof(argumentG));
			Assert.AreEqual(80, argumentH, nameof(argumentH));
			Assert.AreEqual(90, argumentI, nameof(argumentI));
			Assert.AreEqual(100, argumentJ, nameof(argumentJ));
			Assert.AreEqual(110, argumentK, nameof(argumentK));
			Assert.AreEqual(120, argumentL, nameof(argumentL));
			Assert.AreEqual(130, argumentM, nameof(argumentM));
			Assert.AreEqual(140, argumentN, nameof(argumentN));
			Assert.AreEqual(150, argumentO, nameof(argumentO));
			Assert.AreEqual(160, argumentP, nameof(argumentP));
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
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));
			Assert.AreEqual(30, argumentC, nameof(argumentC));
			Assert.AreEqual(40, argumentD, nameof(argumentD));
			Assert.AreEqual(50, argumentE, nameof(argumentE));
			Assert.AreEqual(60, argumentF, nameof(argumentF));
			Assert.AreEqual(70, argumentG, nameof(argumentG));
			Assert.AreEqual(80, argumentH, nameof(argumentH));
			Assert.AreEqual(90, argumentI, nameof(argumentI));
			Assert.AreEqual(100, argumentJ, nameof(argumentJ));
			Assert.AreEqual(110, argumentK, nameof(argumentK));
			Assert.AreEqual(120, argumentL, nameof(argumentL));
			Assert.AreEqual(130, argumentM, nameof(argumentM));
			Assert.AreEqual(140, argumentN, nameof(argumentN));
			Assert.AreEqual(150, argumentO, nameof(argumentO));
			Assert.AreEqual(160, argumentP, nameof(argumentP));

			rock.Verify();
		}
	}

	public interface IHandleFunc16ArgumentTests
	{
		event EventHandler TargetEvent;
		string ReferenceTarget(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l, int m, int n, int o, int p);
		int ValueTarget(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l, int m, int n, int o, int p);
	}
}