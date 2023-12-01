using NUnit.Framework;

namespace Rocks.Performance.Tests;

internal static class CastingComparisonVerifyTests
{
   [Test]
   public static void VerifyOldWay() => 
		Assert.That(new CastingComparisonVerify().VerifyOldWay(), Is.EqualTo("4"));

	[Test]
	public static void VerifyNewWay() =>
		Assert.That(new CastingComparisonVerify().VerifyNewWay(), Is.EqualTo("4"));
}