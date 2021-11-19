using NUnit.Framework;

namespace Rocks.Tests;

public static class HelpUrlBuilderTests
{
	[Test]
	public static void Create() =>
		Assert.That(HelpUrlBuilder.Build("a", "b"),
			Is.EqualTo("https://github.com/JasonBock/Rocks/tree/main/docs/a-b.md"));
}