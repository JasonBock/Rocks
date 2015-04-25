using NUnit.Framework;
using System;
using System.Reflection;
using static Rocks.Extensions.ConstructorInfoExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class ConstructorInfoExtensionsTests
	{
		[Test]
		public void IsUnsafeForMockForPublicCtorWithNoArguments()
		{
			Assert.IsFalse(typeof(SafePublicConstructors).GetConstructor(Type.EmptyTypes).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeForMockForPublicCtorWithSafeArguments()
		{
			Assert.IsFalse(typeof(SafePublicConstructors).GetConstructor(new[] { typeof(int) }).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeForMockForProtectedCtorWithNoArguments()
		{
			Assert.IsFalse(typeof(SafeProtectedConstructors).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, Type.EmptyTypes, null).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeForMockForProtectedCtorWithSafeArguments()
		{
			Assert.IsFalse(typeof(SafeProtectedConstructors).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, new[] { typeof(int) }, null).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeForMockForInternalCtorWithNoArguments()
		{
			Assert.IsFalse(typeof(SafeInternalConstructors).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, Type.EmptyTypes, null).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeForMockForInternalCtorWithSafeArguments()
		{
			Assert.IsFalse(typeof(SafeInternalConstructors).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, new[] { typeof(int) }, null).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeForMockForPublicCtorWithUnsafeArguments()
		{
			Assert.IsTrue(typeof(UnsafePublicConstructors).GetConstructor(new[] { typeof(int*) }).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeForMockForProtectedCtorWithUnsafeArguments()
		{
			Assert.IsTrue(typeof(UnsafeProtectedConstructors).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, new[] { typeof(int*) }, null).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeForMockForInternalCtorWithUnsafeArguments()
		{
			Assert.IsTrue(typeof(UnsafeInternalConstructors).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, new[] { typeof(int*) }, null).IsUnsafeToMock());
		}
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
