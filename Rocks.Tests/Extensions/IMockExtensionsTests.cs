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
			Assert.That(failures.Count, Is.EqualTo(1));
			var failure = failures[0];
			Assert.That(failure.EndsWith("method: void TargetMethod(), message: " + HandlerInformation.ErrorAtLeastOnceCallCount), Is.True);
			Assert.That(failure.StartsWith("Type: Rocks.Tests.Extensions.Rock"), Is.True);
		}

		[Test]
		public void GetVerificationFailuresWhenNoFailuresOccurred()
		{
			var rock = Rock.Create<IMockExtensions>();
			rock.Handle(_ => _.TargetMethod());

			var chunk = rock.Make();
			chunk.TargetMethod();

			var failures = (chunk as IMock).GetVerificationFailures();
			Assert.That(failures.Count, Is.EqualTo(0));
		}

		[Test]
		public void Raise()
		{
			var rock = Rock.Create<IMockExtensions>();
			rock.Handle(_ => _.TargetMethod());

			var chunk = rock.Make();
			var uses = new UsesIMockExtensions(chunk, true);
			chunk.Raise(nameof(IMockExtensions.TargetEvent), EventArgs.Empty);

			Assert.That(uses.WasEventRaised, Is.True);
		}

		[Test]
		public void RaiseWhenNoHandlersExist()
		{
			var rock = Rock.Create<IMockExtensions>();
			rock.Handle(_ => _.TargetMethod());

			var chunk = rock.Make();
			var uses = new UsesIMockExtensions(chunk, false);
			chunk.Raise(nameof(IMockExtensions.TargetEvent), EventArgs.Empty);

			Assert.That(uses.WasEventRaised, Is.False);
		}

		[Test]
		public void VerifyWhenFailuresOccurred()
		{
			var rock = Rock.Create<IMockExtensions>();
			rock.Handle(_ => _.TargetMethod());

			var chunk = rock.Make();

			Assert.That(() => (chunk as IMock).Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public void VerifyWhenNoFailuresOccurred()
		{
			var rock = Rock.Create<IMockExtensions>();
			rock.Handle(_ => _.TargetMethod());

			var chunk = rock.Make();
			chunk.TargetMethod();

			Assert.That(() => (chunk as IMock).Verify(), Throws.Nothing);
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
