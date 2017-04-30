using NUnit.Framework;
using Rocks.Construction.Persistence;

namespace Rocks.Tests.Construction.Persistence
{
	[TestFixture]
	public sealed class PersistenceNameGeneratorTests
	{
		[Test]
		public void Generate()
		{
			var generator = new PersistenceNameGenerator(typeof(PersistenceNameGeneratorTests));
			Assert.That(generator.AssemblyName, Is.EqualTo("Rocks.Tests.Rocks"));
		}
	}
}
