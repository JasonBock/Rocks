using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Descriptors;
using Rocks.Analysis.Diagnostics;
using System.Globalization;

namespace Rocks.Analysis.Tests.Descriptors;

internal static class InterfaceAllowsRefStructAsRefOrRefReadonlyReturnDescriptorTests
{
	[Test]
	public static void Create()
	{
		var descriptor = InterfaceAllowsRefStructAsRefOrRefReadonlyReturnDescriptor.Create();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(descriptor.Id, Is.EqualTo(DescriptorIdentifiers.InterfaceAllowsRefStructAsRefOrRefReadonlyReturnId));
			Assert.That(descriptor.Title.ToString(CultureInfo.CurrentCulture), Is.EqualTo(InterfaceAllowsRefStructAsRefOrRefReadonlyReturnDescriptor.Title));
			Assert.That(descriptor.MessageFormat.ToString(CultureInfo.CurrentCulture), Is.EqualTo(InterfaceAllowsRefStructAsRefOrRefReadonlyReturnDescriptor.Message));
			Assert.That(descriptor.DefaultSeverity, Is.EqualTo(DiagnosticSeverity.Error));
			Assert.That(descriptor.Category, Is.EqualTo(DiagnosticConstants.Usage));
			Assert.That(descriptor.HelpLinkUri, Is.EqualTo(HelpUrlBuilder.Build(
				DescriptorIdentifiers.InterfaceAllowsRefStructAsRefOrRefReadonlyReturnId, InterfaceAllowsRefStructAsRefOrRefReadonlyReturnDescriptor.Title)));
		}
	}
}