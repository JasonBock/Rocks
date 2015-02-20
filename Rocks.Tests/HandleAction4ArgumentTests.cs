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
			rock.Handle(_ => _.Target(default(int), default(int), default(int), default(int)));

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
			rock.HandleAction<int, int, int, int>(_ => _.Target(default(int), default(int), default(int), default(int)),
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
			rock.Handle(_ => _.Target(default(int), default(int), default(int), default(int)), 2);

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
			rock.HandleAction<int, int, int, int>(_ => _.Target(default(int), default(int), default(int), default(int)),
				(a, b, c, d) => { argumentA = a; argumentB = b; argumentC = c; argumentD = d; }, 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3, 4);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			Assert.AreEqual(4, argumentD, nameof(argumentD));
			chunk.Target(10, 20, 30, 40);
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));
			Assert.AreEqual(30, argumentC, nameof(argumentC));
			Assert.AreEqual(40, argumentD, nameof(argumentD));

			rock.Verify();
		}
	}

	public interface IHandleAction4ArgumentTests
	{
		void Target(int a, int b, int c, int d);
	}
}