using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleFunc2ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleFunc2ArgumentTests>();
			rock.HandleFunc(_ => _.ReferenceTarget(1, 2));
			rock.HandleFunc(_ => _.ValueTarget(10, 20));

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2);
			chunk.ValueTarget(10, 20);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandler()
		{
			var argumentA = 0;
			var argumentB = 0;
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc2ArgumentTests>();
			rock.HandleFunc<int, int, string>(_ => _.ReferenceTarget(1, 2),
				(a, b) => { argumentA = a; argumentB = b; return stringReturnValue; });
			rock.HandleFunc<int, int, int>(_ => _.ValueTarget(10, 20),
				(a, b) => { argumentA = a; argumentB = b; return intReturnValue; });

			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1, 2), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10, 20), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleFunc2ArgumentTests>();
			rock.HandleFunc(_ => _.ReferenceTarget(1, 2), 2);
			rock.HandleFunc(_ => _.ValueTarget(10, 20), 2);

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2);
			chunk.ReferenceTarget(1, 2);
			chunk.ValueTarget(10, 20);
			chunk.ValueTarget(10, 20);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;
			var argumentB = 0;
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc2ArgumentTests>();
			rock.HandleFunc<int, int, string>(_ => _.ReferenceTarget(1, 2),
				(a, b) => { argumentA = a; argumentB = b; return stringReturnValue; }, 2);
			rock.HandleFunc<int, int, int>(_ => _.ValueTarget(10, 20),
				(a, b) => { argumentA = a; argumentB = b; return intReturnValue; }, 2);

			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1, 2), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			argumentA = 0;
			argumentB = 0;
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1, 2), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			argumentA = 0;
			argumentB = 0;
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10, 20), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));
			argumentA = 0;
			argumentB = 0;
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10, 20), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));

			rock.Verify();
		}
	}

	public interface IHandleFunc2ArgumentTests
	{
		string ReferenceTarget(int a, int b);
		int ValueTarget(int a, int b);
	}
}