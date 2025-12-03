using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Descriptors;
using Rocks.Analysis.Diagnostics;
using System.Globalization;

namespace Rocks.Analysis.Tests.Descriptors;

public static class InterfaceHasStaticAbstractMembersDescriptorTests
{
	[Test]
	public static void Create()
	{
		var descriptor = InterfaceHasStaticAbstractMembersDescriptor.Create();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(descriptor.Id, Is.EqualTo(InterfaceHasStaticAbstractMembersDescriptor.Id));
			Assert.That(descriptor.Title.ToString(CultureInfo.CurrentCulture), Is.EqualTo(InterfaceHasStaticAbstractMembersDescriptor.Title));
			Assert.That(descriptor.MessageFormat.ToString(CultureInfo.CurrentCulture), Is.EqualTo(InterfaceHasStaticAbstractMembersDescriptor.Message));
			Assert.That(descriptor.DefaultSeverity, Is.EqualTo(DiagnosticSeverity.Error));
			Assert.That(descriptor.Category, Is.EqualTo(DiagnosticConstants.Usage));
			Assert.That(descriptor.HelpLinkUri, Is.EqualTo(HelpUrlBuilder.Build(
				InterfaceHasStaticAbstractMembersDescriptor.Id, InterfaceHasStaticAbstractMembersDescriptor.Title)));
		}
	}
}