using NUnit.Framework;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class VerificationTests
	{
		[Test]
		public void TryThis()
		{
			var expectations = new ReadOnlyDictionary<string, ArgumentExpectation>(new Dictionary<string, ArgumentExpectation>());
			var handlers = new Dictionary<string, HandlerInformation>();
			handlers.Add("x", new HandlerInformation<int>(expectations) { ReturnValue = 42 });

			var roHandlers = new ReadOnlyDictionary<string, HandlerInformation>(handlers);

			HandlerInformation rvHandler = null;
			roHandlers.TryGetValue("x", out rvHandler);

			Assert.AreEqual(42, (rvHandler as HandlerInformation<int>).ReturnValue);
		}

		[Test]
		public void Verify()
		{
			var rock = Rock.Create<IVerification>();
			rock.HandleAction(_ => _.Target());

			var chunk = rock.Make();
			chunk.Target();

			rock.Verify();
		}

		[Test]
		public void VerifyWhenMethodIsNotCalled()
		{
			var rock = Rock.Create<IVerification>();
			rock.HandleAction(_ => _.Target());

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => rock.Verify());
		}

		[Test]
		public void VerifyWhenExpectedCallCountIsNotSpecifiedAndMethodIsCalledMultipleTimes()
		{
			var rock = Rock.Create<IVerification>();
			rock.HandleAction(_ => _.Target());

			var chunk = rock.Make();
			chunk.Target();
			chunk.Target();

			Assert.DoesNotThrow(() => rock.Verify());
		}

		[Test]
		public void RunWhenMethodIsCalledWithoutHandle()
		{
			var rock = Rock.Create<IVerification>();
			var chunk = rock.Make();

			Assert.Throws<NotImplementedException>(() => chunk.Target());
		}
	}

	public interface IVerification
	{
		void Target();
	}
}
