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
			Assert.AreEqual("Rocks.Tests.Rocks", generator.AssemblyName);
		}
	}
}
