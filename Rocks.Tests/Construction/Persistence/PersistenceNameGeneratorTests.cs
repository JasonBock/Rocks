using NUnit.Framework;
using Rocks.Construction.Persistence;

namespace Rocks.Tests.Construction.Persistence
{
	public static class PersistenceNameGeneratorTests
	{
		[Test]
		public static void Generate()
		{
			var generator = new PersistenceNameGenerator(typeof(PersistenceNameGeneratorTests));
			Assert.That(generator.AssemblyName, Is.EqualTo("Rocks.Tests.Rocks"));
		}
	}
}
