using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Exceptions;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class VerificationTests
	{
		[Test]
		public void Verify()
		{
			var rock = Rock.Create<IVerification>();
			rock.Handle(_ => _.Target());

			var chunk = rock.Make();
			chunk.Target();

			rock.Verify();
		}

		[Test]
		public void VerifyWhenMethodIsNotCalled()
		{
			var rock = Rock.Create<IVerification>();
			rock.Handle(_ => _.Target());

			var chunk = rock.Make();

			Assert.Throws<RockVerificationException>(() => rock.Verify());
		}

		[Test]
		public void VerifyWhenExpectedCallCountIsInvalid()
		{
			var rock = Rock.Create<IVerification>();
			rock.Handle(_ => _.Target());

			var chunk = rock.Make();
			chunk.Target();
			chunk.Target();

			Assert.Throws<RockVerificationException>(() => rock.Verify());
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
