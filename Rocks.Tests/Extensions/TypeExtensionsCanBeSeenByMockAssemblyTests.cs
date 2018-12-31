using NUnit.Framework;
using Rocks.Construction.InMemory;
using Rocks.Tests.Types;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	public sealed class TypeExtensionsCanBeSeenByMockAssemblyTests
	{
		[Test]
		public void CanBeSeenWhenTypeIsInAssemblyWithInternalsVisibleTo() =>
			Assert.That(this.GetType().CanBeSeenByMockAssembly(new InMemoryNameGenerator()), Is.True);

		[Test]
		public void CanBeSeenWhenTypeIsInAssemblyWithNoInternalsVisibleToAndPublic() =>
			Assert.That(typeof(HaveInternalAbstractProperty).CanBeSeenByMockAssembly(new InMemoryNameGenerator()), Is.True);
	}
}
