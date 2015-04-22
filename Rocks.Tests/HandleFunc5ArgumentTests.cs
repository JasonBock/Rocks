using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleFunc5ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleFunc5ArgumentTests>();
			rock.Handle(_ => _.ReferenceTarget(1, 2, 3, 4, 5));
			rock.Handle(_ => _.ValueTarget(10, 20, 30, 40, 50));

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2, 3, 4, 5);
			chunk.ValueTarget(10, 20, 30, 40, 50);

			rock.Verify();
		}

		[Test]
		public void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleFunc5ArgumentTests>();
			var referenceAdornment = rock.Handle(_ => _.ReferenceTarget(1, 2, 3, 4, 5));
			referenceAdornment.Raises(nameof(IHandleFunc5ArgumentTests.TargetEvent), EventArgs.Empty);
			var valueAdornment = rock.Handle(_ => _.ValueTarget(10, 20, 30, 40, 50));
			valueAdornment.Raises(nameof(IHandleFunc5ArgumentTests.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.ReferenceTarget(1, 2, 3, 4, 5);
			chunk.ValueTarget(10, 20, 30, 40, 50);

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
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc5ArgumentTests>();
			rock.Handle<int, int, int, int, int, string>(_ => _.ReferenceTarget(1, 2, 3, 4, 5),
				(a, b, c, d, e) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; return stringReturnValue; });
			rock.Handle<int, int, int, int, int, int>(_ => _.ValueTarget(10, 20, 30, 40, 50),
				(a, b, c, d, e) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; return intReturnValue; });
			
			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1, 2, 3, 4, 5), nameof(chunk.ReferenceTarget));
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
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10, 20, 30, 40, 50), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));
			Assert.AreEqual(30, argumentC, nameof(argumentC));
			Assert.AreEqual(40, argumentD, nameof(argumentD));
			Assert.AreEqual(50, argumentE, nameof(argumentE));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleFunc5ArgumentTests>();
			rock.Handle(_ => _.ReferenceTarget(1, 2, 3, 4, 5), 2);
			rock.Handle(_ => _.ValueTarget(10, 20, 30, 40, 50), 2);

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2, 3, 4, 5);
			chunk.ReferenceTarget(1, 2, 3, 4, 5);
			chunk.ValueTarget(10, 20, 30, 40, 50);
			chunk.ValueTarget(10, 20, 30, 40, 50);

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
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc5ArgumentTests>();
			rock.Handle<int, int, int, int, int, string>(_ => _.ReferenceTarget(1, 2, 3, 4, 5),
				(a, b, c, d, e) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; return stringReturnValue; }, 2);
			rock.Handle<int, int, int, int, int, int>(_ => _.ValueTarget(10, 20, 30, 40, 50),
				(a, b, c, d, e) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; return intReturnValue; }, 2);

			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1, 2, 3, 4, 5), nameof(chunk.ReferenceTarget));
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
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1, 2, 3, 4, 5), nameof(chunk.ReferenceTarget));
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
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10, 20, 30, 40, 50), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));
			Assert.AreEqual(30, argumentC, nameof(argumentC));
			Assert.AreEqual(40, argumentD, nameof(argumentD));
			Assert.AreEqual(50, argumentE, nameof(argumentE));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			argumentE = 0;
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10, 20, 30, 40, 50), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));
			Assert.AreEqual(30, argumentC, nameof(argumentC));
			Assert.AreEqual(40, argumentD, nameof(argumentD));
			Assert.AreEqual(50, argumentE, nameof(argumentE));

			rock.Verify();
		}
	}

	public interface IHandleFunc5ArgumentTests
	{
		event EventHandler TargetEvent;
		string ReferenceTarget(int a, int b, int c, int d, int e);
		int ValueTarget(int a, int b, int c, int d, int e);
	}
}