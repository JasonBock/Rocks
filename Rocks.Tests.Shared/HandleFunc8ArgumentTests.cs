using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleFunc8ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleFunc8ArgumentTests>();
			rock.Handle(_ => _.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8));
			rock.Handle(_ => _.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80));

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8);
			chunk.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80);

			rock.Verify();
		}

		[Test]
		public void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleFunc8ArgumentTests>();
			var referenceAdornment = rock.Handle(_ => _.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8));
			referenceAdornment.Raises(nameof(IHandleFunc8ArgumentTests.TargetEvent), EventArgs.Empty);
			var valueAdornment = rock.Handle(_ => _.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80));
			valueAdornment.Raises(nameof(IHandleFunc8ArgumentTests.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8);
			chunk.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80);

			Assert.That(eventRaisedCount, Is.EqualTo(2));
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
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc8ArgumentTests>();
			rock.Handle<int, int, int, int, int, int, int, int, string>(_ => _.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8),
				(a, b, c, d, e, f, g, h) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; argumentG = g; argumentH = h; return stringReturnValue; });
			rock.Handle<int, int, int, int, int, int, int, int, int>(_ => _.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80),
				(a, b, c, d, e, f, g, h) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; argumentG = g; argumentH = h; return intReturnValue; });
			
			var chunk = rock.Make();
			Assert.That(chunk.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(4), nameof(argumentD));
			Assert.That(argumentE, Is.EqualTo(5), nameof(argumentE));
			Assert.That(argumentF, Is.EqualTo(6), nameof(argumentF));
			Assert.That(argumentG, Is.EqualTo(7), nameof(argumentG));
			Assert.That(argumentH, Is.EqualTo(8), nameof(argumentH));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			argumentE = 0;
			argumentF = 0;
			argumentG = 0;
			argumentH = 0;
			Assert.That(chunk.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));
			Assert.That(argumentA, Is.EqualTo(10), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(20), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(30), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(40), nameof(argumentD));
			Assert.That(argumentE, Is.EqualTo(50), nameof(argumentE));
			Assert.That(argumentF, Is.EqualTo(60), nameof(argumentF));
			Assert.That(argumentG, Is.EqualTo(70), nameof(argumentG));
			Assert.That(argumentH, Is.EqualTo(80), nameof(argumentH));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleFunc8ArgumentTests>();
			rock.Handle(_ => _.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8), 2);
			rock.Handle(_ => _.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80), 2);

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8);
			chunk.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8);
			chunk.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80);
			chunk.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80);

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
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc8ArgumentTests>();
			rock.Handle<int, int, int, int, int, int, int, int, string>(_ => _.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8),
				(a, b, c, d, e, f, g, h) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; argumentG = g; argumentH = h; return stringReturnValue; }, 2);
			rock.Handle<int, int, int, int, int, int, int, int, int>(_ => _.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80),
				(a, b, c, d, e, f, g, h) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; argumentG = g; argumentH = h; return intReturnValue; }, 2);

			var chunk = rock.Make();
			Assert.That(chunk.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(4), nameof(argumentD));
			Assert.That(argumentE, Is.EqualTo(5), nameof(argumentE));
			Assert.That(argumentF, Is.EqualTo(6), nameof(argumentF));
			Assert.That(argumentG, Is.EqualTo(7), nameof(argumentG));
			Assert.That(argumentH, Is.EqualTo(8), nameof(argumentH));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			argumentE = 0;
			argumentF = 0;
			argumentG = 0;
			argumentH = 0;
			Assert.That(chunk.ReferenceTarget(1, 2, 3, 4, 5, 6, 7, 8),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(4), nameof(argumentD));
			Assert.That(argumentE, Is.EqualTo(5), nameof(argumentE));
			Assert.That(argumentF, Is.EqualTo(6), nameof(argumentF));
			Assert.That(argumentG, Is.EqualTo(7), nameof(argumentG));
			Assert.That(argumentH, Is.EqualTo(8), nameof(argumentH));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			argumentE = 0;
			argumentF = 0;
			argumentG = 0;
			argumentH = 0;
			Assert.That(chunk.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));
			Assert.That(argumentA, Is.EqualTo(10), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(20), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(30), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(40), nameof(argumentD));
			Assert.That(argumentE, Is.EqualTo(50), nameof(argumentE));
			Assert.That(argumentF, Is.EqualTo(60), nameof(argumentF));
			Assert.That(argumentG, Is.EqualTo(70), nameof(argumentG));
			Assert.That(argumentH, Is.EqualTo(80), nameof(argumentH));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			argumentE = 0;
			argumentF = 0;
			argumentG = 0;
			argumentH = 0;
			Assert.That(chunk.ValueTarget(10, 20, 30, 40, 50, 60, 70, 80),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));
			Assert.That(argumentA, Is.EqualTo(10), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(20), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(30), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(40), nameof(argumentD));
			Assert.That(argumentE, Is.EqualTo(50), nameof(argumentE));
			Assert.That(argumentF, Is.EqualTo(60), nameof(argumentF));
			Assert.That(argumentG, Is.EqualTo(70), nameof(argumentG));
			Assert.That(argumentH, Is.EqualTo(80), nameof(argumentH));

			rock.Verify();
		}
	}

	public interface IHandleFunc8ArgumentTests
	{
		event EventHandler TargetEvent;
		string ReferenceTarget(int a, int b, int c, int d, int e, int f, int g, int h);
		int ValueTarget(int a, int b, int c, int d, int e, int f, int g, int h);
	}
}