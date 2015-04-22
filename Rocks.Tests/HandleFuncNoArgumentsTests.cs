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
		public void MakeAndRaiseEvent()
		{
			var rock = Rock.Create<IHandleFuncNoArgumentsTests>();
			var referenceAdornment = rock.Handle(_ => _.ReferenceTarget());
			referenceAdornment.Raises(nameof(IHandleFuncNoArgumentsTests.TargetEvent), EventArgs.Empty);
			var valueAdornment = rock.Handle(_ => _.ValueTarget());
			valueAdornment.Raises(nameof(IHandleFuncNoArgumentsTests.TargetEvent), EventArgs.Empty);

			var eventRaisedCount = 0;
			var chunk = rock.Make();
			chunk.TargetEvent += (s, e) => eventRaisedCount++;
			chunk.ReferenceTarget();
			chunk.ValueTarget();

			Assert.AreEqual(2, eventRaisedCount);
			rock.Verify();
		}

		[Test]
		public void MakeWithReturnValue()
		{
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFuncNoArgumentsTests>();
			rock.Handle(_ => _.ReferenceTarget()).Returns(stringReturnValue);
			rock.Handle(_ => _.ValueTarget()).Returns(intReturnValue);

			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(), nameof(chunk.ValueTarget));

			rock.Verify();
		}

		[Test]
		public void MakeWithHandler()
		{
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFuncNoArgumentsTests>();
			rock.Handle(_ => _.ReferenceTarget(),
				() => { return stringReturnValue; });
			rock.Handle(_ => _.ValueTarget(),
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
			Assert.AreEqual(default(string), chunk.ReferenceTarget(), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(default(string), chunk.ReferenceTarget(), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(default(int), chunk.ValueTarget(), nameof(chunk.ValueTarget));
			Assert.AreEqual(default(int), chunk.ValueTarget(), nameof(chunk.ValueTarget));

			rock.Verify();
		}

		[Test]
		public void MakeWithExpectedCallCountAndReturnValue()
		{
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFuncNoArgumentsTests>();
			rock.Handle(_ => _.ReferenceTarget(), 2).Returns(stringReturnValue);
			rock.Handle(_ => _.ValueTarget(), 2).Returns(intReturnValue);

			var chunk = rock.Make();
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(stringReturnValue, chunk.ReferenceTarget(), nameof(chunk.ReferenceTarget));
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(), nameof(chunk.ValueTarget));
			Assert.AreEqual(intReturnValue, chunk.ValueTarget(), nameof(chunk.ValueTarget));

			rock.Verify();
		}

		[Test]
		public void MakeWithHandlerAndExpectedCallCount()
		{
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFuncNoArgumentsTests>();
			rock.Handle(_ => _.ReferenceTarget(),
				() => { return stringReturnValue; }, 2);
			rock.Handle(_ => _.ValueTarget(),
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
		event EventHandler TargetEvent;
		string ReferenceTarget();
		int ValueTarget();
	}
}