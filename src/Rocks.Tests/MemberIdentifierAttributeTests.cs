using NUnit.Framework;

namespace Rocks.Tests;

public static class MemberIdentifierAttributeTests
{
	[Test]
	public static void Create()
	{
		var value = 3u;
		var description = "a";

		var attribute = new MemberIdentifierAttribute(value, description);

		Assert.Multiple(() =>
		{
			Assert.That(attribute.Value, Is.EqualTo(value));
			Assert.That(attribute.Description, Is.EqualTo(description));
		});
	}
}