using NUnit.Framework;
using Rocks.Tests.Extensions.AnotherGeneric;
using Rocks.Tests.Extensions.Generic;
using Rocks.Tests.Extensions.NotGeneric;
using System.Collections.Generic;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsAddNamespacesTests
	{
		[Test]
		public void AddNamespacesForNonGenericType()
		{
			var namespaces = new SortedSet<string>();
			typeof(IAmNotGeneric).AddNamespaces(namespaces);

			Assert.AreEqual(1, namespaces.Count);
			Assert.IsTrue(namespaces.Contains(typeof(IAmNotGeneric).Namespace));
		}

		[Test]
		public void AddNamespacesForGenericType()
		{
			var namespaces = new SortedSet<string>();
			typeof(IAmGeneric<IAmNotGeneric>).AddNamespaces(namespaces);

			Assert.AreEqual(2, namespaces.Count);
			Assert.IsTrue(namespaces.Contains(typeof(IAmNotGeneric).Namespace));
			Assert.IsTrue(namespaces.Contains(typeof(IAmGeneric<>).Namespace));
		}

		[Test]
		public void AddNamespacesForGenericTypeHierarchy()
		{
			var namespaces = new SortedSet<string>();
			typeof(IAmAnotherGeneric<IAmGeneric<IAmNotGeneric>>).AddNamespaces(namespaces);

			Assert.AreEqual(3, namespaces.Count);
			Assert.IsTrue(namespaces.Contains(typeof(IAmNotGeneric).Namespace));
			Assert.IsTrue(namespaces.Contains(typeof(IAmGeneric<>).Namespace));
			Assert.IsTrue(namespaces.Contains(typeof(IAmAnotherGeneric<>).Namespace));
		}
	}
}

namespace Rocks.Tests.Extensions.NotGeneric
{
	public interface IAmNotGeneric { }
}

namespace Rocks.Tests.Extensions.Generic
{
	public interface IAmGeneric<T> { }
}

namespace Rocks.Tests.Extensions.AnotherGeneric
{
	public interface IAmAnotherGeneric<T> { }
}
