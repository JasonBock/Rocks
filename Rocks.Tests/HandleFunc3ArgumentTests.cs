using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class HandleFunc3ArgumentTests
	{
		[Test]
		public static void Make()
		{
			var rock = Rock.Create<IHandleFunc3ArgumentTests>();
			rock.Handle(_ => _.ReferenceTarget(1, 2, 3));
			rock.Handle(_ => _.ValueTarget(10, 20, 30));

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2, 3);
			chunk.ValueTarget(10, 20, 30);

			rock.Verify();
		}

		[Test]
		public static void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleFunc3ArgumentTests>();
			var referenceAdornment = rock.Handle(_ => _.ReferenceTarget(1, 2, 3));
			referenceAdornment.Raises(nameof(IHandleFunc3ArgumentTests.TargetEvent), EventArgs.Empty);
			var valueAdornment = rock.Handle(_ => _.ValueTarget(10, 20, 30));
			valueAdornment.Raises(nameof(IHandleFunc3ArgumentTests.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.ReferenceTarget(1, 2, 3);
			chunk.ValueTarget(10, 20, 30);

			Assert.That(eventRaisedCount, Is.EqualTo(2));
			rock.Verify();
		}

		[Test]
		public static void MakeWithHandler()
		{
			var argumentA = 0;
			var argumentB = 0;
			var argumentC = 0;
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc3ArgumentTests>();
			rock.Handle<int, int, int, string>(_ => _.ReferenceTarget(1, 2, 3),
				(a, b, c) => { argumentA = a; argumentB = b; argumentC = c; return stringReturnValue; });
			rock.Handle<int, int, int, int>(_ => _.ValueTarget(10, 20, 30),
				(a, b, c) => { argumentA = a; argumentB = b; argumentC = c; return intReturnValue; });
			
			var chunk = rock.Make();
			Assert.That(chunk.ReferenceTarget(1, 2, 3),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));
			Assert.That(chunk.ValueTarget(10, 20, 30),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));
			Assert.That(argumentA, Is.EqualTo(10), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(20), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(30), nameof(argumentC));

			rock.Verify();
		}

		[Test]
		public static void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleFunc3ArgumentTests>();
			rock.Handle(_ => _.ReferenceTarget(1, 2, 3), 2);
			rock.Handle(_ => _.ValueTarget(10, 20, 30), 2);

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2, 3);
			chunk.ReferenceTarget(1, 2, 3);
			chunk.ValueTarget(10, 20, 30);
			chunk.ValueTarget(10, 20, 30);

			rock.Verify();
		}

		[Test]
		public static void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;
			var argumentB = 0;
			var argumentC = 0;
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc3ArgumentTests>();
			rock.Handle<int, int, int, string>(_ => _.ReferenceTarget(1, 2, 3),
				(a, b, c) => { argumentA = a; argumentB = b; argumentC = c; return stringReturnValue; }, 2);
			rock.Handle<int, int, int, int>(_ => _.ValueTarget(10, 20, 30),
				(a, b, c) => { argumentA = a; argumentB = b; argumentC = c; return intReturnValue; }, 2);

			var chunk = rock.Make();
			Assert.That(chunk.ReferenceTarget(1, 2, 3),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			Assert.That(chunk.ReferenceTarget(1, 2, 3),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			Assert.That(chunk.ValueTarget(10, 20, 30),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));
			Assert.That(argumentA, Is.EqualTo(10), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(20), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(30), nameof(argumentC));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			Assert.That(chunk.ValueTarget(10, 20, 30),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));
			Assert.That(argumentA, Is.EqualTo(10), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(20), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(30), nameof(argumentC));

			rock.Verify();
		}
	}

	public interface IHandleFunc3ArgumentTests
	{
		event EventHandler TargetEvent;
		string ReferenceTarget(int a, int b, int c);
		int ValueTarget(int a, int b, int c);
	}
}