using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleAction4ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleAction4ArgumentTests>();
			rock.HandleAction(_ => _.Target(1, 2, 3, 4));

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandler()
		{
			var argumentA = 0;
			var argumentB = 0;
			var argumentC = 0;
			var argumentD = 0;

			var rock = Rock.Create<IHandleAction4ArgumentTests>();
			rock.HandleAction<int, int, int, int>(_ => _.Target(1, 2, 3, 4),
				(a, b, c, d) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; });

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction4ArgumentTests>();
			rock.HandleAction(_ => _.Target(1, 2, 3, 4), 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4);
			chunk.Target(1, 2, 3, 4);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;
			var argumentB = 0;
			var argumentC = 0;
			var argumentD = 0;

			var rock = Rock.Create<IHandleAction4ArgumentTests>();
			rock.HandleAction<int, int, int, int>(_ => _.Target(1, 2, 3, 4),
				(a, b, c, d) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; }, 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			argumentA = 0;
			argumentB = 0;
			argumentC = 0;
			argumentD = 0;
			chunk.Target(1, 2, 3, 4);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));

			rock.Verify();
		}
	}

	public interface IHandleAction4ArgumentTests
	{
		void Target(int a, int b, int c, int d);
	}
}