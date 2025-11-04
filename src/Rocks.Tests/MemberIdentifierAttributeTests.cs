using NUnit.Framework;

namespace Rocks.Tests;

public static class MemberIdentifierAttributeTests
{
	[Test]
	public static void Create()
	{
		var value = 3u;

		var attribute = new MemberIdentifierAttribute(value);

		Assert.That(attribute.Value, Is.EqualTo(value));
	}
}