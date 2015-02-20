using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class HandleFunc1ArgumentTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Create<IHandleFunc1ArgumentTests>();
			rock.HandleFunc(_ => _.ReferenceTarget(44));
			rock.HandleFunc(_ => _.ValueTarget(44));

			var chunk = rock.Make();
			chunk.ReferenceTarget(44);
			chunk.ValueTarget(44);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandler()
		{
			var wasCalled = false;

			var rock = Rock.Create<IHandleFunc1ArgumentTests>();
			rock.HandleFunc<int, string>(_ => _.ReferenceTarget(44),
				a => { wasCalled = true; return (null as string); });
			rock.HandleFunc<int, int>(_ => _.ValueTarget(44),
				a => { wasCalled = true; return 0; });

			var chunk = rock.Make();
			chunk.ReferenceTarget(44);
			chunk.ValueTarget(44);

			rock.Verify();
			Assert.IsTrue(wasCalled);
		}

		[Test]
		public void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleFunc1ArgumentTests>();
			rock.HandleFunc(_ => _.ReferenceTarget(44), 2);
			rock.HandleFunc(_ => _.ValueTarget(44), 2);

			var chunk = rock.Make();
			chunk.ReferenceTarget(44);
			chunk.ReferenceTarget(44);
			chunk.ValueTarget(44);
			chunk.ValueTarget(44);

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var wasCalled = false;

			var rock = Rock.Create<IHandleFunc1ArgumentTests>();
			rock.HandleFunc<int, string>(_ => _.ReferenceTarget(44),
				a => { wasCalled = true; return null; }, 2);
			rock.HandleFunc<int, int>(_ => _.ValueTarget(44),
				a => { wasCalled = true; return 0; }, 2);

			var chunk = rock.Make();
			chunk.ReferenceTarget(44);
			chunk.ReferenceTarget(44);
			chunk.ValueTarget(44);
			chunk.ValueTarget(44);

			rock.Verify();
			Assert.IsTrue(wasCalled);
		}
	}

	public interface IHandleFunc1ArgumentTests
	{
		string ReferenceTarget(int a);
		int ValueTarget(int a);
	}
}