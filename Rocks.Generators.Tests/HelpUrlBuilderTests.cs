using NUnit.Framework;

namespace Rocks.Tests
{
	public static class HelpUrlBuilderTests
	{
		[Test]
		public static void Create() =>
			Assert.Multiple(() =>
			{
				Assert.That(HelpUrlBuilder.Build("a", "b"),
					Is.EqualTo("https://github.com/JasonBock/Rocks/tree/master/Rocks.Documentation/a-b.md"));
			});
	}
}