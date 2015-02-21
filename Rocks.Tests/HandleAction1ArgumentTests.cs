using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleAction1ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.HandleAction(_ => _.Target(default(int)));

			var chunk = rock.Make();
			chunk.Target(1);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandler()
		{
			var argumentA = 0;

			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.HandleAction<int>(_ => _.Target(default(int)),
				a => argumentA = a);

			var chunk = rock.Make();
			chunk.Target(1);
			Assert.AreEqual(1, argumentA, nameof(argumentA));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.HandleAction(_ => _.Target(default(int)), 2);

			var chunk = rock.Make();
			chunk.Target(1);
			chunk.Target(2);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var argumentA = 0;

			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.HandleAction<int>(_ => _.Target(default(int)),
				a => argumentA = a, 2);

			var chunk = rock.Make();
			chunk.Target(1);
			Assert.AreEqual(1, argumentA, nameof(argumentA));
			chunk.Target(2);
			Assert.AreEqual(2, argumentA, nameof(argumentA));

			rock.Verify();
		}
	}

	public interface IHandleAction1ArgumentTests
	{
		void Target(int a);
	}
}