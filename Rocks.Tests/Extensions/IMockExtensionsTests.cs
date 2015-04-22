using NUnit.Framework;
using static Rocks.Extensions.IMockExtensions;
using System;
using Rocks.Exceptions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class IMockExtensionsTests
	{
		[Test]
		public void GetVerificationFailuresWhenFailuresOccurred()
		{
			var rock = Rock.Create<IMockExtensions>();
			rock.Handle(_ => _.TargetMethod());

			var chunk = rock.Make();

			var failures = (chunk as IMock).GetVerificationFailures();
			Assert.AreEqual(1, failures.Count);
			var failure = failures[0];
			Assert.IsTrue(failure.EndsWith("method: void TargetMethod(), message: " + HandlerInformation.ErrorAtLeastOnceCallCount));
			Assert.IsTrue(failure.StartsWith("Type: Rocks.Tests.Extensions.Rock"));
		}

		[Test]
		public void GetVerificationFailuresWhenNoFailuresOccurred()
		{
			var rock = Rock.Create<IMockExtensions>();
			rock.Handle(_ => _.TargetMethod());

			var chunk = rock.Make();
			chunk.TargetMethod();

			var failures = (chunk as IMock).GetVerificationFailures();
			Assert.AreEqual(0, failures.Count);
		}

		[Test]
		public void Raise()
		{
			var rock = Rock.Create<IMockExtensions>();
			rock.Handle(_ => _.TargetMethod());

			var chunk = rock.Make();
			var uses = new UsesIMockExtensions(chunk, true);
			chunk.Raise(nameof(IMockExtensions.TargetEvent), EventArgs.Empty);

			Assert.IsTrue(uses.WasEventRaised);
		}

		[Test]
		public void RaiseWhenNoHandlersExist()
		{
			var rock = Rock.Create<IMockExtensions>();
			rock.Handle(_ => _.TargetMethod());

			var chunk = rock.Make();
			var uses = new UsesIMockExtensions(chunk, false);
			chunk.Raise(nameof(IMockExtensions.TargetEvent), EventArgs.Empty);

			Assert.IsFalse(uses.WasEventRaised);
		}

		[Test]
		public void VerifyWhenFailuresOccurred()
		{
			var rock = Rock.Create<IMockExtensions>();
			rock.Handle(_ => _.TargetMethod());

			var chunk = rock.Make();

			Assert.Throws<VerificationException>(() => (chunk as IMock).Verify());
		}

		[Test]
		public void VerifyWhenNoFailuresOccurred()
		{
			var rock = Rock.Create<IMockExtensions>();
			rock.Handle(_ => _.TargetMethod());

			var chunk = rock.Make();
			chunk.TargetMethod();

			Assert.DoesNotThrow(() => (chunk as IMock).Verify());
		}
	}

	public interface IMockExtensions
	{
		event EventHandler TargetEvent;
		void TargetMethod();
	}

	public class UsesIMockExtensions
	{
		private IMockExtensions mockExtensions;

		public UsesIMockExtensions(IMockExtensions mockExtensions, bool shouldAddHandler)
		{
			this.mockExtensions = mockExtensions;

			if (shouldAddHandler)
			{
				this.mockExtensions.TargetEvent += (s, e) => this.WasEventRaised = true;
			}
		}

		public bool WasEventRaised { get; private set; }
	}
}
