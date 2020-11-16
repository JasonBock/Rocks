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
		public static void GetGenericArgumentsForTypeWithClassConstraints()
		{
			var namespaces = new SortedSet<string>();
			var (arguments, constraints) = typeof(IHaveGenericsWithClassConstraints<>).GetGenericArguments(namespaces);

			Assert.That(arguments, Is.EqualTo("<T>"));
			Assert.That(constraints, Is.EqualTo("where T : class"));
			Assert.That(namespaces.Count, Is.EqualTo(0), nameof(namespaces.Count));
		}

		[Test]
		public static void GetGenericArgumentsForTypeWithConstructorConstraints()
		{
			var namespaces = new SortedSet<string>();
			var (arguments, constraints) = typeof(IHaveGenericsWithConstructorConstraints<>).GetGenericArguments(namespaces);

			Assert.That(arguments, Is.EqualTo("<T>"));
			Assert.That(constraints, Is.EqualTo("where T : new()"));
			Assert.That(namespaces.Count, Is.EqualTo(0), nameof(namespaces.Count));
		}

		[Test]
		public static void GetGenericArgumentsForTypeWithNotNullConstraints()
		{
			var namespaces = new SortedSet<string>();
			var (arguments, constraints) = typeof(IHaveGenericsWithNotNullConstraints<>).GetGenericArguments(namespaces);

			Assert.That(arguments, Is.EqualTo("<T>"));
			Assert.That(constraints, Is.EqualTo("where T : notnull"));
			Assert.That(namespaces.Count, Is.EqualTo(0), nameof(namespaces.Count));
		}

		[Test]
		public static void GetGenericArgumentsForTypeWithNotNullAndConstructorConstraints()
		{
			var namespaces = new SortedSet<string>();
			var (arguments, constraints) = typeof(IHaveGenericsWithNotNullAndConstructorConstraints<>).GetGenericArguments(namespaces);

			Assert.That(arguments, Is.EqualTo("<T>"));
			Assert.That(constraints, Is.EqualTo("where T : notnull, new()"));
			Assert.That(namespaces.Count, Is.EqualTo(0), nameof(namespaces.Count));
		}

		[Test]
		public static void GetGenericArgumentsForTypeWithStructConstraints()
		{
			var namespaces = new SortedSet<string>();
			var (arguments, constraints) = typeof(IHaveGenericsWithStructConstraints<>).GetGenericArguments(namespaces);

			Assert.That(arguments, Is.EqualTo("<T>"));
			Assert.That(constraints, Is.EqualTo("where T : struct"));
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

	public interface IHaveGenericsWithClassConstraints<T> where T : class { }

	public interface IHaveGenericsWithConstructorConstraints<T> where T : new() { }

	public interface IHaveGenericsWithNotNullConstraints<T> where T : notnull { }

	public interface IHaveGenericsWithNotNullAndConstructorConstraints<T> where T : notnull, new() { }

	public interface IHaveGenericsWithStructConstraints<T> where T : struct { }

	public class HaveMethodWithComplexGenericType<TSource>
	{
		public virtual IEnumerable<KeyValuePair<long, TSource>>? Target(IEnumerable<KeyValuePair<long, TSource>> a) => null; 
	}
}