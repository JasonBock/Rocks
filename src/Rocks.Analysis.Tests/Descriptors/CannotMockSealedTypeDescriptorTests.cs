using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Descriptors;
using Rocks.Analysis.Diagnostics;
using System.Globalization;

namespace Rocks.Analysis.Tests.Descriptors;

public static class CannotMockSealedTypeDescriptorTests
{
	[Test]
	public static void Create()
	{
		var descriptor = CannotMockSealedTypeDescriptor.Create();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(descriptor.Id, Is.EqualTo(CannotMockSealedTypeDescriptor.Id));
			Assert.That(descriptor.Title.ToString(CultureInfo.CurrentCulture), Is.EqualTo(CannotMockSealedTypeDescriptor.Title));
			Assert.That(descriptor.MessageFormat.ToString(CultureInfo.CurrentCulture), Is.EqualTo(CannotMockSealedTypeDescriptor.Message));
			Assert.That(descriptor.DefaultSeverity, Is.EqualTo(DiagnosticSeverity.Error));
			Assert.That(descriptor.Category, Is.EqualTo(DiagnosticConstants.Usage));
			Assert.That(descriptor.HelpLinkUri, Is.EqualTo(HelpUrlBuilder.Build(
				CannotMockSealedTypeDescriptor.Id, CannotMockSealedTypeDescriptor.Title)));
		}
	}
}