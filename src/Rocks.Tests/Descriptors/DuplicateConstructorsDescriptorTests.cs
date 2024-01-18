using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Descriptors;
using Rocks.Diagnostics;
using System.Globalization;

namespace Rocks.Tests.Descriptors;

public static class DuplicateConstructorsDescriptorTests
{
	[Test]
	public static void Create()
	{
		var descriptor = DuplicateConstructorsDescriptor.Create();

		Assert.Multiple(() =>
		{
			Assert.That(descriptor.Id, Is.EqualTo(DuplicateConstructorsDescriptor.Id));
			Assert.That(descriptor.Title.ToString(CultureInfo.CurrentCulture), Is.EqualTo(DuplicateConstructorsDescriptor.Title));
			Assert.That(descriptor.MessageFormat.ToString(CultureInfo.CurrentCulture), Is.EqualTo(DuplicateConstructorsDescriptor.Message));
			Assert.That(descriptor.DefaultSeverity, Is.EqualTo(DiagnosticSeverity.Error));
			Assert.That(descriptor.Category, Is.EqualTo(DiagnosticConstants.Usage));
			Assert.That(descriptor.HelpLinkUri, Is.EqualTo(HelpUrlBuilder.Build(
				DuplicateConstructorsDescriptor.Id, DuplicateConstructorsDescriptor.Title)));
		});
	}
}