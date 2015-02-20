using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleAction2ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleAction2ArgumentTests>();
			rock.Handle(_ => _.Target(default(int), default(int)));

			var chunk = rock.Make();
			chunk.Target(1, 2);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandler()
		{
			var argumentA = 0;
			var argumentB = 0;

			var rock = Rock.Create<IHandleAction2ArgumentTests>();
			rock.HandleAction<int, int>(_ => _.Target(default(int), default(int)),
				(a, b) => { argumentA = a; argumentB = b; });

			var chunk = rock.Make();
			chunk.Target(1, 2);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction2ArgumentTests>();
			rock.Handle(_ => _.Target(default(int), default(int)), 2);

			var chunk = rock.Make();
			chunk.Target(1, 2);
			chunk.Target(1, 2);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;
			var argumentB = 0;

			var rock = Rock.Create<IHandleAction2ArgumentTests>();
			rock.HandleAction<int, int>(_ => _.Target(default(int), default(int)),
				(a, b) => { argumentA = a; argumentB = b; }, 2);

			var chunk = rock.Make();
			chunk.Target(1, 2);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			Assert.AreEqual(2, argumentB, nameof(argumentB));
			chunk.Target(10, 20);
			Assert.AreEqual(10, argumentA, nameof(argumentA));
			Assert.AreEqual(20, argumentB, nameof(argumentB));

			rock.Verify();
		}
	}

	public interface IHandleAction2ArgumentTests
	{
		void Target(int a, int b);
	}
}