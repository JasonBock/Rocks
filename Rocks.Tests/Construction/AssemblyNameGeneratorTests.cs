using NUnit.Framework;
using Rocks.Construction;

namespace Rocks.Tests.Construction
{
	[TestFixture]
	public sealed class AssemblyNameGeneratorTests
	{
		[Test]
		public void Generate()
		{
			var generator = new AssemblyNameGenerator(typeof(AssemblyNameGeneratorTests));
			Assert.AreEqual("Rocks.Tests.Rocks", generator.AssemblyName);
		}
	}
}
