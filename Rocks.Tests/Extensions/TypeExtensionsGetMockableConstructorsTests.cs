using NUnit.Framework;
using Rocks.Construction;
using Rocks.Tests.Types;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsGetMockableConstructorTests
	{
		[Test]
		public void GetMockableConstructorsFromAbstractTypeWithProtectedConstructorThatUsesProtectedInternalType()
		{
			var constructors = typeof(HasConstructorWithArgumentThatUsesProtectedInternalType).GetMockableConstructors(new InMemoryNameGenerator());
			Assert.AreEqual(0, constructors.Count);
		}
	}
}
