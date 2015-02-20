using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleFuncNoArgumentsTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleFuncNoArgumentsTests>();
			rock.Handle(_ => _.ReferenceTarget());
			rock.Handle(_ => _.ValueTarget());

			var chunk = rock.Make();
			chunk.ReferenceTarget();
			chunk.ValueTarget();

			rock.Verify();
		}

		[Test]
		public void MakeWithHandler()
		{
			var wasCalled = false;

			var rock = Rock.Create<IHandleFuncNoArgumentsTests>();
			rock.HandleFunc(_ => _.ReferenceTarget(),
				() => { wasCalled = true; return null; });
			rock.HandleFunc(_ => _.ValueTarget(),
				() => { wasCalled = true; return 0; });

			var chunk = rock.Make();
			chunk.ReferenceTarget();
			chunk.ValueTarget();

			rock.Verify();
			Assert.IsTrue(wasCalled);
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleFuncNoArgumentsTests>();
			rock.Handle(_ => _.ReferenceTarget(), 2);
			rock.Handle(_ => _.ValueTarget(), 2);

			var chunk = rock.Make();
			chunk.ReferenceTarget();
			chunk.ReferenceTarget();
			chunk.ValueTarget();
			chunk.ValueTarget();

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var wasCalled = false;

			var rock = Rock.Create<IHandleFuncNoArgumentsTests>();
			rock.HandleFunc(_ => _.ReferenceTarget(),
				() => { wasCalled = true; return null; }, 2);
			rock.HandleFunc(_ => _.ValueTarget(),
				() => { wasCalled = true; return 0; }, 2);

			var chunk = rock.Make();
			chunk.ReferenceTarget();
			chunk.ReferenceTarget();
			chunk.ValueTarget();
			chunk.ValueTarget();

			rock.Verify();
			Assert.IsTrue(wasCalled);
		}
	}

	public interface IHandleFuncNoArgumentsTests
	{
		string ReferenceTarget();
		int ValueTarget();
	}
}