using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions
{
	public static class ObjectExtensionsTests
	{
		[TestCase("b", "\"b\"")]
		[TestCase(true, "true")]
		[TestCase(false, "false")]
		[TestCase(null, "null")]
		public static void GetDefaultValue(object value, string expectedResult) => 
			Assert.That(value.GetDefaultValue(), Is.EqualTo(expectedResult));
	}
}