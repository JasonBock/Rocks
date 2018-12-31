using NUnit.Framework;
using Rocks.Tests.Extensions.AnotherGeneric;
using Rocks.Tests.Extensions.Generic;
using Rocks.Tests.Extensions.NotGeneric;
using System.Collections.Generic;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	public static class TypeExtensionsAddNamespacesTests
	{
		[Test]
		public static void AddNamespacesForNonGenericType()
		{
			var namespaces = new SortedSet<string>();
			typeof(IAmNotGeneric).AddNamespaces(namespaces);

			Assert.That(namespaces.Count, Is.EqualTo(1));
			Assert.That(namespaces.Contains(typeof(IAmNotGeneric).Namespace), Is.True);
		}

		[Test]
		public static void AddNamespacesForGenericType()
		{
			var namespaces = new SortedSet<string>();
			typeof(IAmGeneric<IAmNotGeneric>).AddNamespaces(namespaces);

			Assert.That(namespaces.Count, Is.EqualTo(2));
			Assert.That(namespaces.Contains(typeof(IAmNotGeneric).Namespace), Is.True);
			Assert.That(namespaces.Contains(typeof(IAmGeneric<>).Namespace), Is.True);
		}

		[Test]
		public static void AddNamespacesForGenericTypeHierarchy()
		{
			var namespaces = new SortedSet<string>();
			typeof(IAmAnotherGeneric<IAmGeneric<IAmNotGeneric>>).AddNamespaces(namespaces);

			Assert.That(namespaces.Count, Is.EqualTo(3));
			Assert.That(namespaces.Contains(typeof(IAmNotGeneric).Namespace), Is.True);
			Assert.That(namespaces.Contains(typeof(IAmGeneric<>).Namespace), Is.True);
			Assert.That(namespaces.Contains(typeof(IAmAnotherGeneric<>).Namespace), Is.True);
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
