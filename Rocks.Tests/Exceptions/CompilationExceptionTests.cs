using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Rocks.Tests.Exceptions
{
	[TestFixture]
	public sealed class CompilationExceptionTests
		: ExceptionTests<CompilationException, Exception>
	{
		[Test]
		public void Create()
		{
			base.CreateExceptionTest();
		}

		[Test]
		public void CreateWithDiagnostic()
		{
			var diagnostics = ImmutableArray.Create<Diagnostic>();
         var exception = new CompilationException(diagnostics);
			Assert.AreEqual(diagnostics, exception.Diagnostics, nameof(exception.Diagnostics));
		}

		[Test]
		public void CreateWithMessage()
		{
			base.CreateExceptionWithMessageTest(Guid.NewGuid().ToString("N"));
      }

		[Test]
		public void CreateWithDiagnosticAndMessage()
		{
			var message = Guid.NewGuid().ToString("N");
         var diagnostics = ImmutableArray.Create<Diagnostic>();
			var exception = new CompilationException(diagnostics, message);
			Assert.AreEqual(message, exception.Message, nameof(exception.Message));
			Assert.AreEqual(diagnostics, exception.Diagnostics, nameof(exception.Diagnostics));
		}

		[Test]
		public void CreateWithMessageAndInnerException()
		{
			base.CreateExceptionWithMessageAndInnerExceptionTest(Guid.NewGuid().ToString("N"));
		}

		[Test]
		public void CreateWithDiagnosticAndMessageAndInnerException()
		{
			var inner = new Exception();
			var message = Guid.NewGuid().ToString("N");
			var diagnostics = ImmutableArray.Create<Diagnostic>();
			var exception = new CompilationException(diagnostics, message, inner);
			Assert.AreEqual(message, exception.Message, nameof(exception.Message));
			Assert.AreEqual(diagnostics, exception.Diagnostics, nameof(exception.Diagnostics));
			Assert.AreSame(inner, exception.InnerException, nameof(exception.InnerException));
		}

		[Test]
		public void Roundtrip()
		{
			base.RoundtripExceptionTest(Guid.NewGuid().ToString("N"));
		}
	}
}
