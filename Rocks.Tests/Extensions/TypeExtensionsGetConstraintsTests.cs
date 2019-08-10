using NUnit.Framework;
using Rocks.Extensions;
using System.Collections.Generic;

namespace Rocks.Tests.Extensions
{
	public static class TypeExtensionsGetConstraintsTests
	{
		[Test]
		public static void GetConstraintsForMethodForNoConstraints()
		{
			var target = typeof(ClassWithConstrainedMethods).GetMethod(nameof(ClassWithConstrainedMethods.NoConstraints))!.GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo(string.Empty));
		}

		[Test]
		public static void GetConstraintsForMethodForClassConstraint()
		{
			var target = typeof(ClassWithConstrainedMethods).GetMethod(nameof(ClassWithConstrainedMethods.ClassConstraint))!.GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo("where T : class"));
		}

		[Test]
		public static void GetConstraintsForMethodForConstructorConstraint()
		{
			var target = typeof(ClassWithConstrainedMethods).GetMethod(nameof(ClassWithConstrainedMethods.ConstructorConstraint))!.GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo("where T : new()"));
		}

		[Test]
		public static void GetConstraintsForMethodForNotNullConstraint()
		{
			var target = typeof(ClassWithConstrainedMethods).GetMethod(nameof(ClassWithConstrainedMethods.NotNullConstraint))!.GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo("where T : notnull"));
		}

		[Test]
		public static void GetConstraintsForMethodForNotNullAndConstructorConstraint()
		{
			var target = typeof(ClassWithConstrainedMethods).GetMethod(nameof(ClassWithConstrainedMethods.NotNullAndConstructorConstraint))!.GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo("where T : notnull, new()"));
		}

		[Test]
		public static void GetConstraintsForMethodForStructConstraint()
		{
			var target = typeof(ClassWithConstrainedMethods).GetMethod(nameof(ClassWithConstrainedMethods.StructConstraint))!.GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo("where T : struct"));
		}

		[Test]
		public static void GetConstraintsForTypeForNoConstraints()
		{
			var target = typeof(ClassWithNoConstraints<>).GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo(string.Empty));
		}

		[Test]
		public static void GetConstraintsForTypeForClassConstraint()
		{
			var target = typeof(ClassWithClassConstraint<>).GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo("where T : class"));
		}

		[Test]
		public static void GetConstraintsForTypeForConstructorConstraint()
		{
			var target = typeof(ClassWithConstructorConstraint<>).GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo("where T : new()"));
		}

		[Test]
		public static void GetConstraintsForTypeForNotNullConstraint()
		{
			var target = typeof(ClassWithNotNullConstraint<>).GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo("where T : notnull"));
		}

		[Test]
		public static void GetConstraintsForTypeForNotNullAndConstructorConstraint()
		{
			var target = typeof(ClassWithNotNullAndConstructorConstraint<>).GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo("where T : notnull, new()"));
		}

		[Test]
		public static void GetConstraintsForTypeForStructConstraint()
		{
			var target = typeof(ClassWithStructConstraint<>).GetGenericArguments()[0];
			var constraints = target.GetConstraints(new SortedSet<string>());
			Assert.That(constraints, Is.EqualTo("where T : struct"));
		}
	}

	public static class ClassWithConstrainedMethods
	{
		public static void NoConstraints<T>() { }
		public static void ClassConstraint<T>() where T : class { }
		public static void ConstructorConstraint<T>() where T : new() { }
		public static void NotNullConstraint<T>() where T : notnull { }
		public static void NotNullAndConstructorConstraint<T>() where T : notnull, new() { }
		public static void StructConstraint<T>() where T : struct { }
	}

	public sealed class ClassWithNoConstraints<T> { }
	public sealed class ClassWithClassConstraint<T> where T : class { }
	public sealed class ClassWithConstructorConstraint<T> where T : new() { }
	public sealed class ClassWithNotNullConstraint<T> where T : notnull { }
	public sealed class ClassWithNotNullAndConstructorConstraint<T> where T : notnull, new() { }
	public sealed class ClassWithStructConstraint<T> where T : struct { }
}
