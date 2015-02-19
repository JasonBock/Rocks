using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleActionNoArgumentsTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleActionNoArgumentsTests>();
			rock.Handle(_ => _.Target());

			var chunk = rock.Make();
			chunk.Target();

			rock.Verify();
		}

		[Test]
		public void MakeWithHandler()
		{
			var wasCalled = false;

			var rock = Rock.Create<IHandleActionNoArgumentsTests>();
			rock.Handle(_ => _.Target(),
				() => wasCalled = true);

			var chunk = rock.Make();
			chunk.Target();

			rock.Verify();
			Assert.IsTrue(wasCalled);
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleActionNoArgumentsTests>(new RockOptions(Microsoft.CodeAnalysis.OptimizationLevel.Debug, true));
			rock.Handle(_ => _.Target(), 2);

			var chunk = rock.Make();
			chunk.Target();
			chunk.Target();

			rock.Verify();
		}

		[Test]
		public void MakeWIthHandlerAndExpectedCallCount()
		{
			var wasCalled = false;

			var rock = Rock.Create<IHandleActionNoArgumentsTests>();
			rock.Handle(_ => _.Target(),
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