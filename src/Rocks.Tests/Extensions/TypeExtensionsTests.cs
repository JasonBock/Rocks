using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions
{
	public static class Identifiers
	{
		[MemberIdentifier(1, "Foo")]
		public static void Foo() { }
	}

	public static class NoIdentifiers
	{
		public static void Foo() { }
	}

	public static class TypeExtensionsTests
	{
		[Test]
		public static void GetMemberDescription() =>
			Assert.Multiple(() =>
			{
				Assert.That(typeof(Identifiers).GetMemberDescription(1), Is.EqualTo("Foo"));
			});

		[Test]
		public static void GetMemberDescriptionWhenIdentifierIsNotFound() =>
			Assert.Multiple(() =>
			{
				Assert.That(typeof(Identifiers).GetMemberDescription(2), Is.Null);
			});

		[Test]
		public static void GetMemberDescriptionWhenAttributesDoNotExist() =>
			Assert.Multiple(() =>
			{
				Assert.That(typeof(NoIdentifiers).GetMemberDescription(1), Is.Null);
			});
	}
}