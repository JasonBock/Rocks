using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Descriptors;
using Rocks.Diagnostics;
using System.Globalization;

namespace Rocks.Tests.Descriptors;

public static class TypeHasNoAccessibleConstructorsDescriptorTests
{
	[Test]
	public static void Create()
	{
		var descriptor = TypeHasNoAccessibleConstructorsDescriptor.Create();

		Assert.Multiple(() =>
		{
			Assert.That(descriptor.Id, Is.EqualTo(TypeHasNoAccessibleConstructorsDescriptor.Id));
			Assert.That(descriptor.Title.ToString(CultureInfo.CurrentCulture), Is.EqualTo(TypeHasNoAccessibleConstructorsDescriptor.Title));
			Assert.That(descriptor.MessageFormat.ToString(CultureInfo.CurrentCulture), Is.EqualTo(TypeHasNoAccessibleConstructorsDescriptor.Message));
			Assert.That(descriptor.DefaultSeverity, Is.EqualTo(DiagnosticSeverity.Error));
			Assert.That(descriptor.Category, Is.EqualTo(DiagnosticConstants.Usage));
			Assert.That(descriptor.HelpLinkUri, Is.EqualTo(HelpUrlBuilder.Build(
				TypeHasNoAccessibleConstructorsDescriptor.Id, TypeHasNoAccessibleConstructorsDescriptor.Title)));
		});
	}
}