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
			rock.HandleAction(_ => _.Target(44, 44));

			var chunk = rock.Make();
			chunk.Target(44, 44);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandler()
		{
			var wasCalled = false;

			var rock = Rock.Create<IHandleAction2ArgumentTests>();
			rock.HandleAction<int, int>(_ => _.Target(44, 44),
				(a, b) => wasCalled = true);

			var chunk = rock.Make();
			chunk.Target(44, 44);

			rock.Verify();
			Assert.IsTrue(wasCalled);
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction2ArgumentTests>();
			rock.HandleAction(_ => _.Target(44, 44), 2);

			var chunk = rock.Make();
			chunk.Target(44, 44);
			chunk.Target(44, 44);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var wasCalled = false;

			var rock = Rock.Create<IHandleAction2ArgumentTests>();
			rock.HandleAction<int, int>(_ => _.Target(44, 44),
				(a, b) => wasCalled = true, 2);

			var chunk = rock.Make();
			chunk.Target(44, 44);
			chunk.Target(44, 44);

			rock.Verify();
			Assert.IsTrue(wasCalled);
		}
	}

	public interface IHandleAction2ArgumentTests
	{
		void Target(int a, int b);
	}
}