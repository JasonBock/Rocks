using NUnit.Framework;

namespace Rocks.Tests;

public static class RockAttributeTests
{
   [Test]
   public static void Create() => 
		Assert.That(new RockAttribute<string>(BuildType.Make).Type, Is.EqualTo(BuildType.Make));
}