using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Diagnostics;
using System;
using System.Globalization;

namespace Rocks.Tests.Diagnostics
{
	public static class UnexpectedExceptionDiagnosticTests
	{
		private static void CreateException() => throw new NotSupportedException();

		[Test]
		public static void Create()
		{
			try
			{
				UnexpectedExceptionDiagnosticTests.CreateException();
			}
			catch(NotSupportedException e)
			{
				var descriptor = UnexpectedExceptionDiagnostic.Create(e);

				Assert.Multiple(() =>
				{
					const string expectedMessageStart =
						"System.NotSupportedException: Specified method is not supported. :    at Rocks.Tests.Diagnostics.UnexpectedExceptionDiagnosticTests.CreateException()";
					Assert.That(descriptor.GetMessage(), Does.StartWith(expectedMessageStart));
					Assert.That(descriptor.Descriptor.Title.ToString(CultureInfo.CurrentCulture), Is.EqualTo(UnexpectedExceptionDiagnostic.Title));
					Assert.That(descriptor.Id, Is.EqualTo(UnexpectedExceptionDiagnostic.Id));
					Assert.That(descriptor.Severity, Is.EqualTo(DiagnosticSeverity.Error));
				});
			}
		}
	}
}