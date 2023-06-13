using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace Rocks.Performance.Tests;

public static class GeneratorDrivingWithCachingAndUpdatesTests
{
	[Test]
	public static void DriveGenerator() =>
		Assert.That(new GeneratorDrivingWithCachingAndUpdates().Drive(), Is.EqualTo(IncrementalStepRunReason.Modified));

	[Test]
	public static void DriveGeneratorV3() =>
		Assert.That(new GeneratorDrivingWithCachingAndUpdates().DriveV3(), Is.EqualTo(IncrementalStepRunReason.Cached));
}