using NUnit.Framework;
using System.Collections.Generic;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class TypeExtensionsGetGenericArgumentsTests
	{
		[Test]
		public void GetGenericArgumentsForTypeWithNoConstraints()
		{
			var namespaces = new SortedSet<string>();
			var arguments = typeof(IHaveGenericsWithNoConstraints<>).GetGenericArguments(namespaces);

         Assert.AreEqual("<T>", arguments.Arguments);
			Assert.AreEqual(string.Empty, arguments.Constraints);
			Assert.AreEqual(0, namespaces.Count, nameof(namespaces.Count));
		}

		[Test]
		public void GetGenericArgumentsForTypeWithConstraints()
		{
			var namespaces = new SortedSet<string>();
			var arguments = typeof(IHaveGenericsWithConstraints<>).GetGenericArguments(namespaces);

			Assert.AreEqual("<T>", arguments.Arguments);
			Assert.AreEqual("where T : class", arguments.Constraints);
			Assert.AreEqual(0, namespaces.Count, nameof(namespaces.Count));
		}

		[Test]
		public void GetGenericArgumentsForComplexGenericType()
		{
			var namespaces = new SortedSet<string>();
			var arguments = typeof(HaveMethodWithComplexGenericType<>).GetMethod(nameof(HaveMethodWithComplexGenericType<int>.Target)).ReturnType.GetGenericArguments(namespaces);

			Assert.AreEqual("<KeyValuePair<Int64, TSource>>", arguments.Arguments);
			Assert.AreEqual(string.Empty, arguments.Constraints);
			Assert.AreEqual(0, namespaces.Count, nameof(namespaces.Count));
		}
	}

	public interface IHaveGenericsWithNoConstraints<T> { }

	public interface IHaveGenericsWithConstraints<T> where T : class { }

	public class HaveMethodWithComplexGenericType<TSource>
	{
		public virtual IEnumerable<KeyValuePair<long, TSource>> Target(IEnumerable<KeyValuePair<long, TSource>> a) { return null; }
	}

}
