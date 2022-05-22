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
	[TestCase(Accessibility.ProtectedOrInternal, "protected internal")]
	[TestCase(Accessibility.ProtectedAndInternal, "private protected")]
	public static void GetCodeValue(Accessibility accessibility, string codeValue) =>
		Assert.That(accessibility.GetCodeValue(), Is.EqualTo(codeValue));

	[Test]
	public static void GetCodeValueWithInvalidAccesibility() =>
		Assert.That(() => Accessibility.NotApplicable.GetCodeValue(), Throws.TypeOf<InvalidEnumArgumentException>());
}