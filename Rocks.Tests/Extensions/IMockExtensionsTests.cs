using NUnit.Framework;
using System;
using Rocks.Exceptions;
using static Rocks.Extensions.IMockExtensions;

namespace Rocks.Tests.Extensions
{
	public static class IMockExtensionsTests
	{
		[Test]
		public static void GetVerificationFailuresWhenFailuresOccurred()
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
		public static void GetVerificationFailuresWhenNoFailuresOccurred()
		{
			var rock = Rock.Create<IMockExtensions>();
			rock.Handle(_ => _.TargetMethod());

			var chunk = rock.Make();
			chunk.TargetMethod();

			var failures = (chunk as IMock).GetVerificationFailures();
			Assert.That(failures.Count, Is.EqualTo(0));
		}

		[Test]
		public static void Raise()
		{
			var rock = Rock.Create<IMockExtensions>();
			rock.Handle(_ => _.TargetMethod());

			var chunk = rock.Make();
			var uses = new UsesIMockExtensions(chunk, true);
			chunk.Raise(nameof(IMockExtensions.TargetEvent), EventArgs.Empty);

			Assert.That(uses.WasEventRaised, Is.True);
		}

		[Test]
		public static void RaiseWhenNoHandlersExist()
		{
			var rock = Rock.Create<IMockExtensions>();
			rock.Handle(_ => _.TargetMethod());

			var chunk = rock.Make();
			var uses = new UsesIMockExtensions(chunk, false);
			chunk.Raise(nameof(IMockExtensions.TargetEvent), EventArgs.Empty);

			Assert.That(uses.WasEventRaised, Is.False);
		}

		[Test]
		public static void VerifyWhenFailuresOccurred()
		{
			var rock = Rock.Create<IMockExtensions>();
			rock.Handle(_ => _.TargetMethod());

			var chunk = rock.Make();

			Assert.That(() => (chunk as IMock).Verify(), Throws.TypeOf<VerificationException>());
		}

		[Test]
		public static void VerifyWhenNoFailuresOccurred()
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
		private readonly IMockExtensions mockExtensions;

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
