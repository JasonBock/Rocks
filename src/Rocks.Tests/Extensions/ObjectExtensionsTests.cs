using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class ObjectExtensionsTests
{
	[TestCase("b", "\"b\"")]
	[TestCase(true, "true")]
	[TestCase(false, "false")]
	[TestCase(null, "null")]
	[TestCase(null, "default", true)]
	public static void GetDefaultValue(object value, string expectedResult, bool isValueType = false) =>
		Assert.That(value.GetDefaultValue(isValueType), Is.EqualTo(expectedResult));
}