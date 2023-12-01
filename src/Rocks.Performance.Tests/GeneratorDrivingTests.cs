using NUnit.Framework;

namespace Rocks.Performance.Tests;

	public static class GeneratorDrivingTests
	{
		[Test]
		public static void DriveGenerator() =>
			Assert.That(new GeneratorDriving().Drive(), Is.EqualTo(0));
	}