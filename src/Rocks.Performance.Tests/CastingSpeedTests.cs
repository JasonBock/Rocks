using NUnit.Framework;

namespace Rocks.Performance.Tests
{
	public static class CastingSpeedTests
	{
		[Test]
		public static void AsCast() =>
			Assert.That(new CastingSpeed().AsCast(), Is.Not.Null);

		[Test]
		public static void DirectCast() =>
			Assert.That(new CastingSpeed().DirectCast(), Is.Not.Null);

		[Test]
		public static void UnsafeAs() =>
			Assert.That(new CastingSpeed().UnsafeAs(), Is.Not.Null);
	}
}