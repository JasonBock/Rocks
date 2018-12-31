using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class HandleActionNoArgumentsTests
	{
		[Test]
		public static void Make()
		{
			var rock = Rock.Create<IHandleActionNoArgumentsTests>();
			rock.Handle(_ => _.Target());

			var chunk = rock.Make();
			chunk.Target();

			rock.Verify();
		}

		[Test]
		public static void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleActionNoArgumentsTests>();
			rock.Handle(_ => _.Target())
				.Raises(nameof(IHandleActionNoArgumentsTests.TargetEvent), EventArgs.Empty);

			var wasEventRaised = false;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => wasEventRaised = true;
			chunk.Target();

			Assert.That(wasEventRaised, Is.True);
			rock.Verify();
		}

		[Test]
		public static void MakeWithHandler()
		{
			var wasCalled = false;

			var rock = Rock.Create<IHandleActionNoArgumentsTests>();
			rock.Handle(_ => _.Target(),
				() => wasCalled = true);

			var chunk = rock.Make();
			chunk.Target();

			rock.Verify();
			Assert.That(wasCalled, Is.True);
		}

		[Test]
		public static void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleActionNoArgumentsTests>();
			rock.Handle(_ => _.Target(), 2);

			var chunk = rock.Make();
			chunk.Target();
			chunk.Target();

			rock.Verify();
		}

		[Test]
		public static void MakeWithHandlerAndExpectedCallCount()
		{
			var wasCalled = false;

			var rock = Rock.Create<IHandleActionNoArgumentsTests>();
			rock.Handle(_ => _.Target(),
				() => wasCalled = true, 2);

			var chunk = rock.Make();
			chunk.Target();
			chunk.Target();

			rock.Verify();
			Assert.That(wasCalled, Is.True);
		}
	}

	public interface IHandleActionNoArgumentsTests
	{
		event EventHandler TargetEvent;
		void Target();
	}
}