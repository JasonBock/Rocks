using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleFunc6ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleFunc6ArgumentTests>();
			rock.HandleFunc(_ => _.ReferenceTarget(1, 2, 3, 4, 5, 6));
			rock.HandleFunc(_ => _.ValueTarget(10, 20, 30, 40, 50, 60));

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2, 3, 4, 5, 6);
			chunk.ValueTarget(10, 20, 30, 40, 50, 60);

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
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc6ArgumentTests>();
			rock.HandleFunc<int, int, int, int, int, int, string>(_ => _.ReferenceTarget(1, 2, 3, 4, 5, 6),
				(a, b, c, d, e, f) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; return stringReturnValue; });
			rock.HandleFunc<int, int, int, int, int, int, int>(_ => _.ValueTarget(10, 20, 30, 40, 50, 60),
				(a, b, c, d, e, f) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; return intReturnValue; });
			
			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1, 2, 3, 4, 5, 6), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(5, argumentE, nameof(argumentE));
			Assert.AreEqual(6, argumentF, nameof(argumentF));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			argumentE = 0;
			argumentF = 0;
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10, 20, 30, 40, 50, 60), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));
			Assert.AreEqual(30, argumentC, nameof(argumentC));
			Assert.AreEqual(40, argumentD, nameof(argumentD));
			Assert.AreEqual(50, argumentE, nameof(argumentE));
			Assert.AreEqual(60, argumentF, nameof(argumentF));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleFunc6ArgumentTests>();
			rock.HandleFunc(_ => _.ReferenceTarget(1, 2, 3, 4, 5, 6), 2);
			rock.HandleFunc(_ => _.ValueTarget(10, 20, 30, 40, 50, 60), 2);

			var chunk = rock.Make();
			chunk.ReferenceTarget(1, 2, 3, 4, 5, 6);
			chunk.ReferenceTarget(1, 2, 3, 4, 5, 6);
			chunk.ValueTarget(10, 20, 30, 40, 50, 60);
			chunk.ValueTarget(10, 20, 30, 40, 50, 60);

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
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFunc6ArgumentTests>();
			rock.HandleFunc<int, int, int, int, int, int, string>(_ => _.ReferenceTarget(1, 2, 3, 4, 5, 6),
				(a, b, c, d, e, f) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; return stringReturnValue; }, 2);
			rock.HandleFunc<int, int, int, int, int, int, int>(_ => _.ValueTarget(10, 20, 30, 40, 50, 60),
				(a, b, c, d, e, f) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; argumentE = e; argumentF = f; return intReturnValue; }, 2);

			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1, 2, 3, 4, 5, 6), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(5, argumentE, nameof(argumentE));
			Assert.AreEqual(6, argumentF, nameof(argumentF));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			argumentE = 0;
			argumentF = 0;
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(1, 2, 3, 4, 5, 6), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			Assert.AreEqual(5, argumentE, nameof(argumentE));
			Assert.AreEqual(6, argumentF, nameof(argumentF));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			argumentE = 0;
			argumentF = 0;
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10, 20, 30, 40, 50, 60), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));
			Assert.AreEqual(30, argumentC, nameof(argumentC));
			Assert.AreEqual(40, argumentD, nameof(argumentD));
			Assert.AreEqual(50, argumentE, nameof(argumentE));
			Assert.AreEqual(60, argumentF, nameof(argumentF));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			argumentE = 0;
			argumentF = 0;
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(10, 20, 30, 40, 50, 60), nameof(chunk.ValueTarget));
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));
			Assert.AreEqual(30, argumentC, nameof(argumentC));
			Assert.AreEqual(40, argumentD, nameof(argumentD));
			Assert.AreEqual(50, argumentE, nameof(argumentE));
			Assert.AreEqual(60, argumentF, nameof(argumentF));

			rock.Verify();
		}
	}

	public interface IHandleFunc6ArgumentTests
	{
		string ReferenceTarget(int a, int b, int c, int d, int e, int f);
		int ValueTarget(int a, int b, int c, int d, int e, int f);
	}
}