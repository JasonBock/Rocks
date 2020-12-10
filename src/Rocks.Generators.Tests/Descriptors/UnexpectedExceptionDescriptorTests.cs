using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Descriptors;
using System;
using System.Globalization;

namespace Rocks.Tests.Descriptors
{
	public static class UnexpectedExceptionDescriptorTests
	{
		private static void CreateException() => throw new NotSupportedException();

		[Test]
		public static void Create()
		{
			try
			{
				UnexpectedExceptionDescriptorTests.CreateException();
			}
			catch(NotSupportedException e)
			{
				var descriptor = UnexpectedExceptionDescriptor.Create(e);

				Assert.Multiple(() =>
				{
					const string expectedMessageStart =
						"System.NotSupportedException: Specified method is not supported. :    at Rocks.Tests.Descriptors.UnexpectedExceptionDescriptorTests.CreateException()";
					Assert.That(descriptor.GetMessage().StartsWith(expectedMessageStart), Is.True);
					Assert.That(descriptor.Descriptor.Title.ToString(CultureInfo.CurrentCulture), Is.EqualTo(UnexpectedExceptionDescriptor.Title));
					Assert.That(descriptor.Id, Is.EqualTo(UnexpectedExceptionDescriptor.Id));
					Assert.That(descriptor.Severity, Is.EqualTo(DiagnosticSeverity.Error));
				});
			}
		}
	}
}