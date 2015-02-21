using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleAction3ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleAction3ArgumentTests>();
			rock.HandleAction(_ => _.Target(default(int), default(int), default(int)));

			var chunk = rock.Make();
			chunk.Target(1, 2, 3);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandler()
		{
			var argumentA = 0;
			var argumentB = 0;
			var argumentC = 0;

			var rock = Rock.Create<IHandleAction3ArgumentTests>();
			rock.HandleAction<int, int, int>(_ => _.Target(default(int), default(int), default(int)),
				(a, b, c) => { argumentA = a; argumentB = b; argumentC = c; });

			var chunk = rock.Make();
			chunk.Target(1, 2, 3);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction3ArgumentTests>();
			rock.HandleAction(_ => _.Target(default(int), default(int), default(int)), 2);

			var chunk = rock.Make();
			chunk.Target(44, 44, 44);
			chunk.Target(44, 44, 44);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;
			var argumentB = 0;
			var argumentC = 0;

			var rock = Rock.Create<IHandleAction3ArgumentTests>();
			rock.HandleAction<int, int, int>(_ => _.Target(default(int), default(int), default(int)),
				(a, b, c) => { argumentA = a; argumentB = b; argumentC = c; }, 2);

			var chunk = rock.Make();
			chunk.Target(1, 2, 3);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			Assert.AreEqual(3, argumentC, nameof(argumentC));
			chunk.Target(10, 20, 30);
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));
			Assert.AreEqual(30, argumentC, nameof(argumentC));

			rock.Verify();
		}
	}

	public interface IHandleAction3ArgumentTests
	{
		void Target(int a, int b, int c);
	}
}