using NUnit.Framework;

namespace Rocks.Runtime.Tests;

public static class RockAttributeTests
{
	[Test]
	public static void Create()
	{
		var mockType = typeof(string);
		var buildType = BuildType.Create;

		var attribute = new RockAttribute(mockType, buildType);

		Assert.Multiple(() =>
		{
			Assert.That(attribute.MockType, Is.EqualTo(mockType));
			Assert.That(attribute.BuildType.HasFlag(BuildType.Create), Is.True);
		});
	}
}