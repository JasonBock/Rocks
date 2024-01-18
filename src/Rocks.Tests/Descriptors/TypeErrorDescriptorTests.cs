using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Descriptors;
using Rocks.Diagnostics;
using System.Globalization;

namespace Rocks.Tests.Descriptors;

public static class TypeErrorDescriptorTests
{
	[Test]
	public static void Create()
	{
		var descriptor = TypeErrorDescriptor.Create();

		Assert.Multiple(() =>
		{
			Assert.That(descriptor.Id, Is.EqualTo(TypeErrorDescriptor.Id));
			Assert.That(descriptor.Title.ToString(CultureInfo.CurrentCulture), Is.EqualTo(TypeErrorDescriptor.Title));
			Assert.That(descriptor.MessageFormat.ToString(CultureInfo.CurrentCulture), Is.EqualTo(TypeErrorDescriptor.Message));
			Assert.That(descriptor.DefaultSeverity, Is.EqualTo(DiagnosticSeverity.Error));
			Assert.That(descriptor.Category, Is.EqualTo(DiagnosticConstants.Usage));
			Assert.That(descriptor.HelpLinkUri, Is.EqualTo(HelpUrlBuilder.Build(
				TypeErrorDescriptor.Id, TypeErrorDescriptor.Title)));
		});
	}
}