using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Extensions;
using System.ComponentModel;

namespace Rocks.Tests.Extensions;

public static class AccessibilityExtensionsTests
{
	[TestCase(Accessibility.Public, "public")]
	[TestCase(Accessibility.Private, "private")]
	[TestCase(Accessibility.Protected, "protected")]
	[TestCase(Accessibility.Internal, "internal")]
	[TestCase(Accessibility.ProtectedOrInternal, "protected")]
	[TestCase(Accessibility.ProtectedAndInternal, "private protected")]
	public static void GetOverridingCodeValue(Accessibility accessibility, string codeValue) =>
		Assert.That(accessibility.GetOverridingCodeValue(), Is.EqualTo(codeValue));

	[Test]
	public static void GetOverridingCodeValueWithInvalidAccesibility() =>
		Assert.That(() => Accessibility.NotApplicable.GetOverridingCodeValue(), Throws.TypeOf<InvalidEnumArgumentException>());
}