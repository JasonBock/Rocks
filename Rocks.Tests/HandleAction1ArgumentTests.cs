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
			rock.HandleAction(_ => _.Target(44));

			var chunk = rock.Make();
			chunk.Target(44);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandler()
		{
			var wasCalled = false;

			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.HandleAction<int>(_ => _.Target(44),
				a => wasCalled = true);

			var chunk = rock.Make();
			chunk.Target(44);

			rock.Verify();
			Assert.IsTrue(wasCalled);
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.HandleAction(_ => _.Target(44), 2);

			var chunk = rock.Make();
			chunk.Target(44);
			chunk.Target(44);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var wasCalled = false;

			var rock = Rock.Create<IHandleAction1ArgumentTests>();
			rock.HandleAction<int>(_ => _.Target(44),
				a => wasCalled = true, 2);

			var chunk = rock.Make();
			chunk.Target(44);
			chunk.Target(44);

			rock.Verify();
			Assert.IsTrue(wasCalled);
		}
	}

	public interface IHandleAction1ArgumentTests
	{
		void Target(int a);
	}
}