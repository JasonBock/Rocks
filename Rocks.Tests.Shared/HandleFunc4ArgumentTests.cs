using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleFunc4ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleFunc4ArgumentTests>();
			rock.Handle(_ => _.ReferenceTarget(1, 2, 3, 4));
			rock.Handle(_ => _.ValueTarget(10, 20, 30, 40));

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2, 3, 4);
			chunk.ValueTarget(10, 20, 30, 40);

			rock.Verify();
		}

		[Test]
		public void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleFunc4ArgumentTests>();
			var referenceAdornment = rock.Handle(_ => _.ReferenceTarget(1, 2, 3, 4));
			referenceAdornment.Raises(nameof(IHandleFunc4ArgumentTests.TargetEvent), EventArgs.Empty);
			var valueAdornment = rock.Handle(_ => _.ValueTarget(10, 20, 30, 40));
			valueAdornment.Raises(nameof(IHandleFunc4ArgumentTests.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.ReferenceTarget(1, 2, 3, 4);
			chunk.ValueTarget(10, 20, 30, 40);

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
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc4ArgumentTests>();
			rock.Handle<int, int, int, int, string>(_ => _.ReferenceTarget(1, 2, 3, 4),
				(a, b, c, d) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; return stringReturnValue; });
			rock.Handle<int, int, int, int, int>(_ => _.ValueTarget(10, 20, 30, 40),
				(a, b, c, d) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; return intReturnValue; });
			
			var chunk = rock.Make();
			Assert.That(chunk.ReferenceTarget(1, 2, 3, 4),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(4), nameof(argumentD));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			Assert.That(chunk.ValueTarget(10, 20, 30, 40),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));
			Assert.That(argumentA, Is.EqualTo(10), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(20), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(30), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(40), nameof(argumentD));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleFunc4ArgumentTests>();
			rock.Handle(_ => _.ReferenceTarget(1, 2, 3, 4), 2);
			rock.Handle(_ => _.ValueTarget(10, 20, 30, 40), 2);

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2, 3, 4);
			chunk.ReferenceTarget(1, 2, 3, 4);
			chunk.ValueTarget(10, 20, 30, 40);
			chunk.ValueTarget(10, 20, 30, 40);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;
			var argumentB = 0;
			var argumentC = 0;
			var argumentD = 0;
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc4ArgumentTests>();
			rock.Handle<int, int, int, int, string>(_ => _.ReferenceTarget(1, 2, 3, 4),
				(a, b, c, d) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; return stringReturnValue; }, 2);
			rock.Handle<int, int, int, int, int>(_ => _.ValueTarget(10, 20, 30, 40),
				(a, b, c, d) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; return intReturnValue; }, 2);

			var chunk = rock.Make();
			Assert.That(chunk.ReferenceTarget(1, 2, 3, 4),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(4), nameof(argumentD));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			Assert.That(chunk.ReferenceTarget(1, 2, 3, 4),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(argumentA, Is.EqualTo(1), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(2), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(3), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(4), nameof(argumentD));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			Assert.That(chunk.ValueTarget(10, 20, 30, 40),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));
			Assert.That(argumentA, Is.EqualTo(10), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(20), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(30), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(40), nameof(argumentD));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			Assert.That(chunk.ValueTarget(10, 20, 30, 40),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));
			Assert.That(argumentA, Is.EqualTo(10), nameof(argumentA));
			Assert.That(argumentB, Is.EqualTo(20), nameof(argumentB));
			Assert.That(argumentC, Is.EqualTo(30), nameof(argumentC));
			Assert.That(argumentD, Is.EqualTo(40), nameof(argumentD));

			rock.Verify();
		}
	}

	public interface IHandleFunc4ArgumentTests
	{
		event EventHandler TargetEvent;
		string ReferenceTarget(int a, int b, int c, int d);
		int ValueTarget(int a, int b, int c, int d);
	}
}