using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Descriptors;
using Rocks.Analysis.Diagnostics;
using System.Globalization;

namespace Rocks.Analysis.Tests.Descriptors;

public static class CannotMockObsoleteTypeDescriptorTests
{
	[Test]
	public static void Create()
	{
		var descriptor = CannotMockObsoleteTypeDescriptor.Create();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(descriptor.Id, Is.EqualTo(CannotMockObsoleteTypeDescriptor.Id));
			Assert.That(descriptor.Title.ToString(CultureInfo.CurrentCulture), Is.EqualTo(CannotMockObsoleteTypeDescriptor.Title));
			Assert.That(descriptor.MessageFormat.ToString(CultureInfo.CurrentCulture), Is.EqualTo(CannotMockObsoleteTypeDescriptor.Message));
			Assert.That(descriptor.DefaultSeverity, Is.EqualTo(DiagnosticSeverity.Error));
			Assert.That(descriptor.Category, Is.EqualTo(DiagnosticConstants.Usage));
			Assert.That(descriptor.HelpLinkUri, Is.EqualTo(HelpUrlBuilder.Build(
				CannotMockObsoleteTypeDescriptor.Id, CannotMockObsoleteTypeDescriptor.Title)));
		}
	}
}