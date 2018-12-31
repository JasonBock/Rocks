using NUnit.Framework;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rocks.Tests
{
	public static class VerificationTests
	{
		[Test]
		public static void TryThis()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var handlers = new Dictionary<string, HandlerInformation>
			{
				{ "x", new HandlerInformation<int>(expectations) { ReturnValue = 42 } }
			};

			var roHandlers = new ReadOnlyDictionary<string, HandlerInformation>(handlers);

			roHandlers.TryGetValue("x", out var rvHandler);

			Assert.That((rvHandler as HandlerInformation<int>).ReturnValue, Is.EqualTo(42));
		}

		[Test]
		public static void Verify()
		{
			var rock = Rock.Create<IVerification>();
			rock.Handle(_ => _.Target());

			var chunk = rock.Make();
			chunk.Target();

			rock.Verify();
		}

		[Test]
		public static void VerifyWhenMethodIsNotCalled()
		{
			var rock = Rock.Create<IVerification>();
			rock.Handle(_ => _.Target());

			var chunk = rock.Make();

			Assert.That(() => rock.Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void VerifyWhenExpectedCallCountIsNotSpecifiedAndMethodIsCalledMultipleTimes()
		{
			var rock = Rock.Create<IVerification>();
			rock.Handle(_ => _.Target());

			var chunk = rock.Make();
			chunk.Target();
			chunk.Target();

			Assert.That(() => rock.Verify(), Throws.Nothing);
		}

		[Test]
		public static void RunWhenMethodIsCalledWithoutHandle()
		{
			var rock = Rock.Create<IVerification>();
			var chunk = rock.Make();

			Assert.That(() => chunk.Target(), Throws.TypeOf<NotImplementedException>());
		}
	}

	public interface IVerification
	{
		void Target();
	}
}
