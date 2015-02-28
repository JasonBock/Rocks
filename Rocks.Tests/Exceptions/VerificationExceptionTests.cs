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
			Assert.AreEqual(1, exception.Failures.Count, nameof(exception.Failures.Count));
			Assert.AreEqual("failure", exception.Failures[0], nameof(exception.Failures));
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
			Assert.AreEqual(message, exception.Message, nameof(exception.Message));
			Assert.AreEqual(1, exception.Failures.Count, nameof(exception.Failures.Count));
			Assert.AreEqual("failure", exception.Failures[0], nameof(exception.Failures));
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
			Assert.AreSame(inner, exception.InnerException, nameof(exception.InnerException));
			Assert.AreEqual(message, exception.Message, nameof(exception.Message));
			Assert.AreEqual(1, exception.Failures.Count, nameof(exception.Failures.Count));
			Assert.AreEqual("failure", exception.Failures[0], nameof(exception.Failures));
		}

		[Test]
		public void Roundtrip()
		{
			base.RoundtripExceptionTest(Guid.NewGuid().ToString("N"));
		}
	}
}
