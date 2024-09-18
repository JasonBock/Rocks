using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions;

public static class StringExtensionsTests
{
	[TestCase("global::this<that>:?", "thisthat_null_")]
	[TestCase("thisthat", "thisthat")]
	public static void GenerateFileName(string input, string expectedFileName) =>
		Assert.That(input.GenerateFileName(), Is.EqualTo(expectedFileName));
}