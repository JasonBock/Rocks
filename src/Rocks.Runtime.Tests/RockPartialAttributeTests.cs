using NUnit.Framework;

namespace Rocks.Runtime.Tests;

public static class RockPartialAttributeTests
{
	[TestCase(BuildType.Create, BuildType.Create)]
	[TestCase(BuildType.Make, BuildType.Make)]
	[TestCase(BuildType.Create | BuildType.Make, BuildType.Create)]
	public static void Create(BuildType buildType, BuildType expectedBuildType)
	{
		var mockType = typeof(string);

		var attribute = new RockPartialAttribute(mockType, buildType);

		Assert.Multiple(() =>
		{
			Assert.That(attribute.MockType, Is.EqualTo(mockType));
			Assert.That(attribute.BuildType, Is.EqualTo(expectedBuildType));
		});
	}
}
