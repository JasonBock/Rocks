using NUnit.Framework;
using Rocks.Extensions;
using System.Collections.Generic;

namespace Rocks.Tests.Extensions
{
	public static class TypeExtensionsGetConstraintsTests
	{
		[Test]
		public static void GetConstraintsForNoConstraints()
		{
			var target = typeof(ClassWithNoConstraints<>).GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo(string.Empty));
		}

		[Test]
		public static void GetConstraintsForClassConstraint()
		{
			var target = typeof(ClassWithClassConstraint<>).GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo("where T : class"));
		}

		[Test]
		public static void GetConstraintsForConstructorConstraint()
		{
			var target = typeof(ClassWithConstructorConstraint<>).GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo("where T : new()"));
		}

		[Test]
		public static void GetConstraintsForNotNullConstraint()
		{
			var target = typeof(ClassWithNotNullConstraint<>).GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo("where T : notnull"));
		}

		[Test]
		public static void GetConstraintsForNotNullAndConstructorConstraint()
		{
			var target = typeof(ClassWithNotNullAndConstructorConstraint<>).GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo("where T : notnull, new()"));
		}

		[Test]
		public static void GetConstraintsForStructConstraint()
		{
			var target = typeof(ClassWithStructConstraint<>).GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo("where T : struct"));
		}
	}

	public sealed class ClassWithNoConstraints<T> { }
	public sealed class ClassWithClassConstraint<T> where T : class { }
	public sealed class ClassWithConstructorConstraint<T> where T : new() { }
	public sealed class ClassWithNotNullConstraint<T> where T : notnull { }
	public sealed class ClassWithNotNullAndConstructorConstraint<T> where T : notnull, new() { }
	public sealed class ClassWithStructConstraint<T> where T : struct { }
}
