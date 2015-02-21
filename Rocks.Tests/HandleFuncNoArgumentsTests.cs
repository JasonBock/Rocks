using NUnit.Framework;
using System;

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
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFuncNoArgumentsTests>();
			rock.HandleFunc(_ => _.ReferenceTarget(),
				() => { return stringReturnValue; });
			rock.HandleFunc(_ => _.ValueTarget(),
				() => { return intReturnValue; });

			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(), nameof(chunk.ValueTarget));

			rock.Verify();
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
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFuncNoArgumentsTests>();
			rock.HandleFunc(_ => _.ReferenceTarget(),
				() => { return stringReturnValue; }, 2);
			rock.HandleFunc(_ => _.ValueTarget(),
				() => { return intReturnValue; }, 2);

			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(), nameof(chunk.ValueTarget));
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(), nameof(chunk.ValueTarget));

			rock.Verify();
		}
	}

	public interface IHandleFuncNoArgumentsTests
	{
		string ReferenceTarget();
		int ValueTarget();
	}
}