using NUnit.Framework;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;

namespace Rocks.Tests.Exceptions
{
	[TestFixture]
	public sealed class VerificationExceptionTests
		: ExceptionTests<VerificationException, Exception>
	{
		[Test]
		public void Create()
		{
			base.CreateExceptionTest();
		}

		[Test]
		public void CreateWithFailures()
		{
			var exception = new VerificationException(new List<string> { "failure" }.AsReadOnly());
			Assert.That(exception.Failures.Count, Is.EqualTo(1), nameof(exception.Failures.Count));
			Assert.That(exception.Failures[0], Is.EqualTo("failure"), nameof(exception.Failures));
		}

		[Test]
		public void CreateWithMessage()
		{
			base.CreateExceptionWithMessageTest(Guid.NewGuid().ToString("N"));
      }

		[Test]
		public void CreateWithMessageAndFailures()
		{
			var message = Guid.NewGuid().ToString("N");
         var exception = new VerificationException(new List <string> { "failure" }.AsReadOnly(), message);
			Assert.That(exception.Message, Is.EqualTo(message), nameof(exception.Message));
			Assert.That(exception.Failures.Count, Is.EqualTo(1), nameof(exception.Failures.Count));
			Assert.That(exception.Failures[0], Is.EqualTo("failure"), nameof(exception.Failures));
		}

		[Test]
		public void CreateWithMessageAndInnerException()
		{
			base.CreateExceptionWithMessageAndInnerExceptionTest(Guid.NewGuid().ToString("N"));
		}

		[Test]
		public void CreateWithMessageAndInnerExceptionAndFailures()
		{
			var inner = new Exception();
			var message = Guid.NewGuid().ToString("N");
			var exception = new VerificationException(new List<string> { "failure" }.AsReadOnly(), message, inner);
			Assert.That(exception.InnerException, Is.SameAs(inner), nameof(exception.InnerException));
			Assert.That(exception.Message, Is.EqualTo(message), nameof(exception.Message));
			Assert.That(exception.Failures.Count, Is.EqualTo(1), nameof(exception.Failures.Count));
			Assert.That(exception.Failures[0], Is.EqualTo("failure"), nameof(exception.Failures));
		}

#if !NETCOREAPP1_1
		[Test]
		public void Roundtrip()
		{
			base.RoundtripExceptionTest(Guid.NewGuid().ToString("N"));
		}
#endif
	}
}
