using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleFunc4ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleFunc4ArgumentTests>();
			rock.HandleFunc(_ => _.ReferenceTarget(default(int), default(int), default(int), default(int)));
			rock.HandleFunc(_ => _.ValueTarget(default(int), default(int), default(int), default(int)));

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2, 3, 4);
			chunk.ValueTarget(10, 20, 30, 40);

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
			rock.HandleFunc<int, int, int, int, string>(_ => _.ReferenceTarget(default(int), default(int), default(int), default(int)),
				(a, b, c, d) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; return stringReturnValue; });
			rock.HandleFunc<int, int, int, int, int>(_ => _.ValueTarget(default(int), default(int), default(int), default(int)),
				(a, b, c, d) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; return intReturnValue; });
			
			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1, 2, 3, 4), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10, 20, 30, 40), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));
			Assert.AreEqual(30, argumentC, nameof(argumentC));
			Assert.AreEqual(40, argumentD, nameof(argumentD));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleFunc4ArgumentTests>();
			rock.HandleFunc(_ => _.ReferenceTarget(default(int), default(int), default(int), default(int)), 2);
			rock.HandleFunc(_ => _.ValueTarget(default(int), default(int), default(int), default(int)), 2);

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
			rock.HandleFunc<int, int, int, int, string>(_ => _.ReferenceTarget(default(int), default(int), default(int), default(int)),
				(a, b, c, d) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; return stringReturnValue; }, 2);
			rock.HandleFunc<int, int, int, int, int>(_ => _.ValueTarget(default(int), default(int), default(int), default(int)),
				(a, b, c, d) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; return intReturnValue; }, 2);

			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1, 2, 3, 4), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(100, 200, 300, 400), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(100, argumentA, nameof(argumentA));
			Assert.AreEqual(200, argumentB, nameof(argumentB));
			Assert.AreEqual(300, argumentC, nameof(argumentC));
			Assert.AreEqual(400, argumentD, nameof(argumentD));
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10, 20, 30, 40), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));
			Assert.AreEqual(30, argumentC, nameof(argumentC));
			Assert.AreEqual(40, argumentD, nameof(argumentD));
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(1000, 2000, 3000, 4000), nameof(chunk.ValueTarget));
			Assert.AreEqual(1000, argumentA, nameof(argumentA));
			Assert.AreEqual(2000, argumentB, nameof(argumentB));
			Assert.AreEqual(3000, argumentC, nameof(argumentC));
			Assert.AreEqual(4000, argumentD, nameof(argumentD));

			rock.Verify();
		}
	}

	public interface IHandleFunc4ArgumentTests
	{
		string ReferenceTarget(int a, int b, int c, int d);
		int ValueTarget(int a, int b, int c, int d);
	}
}