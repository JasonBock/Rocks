using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class HandleFunc2ArgumentTests
	{
		[Test]
		public static void Make()
		{
			var rock = Rock.Create<IHandleFunc2ArgumentTests>();
			rock.Handle(_ => _.ReferenceTarget(1, 2));
			rock.Handle(_ => _.ValueTarget(10, 20));

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2);
			chunk.ValueTarget(10, 20);

			rock.Verify();
		}

		[Test]
		public static void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleFunc2ArgumentTests>();
			var referenceAdornment = rock.Handle(_ => _.ReferenceTarget(1, 2));
			referenceAdornment.Raises(nameof(IHandleFunc2ArgumentTests.TargetEvent), EventArgs.Empty);
			var valueAdornment = rock.Handle(_ => _.ValueTarget(10, 20));
			valueAdornment.Raises(nameof(IHandleFunc2ArgumentTests.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.ReferenceTarget(1, 2);
			chunk.ValueTarget(10, 20);

			Assert.That(eventRaisedCount, Is.EqualTo(2));
			rock.Verify();
		}

		[Test]
		public static void MakeWithHandler()
		{
			var argumentA = 0;
			var argumentB = 0;
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc2ArgumentTests>();
			rock.Handle<int, int, string>(_ => _.ReferenceTarget(1, 2),
				(a, b) => { argumentA = a; argumentB = b; return stringReturnValue; });
			rock.Handle<int, int, int>(_ => _.ValueTarget(10, 20),
				(a, b) => { argumentA = a; argumentB = b; return intReturnValue; });

			var chunk = rock.Make();
			Assert.That(chunk.ReferenceTarget(1, 2),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(chunk.ValueTarget(10, 20),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));
			Assert.That(argumentA, Is.EqualTo(10), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(20), nameof(argumentB));

			rock.Verify();
		}

		[Test]
		public static void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleFunc2ArgumentTests>();
			rock.Handle(_ => _.ReferenceTarget(1, 2), 2);
			rock.Handle(_ => _.ValueTarget(10, 20), 2);

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2);
			chunk.ReferenceTarget(1, 2);
			chunk.ValueTarget(10, 20);
			chunk.ValueTarget(10, 20);

			rock.Verify();
		}

		[Test]
		public static void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;
			var argumentB = 0;
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc2ArgumentTests>();
			rock.Handle<int, int, string>(_ => _.ReferenceTarget(1, 2),
				(a, b) => { argumentA = a; argumentB = b; return stringReturnValue; }, 2);
			rock.Handle<int, int, int>(_ => _.ValueTarget(10, 20),
				(a, b) => { argumentA = a; argumentB = b; return intReturnValue; }, 2);

			var chunk = rock.Make();
			Assert.That(chunk.ReferenceTarget(1, 2),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			argumentA = 0;
			argumentB = 0;
			Assert.That(chunk.ReferenceTarget(1, 2),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			argumentA = 0;
			argumentB = 0;
			Assert.That(chunk.ValueTarget(10, 20),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));
			Assert.That(argumentA, Is.EqualTo(10), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(20), nameof(argumentB));
			argumentA = 0;
			argumentB = 0;
			Assert.That(chunk.ValueTarget(10, 20),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));
			Assert.That(argumentA, Is.EqualTo(10), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(20), nameof(argumentB));

			rock.Verify();
		}
	}

	public interface IHandleFunc2ArgumentTests
	{
		event EventHandler TargetEvent;
		string ReferenceTarget(int a, int b);
		int ValueTarget(int a, int b);
	}
}