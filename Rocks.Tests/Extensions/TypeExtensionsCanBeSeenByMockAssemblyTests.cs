using NUnit.Framework;
using Rocks.Construction;
using Rocks.Construction.InMemory;
using Rocks.Tests.Types;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsCanBeSeenByMockAssemblyTests
	{
		[Test]
		public void CanBeSeenWhenTypeIsInAssemblyWithInternalsVisibleTo()
		{
			Assert.IsTrue(this.GetType().CanBeSeenByMockAssembly(new InMemoryNameGenerator()));
		}

		[Test]
		public void CanBeSeenWhenTypeIsInAssemblyWithNoInternalsVisibleToAndPublic()
		{
			Assert.IsTrue(typeof(HaveInternalAbstractProperty).CanBeSeenByMockAssembly(new InMemoryNameGenerator()));
		}
	}
}
