using NUnit.Framework;
using Rocks.Exceptions;
using System;

namespace Rocks.Tests.Exceptions
{
	[TestFixture]
	public sealed class ValidationExceptionTests
		: ExceptionTests<ValidationException, Exception>
	{
		[Test]
		public void Create()
		{
			base.CreateExceptionTest();
		}

		[Test]
		public void CreateWithMessage()
		{
			base.CreateExceptionWithMessageTest(System.Guid.NewGuid().ToString("N"));
      }

		[Test]
		public void CreateWithMessageAndInnerException()
		{
			base.CreateExceptionWithMessageAndInnerExceptionTest(System.Guid.NewGuid().ToString("N"));
		}

		[Test]
		public void Roundtrip()
		{
			base.RoundtripExceptionTest(System.Guid.NewGuid().ToString("N"));
		}
	}
}
