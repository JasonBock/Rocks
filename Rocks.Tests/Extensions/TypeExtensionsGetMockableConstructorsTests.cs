using NUnit.Framework;
using Rocks.Construction.InMemory;
using Rocks.Tests.Types;
using System;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsGetMockableConstructorTests
	{
		[Test]
		public void GetMockableConstructorsFromAbstractTypeWithProtectedConstructorThatUsesProtectedInternalType()
		{
			var constructors = typeof(HasConstructorWithArgumentThatUsesProtectedInternalType).GetMockableConstructors(
				new InMemoryNameGenerator());
			Assert.AreEqual(0, constructors.Count);
		}

		[Test]
		public void GetMockableConstructorsFromTypeWithObsoleteConstructors()
		{
			var constructors = typeof(HasObsoleteConstructors).GetMockableConstructors(
				new InMemoryNameGenerator());
			Assert.AreEqual(1, constructors.Count);
		}
	}

	public class HasObsoleteConstructors
	{
		[Obsolete("", false)]
		public HasObsoleteConstructors() { }

		[Obsolete("", true)]
		public HasObsoleteConstructors(int a) { }
	}
}
