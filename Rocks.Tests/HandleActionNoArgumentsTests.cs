using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleActionNoArgumentsTests
	{
		public delegate void Target();

		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleActionNoArgumentsTests>();
			rock.HandleAction(_ => _.Target());

			var chunk = rock.Make();
			chunk.Target();

			rock.Verify();
		}

		[Test, Ignore]
		public void MakeWithDelegate()
		{
			var wasCalled = false;
			var @delegate = new Target(() => wasCalled = true);

			var rock = Rock.Create<IHandleActionNoArgumentsTests>();
			rock.HandleAction(_ => _.Target(), @delegate);

			var chunk = rock.Make();
			chunk.Target();

			rock.Verify();
			Assert.IsTrue(wasCalled);
		}

		[Test, Ignore]
		public void MakeWithDelegateAndExpectedCallCount()
		{
			var wasCalled = false;
			var @delegate = new Target(() => wasCalled = true);

			var rock = Rock.Create<IHandleActionNoArgumentsTests>();
			rock.HandleAction(_ => _.Target(), @delegate, 2);

			var chunk = rock.Make();
			chunk.Target();
			chunk.Target();

			rock.Verify();
			Assert.IsTrue(wasCalled);
		}

		[Test]
		public void MakeWithHandler()
		{
			var wasCalled = false;

			var rock = Rock.Create<IHandleActionNoArgumentsTests>();
			rock.HandleAction(_ => _.Target(),
				() => wasCalled = true);

			var chunk = rock.Make();
			chunk.Target();

			rock.Verify();
			Assert.IsTrue(wasCalled);
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleActionNoArgumentsTests>();
			rock.HandleAction(_ => _.Target(), 2);

			var chunk = rock.Make();
			chunk.Target();
			chunk.Target();

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var wasCalled = false;

			var rock = Rock.Create<IHandleActionNoArgumentsTests>();
			rock.HandleAction(_ => _.Target(),
				() => wasCalled = true, 2);

			var chunk = rock.Make();
			chunk.Target();
			chunk.Target();

			rock.Verify();
			Assert.IsTrue(wasCalled);
		}
	}

	public interface IHandleActionNoArgumentsTests
	{
		void Target();
	}
}