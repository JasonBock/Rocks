using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
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

			Assert.That(arguments.Arguments, Is.EqualTo("<T>"));
			Assert.That(arguments.Constraints, Is.Empty);
			Assert.That(namespaces.Count, Is.EqualTo(0), nameof(namespaces.Count));
		}

		[Test]
		public void GetGenericArgumentsForTypeWithConstraints()
		{
			var namespaces = new SortedSet<string>();
			var arguments = typeof(IHaveGenericsWithConstraints<>).GetGenericArguments(namespaces);

			Assert.That(arguments.Arguments, Is.EqualTo("<T>"));
			Assert.That(arguments.Constraints, Is.EqualTo("where T : class"));
			Assert.That(namespaces.Count, Is.EqualTo(0), nameof(namespaces.Count));
		}

		[Test]
		public void GetGenericArgumentsForComplexGenericType()
		{
			var namespaces = new SortedSet<string>();
			var arguments = typeof(HaveMethodWithComplexGenericType<>).GetTypeInfo().GetMethod(
				nameof(HaveMethodWithComplexGenericType<int>.Target)).ReturnType.GetGenericArguments(namespaces);

			Assert.That(arguments.Arguments, Is.EqualTo("<KeyValuePair<long, TSource>>"));
			Assert.That(arguments.Constraints, Is.Empty);
			Assert.That(namespaces.Count, Is.EqualTo(0), nameof(namespaces.Count));
		}
	}

	public interface IHaveGenericsWithNoConstraints<T> { }

	public interface IHaveGenericsWithConstraints<T> where T : class { }

	public class HaveMethodWithComplexGenericType<TSource>
	{
		public virtual IEnumerable<KeyValuePair<long, TSource>> Target(IEnumerable<KeyValuePair<long, TSource>> a) => null; 
	}
}
