using NUnit.Framework;
using System.Reflection;
using static Rocks.Extensions.MethodInfoExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodInfoExtensionsIsUnsafeToMockTests
	{
		[Test]
		public void IsUnsafeToMockWithMethodPublicVirtualSafeReturn()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod(nameof(UnsafeMembers.MethodPublicVirtualSafeReturn)).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicVirtualSafeArgument()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod(nameof(UnsafeMembers.MethodPublicVirtualSafeArgument)).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicNonVirtualSafeReturn()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod(nameof(UnsafeMembers.MethodPublicNonVirtualSafeReturn)).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicNonVirtualSafeArgument()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod(nameof(UnsafeMembers.MethodPublicNonVirtualSafeArgument)).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicAbstractSafeReturn()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod(nameof(UnsafeMembers.MethodPublicAbstractSafeReturn)).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicAbstractSafeArgument()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod(nameof(UnsafeMembers.MethodPublicAbstractSafeArgument)).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedVirtualSafeReturn()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodProtectedVirtualSafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedVirtualSafeArgument()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodProtectedVirtualSafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedNonVirtualSafeReturn()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodProtectedNonVirtualSafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedNonVirtualSafeArgument()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodProtectedNonVirtualSafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedAbstractSafeReturn()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodProtectedAbstractSafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedAbstractSafeArgument()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodProtectedAbstractSafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalVirtualSafeReturn()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodInternalVirtualSafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalVirtualSafeArgument()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodInternalVirtualSafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalNonVirtualSafeReturn()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodInternalNonVirtualSafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalNonVirtualSafeArgument()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodInternalNonVirtualSafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalAbstractSafeReturn()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodInternalAbstractSafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalAbstractSafeArgument()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodInternalAbstractSafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicVirtualUnsafeReturn()
		{
			Assert.IsTrue(typeof(UnsafeMembers).GetMethod(nameof(UnsafeMembers.MethodPublicVirtualUnsafeReturn)).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicVirtualUnsafeArgument()
		{
			Assert.IsTrue(typeof(UnsafeMembers).GetMethod(nameof(UnsafeMembers.MethodPublicVirtualUnsafeArgument)).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicNonVirtualUnsafeReturn()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod(nameof(UnsafeMembers.MethodPublicNonVirtualUnsafeReturn)).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicNonVirtualUnsafeArgument()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod(nameof(UnsafeMembers.MethodPublicNonVirtualUnsafeArgument)).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicAbstractUnsafeReturn()
		{
			Assert.IsTrue(typeof(UnsafeMembers).GetMethod(nameof(UnsafeMembers.MethodPublicAbstractUnsafeReturn)).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicAbstractUnsafeArgument()
		{
			Assert.IsTrue(typeof(UnsafeMembers).GetMethod(nameof(UnsafeMembers.MethodPublicAbstractUnsafeArgument)).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedVirtualUnsafeReturn()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodProtectedVirtualUnsafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedVirtualUnsafeArgument()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodProtectedVirtualUnsafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedNonVirtualUnsafeReturn()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodProtectedNonVirtualUnsafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedNonVirtualUnsafeArgument()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodProtectedNonVirtualUnsafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedAbstractUnsafeReturn()
		{
			Assert.IsTrue(typeof(UnsafeMembers).GetMethod("MethodProtectedAbstractUnsafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedAbstractUnsafeArgument()
		{
			Assert.IsTrue(typeof(UnsafeMembers).GetMethod("MethodProtectedAbstractUnsafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalVirtualUnsafeReturn()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodInternalVirtualUnsafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalVirtualUnsafeArgument()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodInternalVirtualUnsafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalNonVirtualUnsafeReturn()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodInternalNonVirtualUnsafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalNonVirtualUnsafeArgument()
		{
			Assert.IsFalse(typeof(UnsafeMembers).GetMethod("MethodInternalNonVirtualUnsafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalAbstractUnsafeReturn()
		{
			Assert.IsTrue(typeof(UnsafeMembers).GetMethod("MethodInternalAbstractUnsafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalAbstractUnsafeArgument()
		{
			Assert.IsTrue(typeof(UnsafeMembers).GetMethod("MethodInternalAbstractUnsafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock());
		}

	}

	public abstract unsafe class UnsafeMembers
	{
		public virtual byte MethodPublicVirtualSafeReturn() { return default(byte); }
		public virtual void MethodPublicVirtualSafeArgument(byte a) { }
		public byte MethodPublicNonVirtualSafeReturn() { return default(byte); }
		public void MethodPublicNonVirtualSafeArgument(byte a) { }
		public abstract byte MethodPublicAbstractSafeReturn();
		public abstract void MethodPublicAbstractSafeArgument(byte a);
		protected virtual byte MethodProtectedVirtualSafeReturn() { return default(byte); }
		protected virtual void MethodProtectedVirtualSafeArgument(byte a) { }
		protected byte MethodProtectedNonVirtualSafeReturn() { return default(byte); }
		protected void MethodProtectedNonVirtualSafeArgument(byte a) { }
		protected abstract byte MethodProtectedAbstractSafeReturn();
		protected abstract void MethodProtectedAbstractSafeArgument(byte a);
		internal virtual byte MethodInternalVirtualSafeReturn() { return default(byte); }
		internal virtual void MethodInternalVirtualSafeArgument(byte a) { }
		internal byte MethodInternalNonVirtualSafeReturn() { return default(byte); }
		internal void MethodInternalNonVirtualSafeArgument(byte a) { }
		internal abstract byte MethodInternalAbstractSafeReturn();
		internal abstract void MethodInternalAbstractSafeArgument(byte a);
		public virtual byte* MethodPublicVirtualUnsafeReturn() { return default(byte*); }
		public virtual void MethodPublicVirtualUnsafeArgument(byte* a) { }
		public byte* MethodPublicNonVirtualUnsafeReturn() { return default(byte*); }
		public void MethodPublicNonVirtualUnsafeArgument(byte* a) { }
		public abstract byte* MethodPublicAbstractUnsafeReturn();
		public abstract void MethodPublicAbstractUnsafeArgument(byte* a);
		protected virtual byte* MethodProtectedVirtualUnsafeReturn() { return default(byte*); }
		protected virtual void MethodProtectedVirtualUnsafeArgument(byte* a) { }
		protected byte* MethodProtectedNonVirtualUnsafeReturn() { return default(byte*); }
		protected void MethodProtectedNonVirtualUnsafeArgument(byte* a) { }
		protected abstract byte* MethodProtectedAbstractUnsafeReturn();
		protected abstract void MethodProtectedAbstractUnsafeArgument(byte* a);
		internal virtual byte* MethodInternalVirtualUnsafeReturn() { return default(byte*); }
		internal virtual void MethodInternalVirtualUnsafeArgument(byte* a) { }
		internal byte* MethodInternalNonVirtualUnsafeReturn() { return default(byte*); }
		internal void MethodInternalNonVirtualUnsafeArgument(byte* a) { }
		internal abstract byte* MethodInternalAbstractUnsafeReturn();
		internal abstract void MethodInternalAbstractUnsafeArgument(byte* a);
	}
}
