using NUnit.Framework;
using System;

namespace Rocks.Tests
{
	public static class HandleFuncNoArgumentsTests
	{
		[Test]
		public static void Make()
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
		public static void MakeAndRaiseEvent()
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

			Assert.That(eventRaisedCount, Is.EqualTo(2));
			rock.Verify();
		}

		[Test]
		public static void MakeWithReturnValue()
		{
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFuncNoArgumentsTests>();
			rock.Handle(_ => _.ReferenceTarget()).Returns(stringReturnValue);
			rock.Handle(_ => _.ValueTarget()).Returns(intReturnValue);

			var chunk = rock.Make();
			Assert.That(chunk.ReferenceTarget(),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(chunk.ValueTarget(),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));

			rock.Verify();
		}

		[Test]
		public static void MakeWithHandler()
		{
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFuncNoArgumentsTests>();
			rock.Handle(_ => _.ReferenceTarget(),
				() => { return stringReturnValue; });
			rock.Handle(_ => _.ValueTarget(),
				() => { return intReturnValue; });

			var chunk = rock.Make();
			Assert.That(chunk.ReferenceTarget(),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(chunk.ValueTarget(),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));

			rock.Verify();
		}

		[Test]
		public static void MakeWithExpectedCallCount()
		{
			var rock = Rock.Create<IHandleFuncNoArgumentsTests>();
			rock.Handle(_ => _.ReferenceTarget(), 2);
			rock.Handle(_ => _.ValueTarget(), 2);

			var chunk = rock.Make();
			Assert.That(chunk.ReferenceTarget(),
				Is.EqualTo(default(string)), nameof(chunk.ReferenceTarget));
			Assert.That(chunk.ReferenceTarget(),
				Is.EqualTo(default(string)), nameof(chunk.ReferenceTarget));
			Assert.That(chunk.ValueTarget(),
				Is.EqualTo(default(int)), nameof(chunk.ValueTarget));
			Assert.That(chunk.ValueTarget(),
				Is.EqualTo(default(int)), nameof(chunk.ValueTarget));

			rock.Verify();
		}

		[Test]
		public static void MakeWithExpectedCallCountAndReturnValue()
		{
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFuncNoArgumentsTests>();
			rock.Handle(_ => _.ReferenceTarget(), 2).Returns(stringReturnValue);
			rock.Handle(_ => _.ValueTarget(), 2).Returns(intReturnValue);

			var chunk = rock.Make();
			Assert.That(chunk.ReferenceTarget(),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(chunk.ReferenceTarget(),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(chunk.ValueTarget(),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));
			Assert.That(chunk.ValueTarget(),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));

			rock.Verify();
		}

		[Test]
		public static void MakeWithHandlerAndExpectedCallCount()
		{
			var stringReturnValue = "a";
			var intReturnValue = 1;

			var rock = Rock.Create<IHandleFuncNoArgumentsTests>();
			rock.Handle(_ => _.ReferenceTarget(),
				() => { return stringReturnValue; }, 2);
			rock.Handle(_ => _.ValueTarget(),
				() => { return intReturnValue; }, 2);

			var chunk = rock.Make();
			Assert.That(chunk.ReferenceTarget(),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(chunk.ReferenceTarget(),
				Is.EqualTo(stringReturnValue), nameof(chunk.ReferenceTarget));
			Assert.That(chunk.ValueTarget(),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));
			Assert.That(chunk.ValueTarget(),
				Is.EqualTo(intReturnValue), nameof(chunk.ValueTarget));

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