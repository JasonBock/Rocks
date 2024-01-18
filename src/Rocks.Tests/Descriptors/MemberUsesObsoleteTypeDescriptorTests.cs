using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Descriptors;
using Rocks.Diagnostics;
using System.Globalization;

namespace Rocks.Tests.Descriptors;

public static class MemberUsesObsoleteTypeDescriptorTests
{
	[Test]
	public static void Create()
	{
		var descriptor = MemberUsesObsoleteTypeDescriptor.Create();

		Assert.Multiple(() =>
		{
			Assert.That(descriptor.Id, Is.EqualTo(MemberUsesObsoleteTypeDescriptor.Id));
			Assert.That(descriptor.Title.ToString(CultureInfo.CurrentCulture), Is.EqualTo(MemberUsesObsoleteTypeDescriptor.Title));
			Assert.That(descriptor.MessageFormat.ToString(CultureInfo.CurrentCulture), Is.EqualTo(MemberUsesObsoleteTypeDescriptor.Message));
			Assert.That(descriptor.DefaultSeverity, Is.EqualTo(DiagnosticSeverity.Error));
			Assert.That(descriptor.Category, Is.EqualTo(DiagnosticConstants.Usage));
			Assert.That(descriptor.HelpLinkUri, Is.EqualTo(HelpUrlBuilder.Build(
				MemberUsesObsoleteTypeDescriptor.Id, MemberUsesObsoleteTypeDescriptor.Title)));
		});
	}
}