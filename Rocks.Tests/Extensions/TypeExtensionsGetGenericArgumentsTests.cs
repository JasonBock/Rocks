using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using static Rocks.Extensions.TypeExtensions;

namespace Rocks.Tests.Extensions
{
	public static class TypeExtensionsGetGenericArgumentsTests
	{
		[Test]
		public static void GetGenericArgumentsForTypeWithNoConstraints()
		{
			var namespaces = new SortedSet<string>();
			var (arguments, constraints) = typeof(IHaveGenericsWithNoConstraints<>).GetGenericArguments(namespaces);

			Assert.That(arguments, Is.EqualTo("<T>"));
			Assert.That(constraints, Is.Empty);
			Assert.That(namespaces.Count, Is.EqualTo(0), nameof(namespaces.Count));
		}

		[Test]
		public static void GetGenericArgumentsForTypeWithConstraints()
		{
			var namespaces = new SortedSet<string>();
			var (arguments, constraints) = typeof(IHaveGenericsWithConstraints<>).GetGenericArguments(namespaces);

			Assert.That(arguments, Is.EqualTo("<T>"));
			Assert.That(constraints, Is.EqualTo("where T : class"));
			Assert.That(namespaces.Count, Is.EqualTo(0), nameof(namespaces.Count));
		}

		[Test]
		public static void GetGenericArgumentsForComplexGenericType()
		{
			var namespaces = new SortedSet<string>();
			var (arguments, constraints) = typeof(HaveMethodWithComplexGenericType<>).GetMethod(
				nameof(HaveMethodWithComplexGenericType<int>.Target))!.ReturnType.GetGenericArguments(namespaces);

			Assert.That(arguments, Is.EqualTo("<KeyValuePair<long, TSource>>"));
			Assert.That(constraints, Is.Empty);
			Assert.That(namespaces.Count, Is.EqualTo(0), nameof(namespaces.Count));
		}
	}

	public interface IHaveGenericsWithNoConstraints<T> { }

	public interface IHaveGenericsWithConstraints<T> where T : class { }

	public class HaveMethodWithComplexGenericType<TSource>
	{
		public virtual IEnumerable<KeyValuePair<long, TSource>>? Target(IEnumerable<KeyValuePair<long, TSource>> a) => null; 
	}
}