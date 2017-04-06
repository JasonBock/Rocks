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
			Assert.That(exception.Diagnostics, Is.EqualTo(diagnostics), nameof(exception.Diagnostics));
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
			Assert.That(exception.Message, Is.EqualTo(message), nameof(exception.Message));
			Assert.That(exception.Diagnostics, Is.EqualTo(diagnostics), nameof(exception.Diagnostics));
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
			Assert.That(exception.Message, Is.EqualTo(message), nameof(exception.Message));
			Assert.That(exception.Diagnostics, Is.EqualTo(diagnostics), nameof(exception.Diagnostics));
			Assert.That(exception.InnerException, Is.SameAs(inner), nameof(exception.InnerException));
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
