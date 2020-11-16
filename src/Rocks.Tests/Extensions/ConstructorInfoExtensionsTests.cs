using NUnit.Framework;
using System;
using System.Reflection;
using static Rocks.Extensions.ConstructorInfoExtensions;

namespace Rocks.Tests.Extensions
{
	public static class ConstructorInfoExtensionsTests
	{
		[Test]
		public static void IsUnsafeForMockForPublicCtorWithNoArguments() =>
			Assert.That(typeof(SafePublicConstructors).GetConstructor(Type.EmptyTypes)!.IsUnsafeToMock(), Is.False);

		[Test]
		public static void IsUnsafeForMockForPublicCtorWithSafeArguments() =>
			Assert.That(typeof(SafePublicConstructors).GetConstructor(new[] { typeof(int) })!.IsUnsafeToMock(), Is.False);

		[Test]
		public static void IsUnsafeForMockForProtectedCtorWithNoArguments() =>
			Assert.That(typeof(SafeProtectedConstructors).GetConstructor(
				BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, Type.EmptyTypes, null)!.IsUnsafeToMock(), Is.False);

		[Test]
		public static void IsUnsafeForMockForProtectedCtorWithSafeArguments() =>
			Assert.That(typeof(SafeProtectedConstructors).GetConstructor(
				BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, new[] { typeof(int) }, null)!.IsUnsafeToMock(), Is.False);

		[Test]
		public static void IsUnsafeForMockForInternalCtorWithNoArguments() =>
			Assert.That(typeof(SafeInternalConstructors).GetConstructor(
				BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, Type.EmptyTypes, null)!.IsUnsafeToMock(), Is.False);

		[Test]
		public static void IsUnsafeForMockForInternalCtorWithSafeArguments() =>
			Assert.That(typeof(SafeInternalConstructors).GetConstructor(
				BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, new[] { typeof(int) }, null)!.IsUnsafeToMock(), Is.False);

		[Test]
		public static void IsUnsafeForMockForProtectedCtorWithUnsafeArguments() =>
			Assert.That(typeof(UnsafeProtectedConstructors).GetConstructor(
				BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, new[] { typeof(int*) }, null)!.IsUnsafeToMock(), Is.True);

		[Test]
		public static void IsUnsafeForMockForInternalCtorWithUnsafeArguments() =>
			Assert.That(typeof(UnsafeInternalConstructors).GetConstructor(
				BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, new[] { typeof(int*) }, null)!.IsUnsafeToMock(), Is.True);

		[Test]
		public static void IsUnsafeForMockForPublicCtorWithUnsafeArguments() =>
			Assert.That(typeof(UnsafePublicConstructors).GetConstructor(new[] { typeof(int*) })!.IsUnsafeToMock(), Is.True);
	}

	public class SafePublicConstructors
	{
		public SafePublicConstructors() { }
		public SafePublicConstructors(int a) { }
	}

	public class SafeInternalConstructors
	{
		internal SafeInternalConstructors() { }
		internal SafeInternalConstructors(int a) { }
	}

	public class SafeProtectedConstructors
	{
		protected SafeProtectedConstructors() { }
		protected SafeProtectedConstructors(int a) { }
	}

	public unsafe class UnsafePublicConstructors
	{
		public UnsafePublicConstructors(int* a) { }
	}

	public unsafe class UnsafeInternalConstructors
	{
		internal UnsafeInternalConstructors(int* a) { }
	}

	public unsafe class UnsafeProtectedConstructors
	{
		protected UnsafeProtectedConstructors(int* a) { }
	}
}
