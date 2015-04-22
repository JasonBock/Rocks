using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleFunc1ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleFunc1ArgumentTests>();
			rock.Handle(_ => _.ReferenceTarget(1));
			rock.Handle(_ => _.ValueTarget(10));

			var chunk = rock.Make();
			chunk.ReferenceTarget(1);
			chunk.ValueTarget(10);

			rock.Verify();
		}

		[Test]
		public void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleFunc1ArgumentTests>();
			var referenceAdornment = rock.Handle(_ => _.ReferenceTarget(1));
			referenceAdornment.Raises(nameof(IHandleFunc1ArgumentTests.TargetEvent), EventArgs.Empty);
			var valueAdornment = rock.Handle(_ => _.ValueTarget(10));
			valueAdornment.Raises(nameof(IHandleFunc1ArgumentTests.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.ReferenceTarget(1);
			chunk.ValueTarget(10);

			Assert.AreEqual(2, eventRaisedCount);
			rock.Verify();
		}

		[Test]
		public void MakeWithHandler()
		{
			var argumentA = 0;
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc1ArgumentTests>();
			rock.Handle<int, string>(_ => _.ReferenceTarget(1),
				a => { argumentA = a; return stringReturnValue; });
			rock.Handle<int, int>(_ => _.ValueTarget(10),
				a => { argumentA = a; return intReturnValue; });

			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleFunc1ArgumentTests>();
			rock.Handle(_ => _.ReferenceTarget(1), 2);
			rock.Handle(_ => _.ValueTarget(10), 2);

			var chunk = rock.Make();
			chunk.ReferenceTarget(1);
			chunk.ReferenceTarget(1);
			chunk.ValueTarget(10);
			chunk.ValueTarget(10);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc1ArgumentTests>();
			rock.Handle<int, string>(_ => _.ReferenceTarget(1),
				a => { argumentA = a; return stringReturnValue; }, 2);
			rock.Handle<int, int>(_ => _.ValueTarget(10),
				a => { argumentA = a; return intReturnValue; }, 2);

			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			argumentA = 0;
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			argumentA = 0;
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			argumentA = 0;
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));

			rock.Verify();
		}
	}

	public interface IHandleFunc1ArgumentTests
	{
		event EventHandler TargetEvent;
		string ReferenceTarget(int a);
		int ValueTarget(int a);
	}
}