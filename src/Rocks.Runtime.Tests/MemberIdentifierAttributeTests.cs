using NUnit.Framework;

namespace Rocks.Runtime.Tests;

public static class MemberIdentifierAttributeTests
{
	[Test]
	public static void CreateWithValue()
	{
		var value = 3u;

		var attribute = new MemberIdentifierAttribute(value);

		Assert.Multiple(() =>
		{
			Assert.That(attribute.Value, Is.EqualTo(value));
			Assert.That(attribute.PropertyAccessor, Is.Null);
		});
	}

	[Test]
	public static void CreateWithValueAndPropertyAccessor()
	{
		var value = 3u;
		var propertyAccessor = PropertyAccessor.Set;

		var attribute = new MemberIdentifierAttribute(value, propertyAccessor);

		Assert.Multiple(() =>
		{
			Assert.That(attribute.Value, Is.EqualTo(value));
			Assert.That(attribute.PropertyAccessor, Is.EqualTo(propertyAccessor));
		});
	}
}