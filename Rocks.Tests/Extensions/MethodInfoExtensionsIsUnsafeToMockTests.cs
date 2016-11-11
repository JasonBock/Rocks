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
			Assert.That(typeof(UnsafeMembers).GetMethod(
				nameof(UnsafeMembers.MethodPublicVirtualSafeReturn)).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicVirtualSafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				nameof(UnsafeMembers.MethodPublicVirtualSafeArgument)).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicNonVirtualSafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				nameof(UnsafeMembers.MethodPublicNonVirtualSafeReturn)).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicNonVirtualSafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				nameof(UnsafeMembers.MethodPublicNonVirtualSafeArgument)).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicAbstractSafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				nameof(UnsafeMembers.MethodPublicAbstractSafeReturn)).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicAbstractSafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				nameof(UnsafeMembers.MethodPublicAbstractSafeArgument)).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedVirtualSafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodProtectedVirtualSafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedVirtualSafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodProtectedVirtualSafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedNonVirtualSafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodProtectedNonVirtualSafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedNonVirtualSafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodProtectedNonVirtualSafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedAbstractSafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodProtectedAbstractSafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedAbstractSafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodProtectedAbstractSafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalVirtualSafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodInternalVirtualSafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalVirtualSafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodInternalVirtualSafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalNonVirtualSafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodInternalNonVirtualSafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalNonVirtualSafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodInternalNonVirtualSafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalAbstractSafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodInternalAbstractSafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalAbstractSafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodInternalAbstractSafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicVirtualUnsafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				nameof(UnsafeMembers.MethodPublicVirtualUnsafeReturn)).IsUnsafeToMock(), Is.True);
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicVirtualUnsafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				nameof(UnsafeMembers.MethodPublicVirtualUnsafeArgument)).IsUnsafeToMock(), Is.True);
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicNonVirtualUnsafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				nameof(UnsafeMembers.MethodPublicNonVirtualUnsafeReturn)).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicNonVirtualUnsafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				nameof(UnsafeMembers.MethodPublicNonVirtualUnsafeArgument)).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicAbstractUnsafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				nameof(UnsafeMembers.MethodPublicAbstractUnsafeReturn)).IsUnsafeToMock(), Is.True);
		}

		[Test]
		public void IsUnsafeToMockWithMethodPublicAbstractUnsafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				nameof(UnsafeMembers.MethodPublicAbstractUnsafeArgument)).IsUnsafeToMock(), Is.True);
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedVirtualUnsafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodProtectedVirtualUnsafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedVirtualUnsafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodProtectedVirtualUnsafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedNonVirtualUnsafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodProtectedNonVirtualUnsafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedNonVirtualUnsafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodProtectedNonVirtualUnsafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedAbstractUnsafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodProtectedAbstractUnsafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.True);
		}

		[Test]
		public void IsUnsafeToMockWithMethodProtectedAbstractUnsafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodProtectedAbstractUnsafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.True);
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalVirtualUnsafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodInternalVirtualUnsafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalVirtualUnsafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodInternalVirtualUnsafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalNonVirtualUnsafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodInternalNonVirtualUnsafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalNonVirtualUnsafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodInternalNonVirtualUnsafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.False);
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalAbstractUnsafeReturn()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodInternalAbstractUnsafeReturn", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.True);
		}

		[Test]
		public void IsUnsafeToMockWithMethodInternalAbstractUnsafeArgument()
		{
			Assert.That(typeof(UnsafeMembers).GetMethod(
				"MethodInternalAbstractUnsafeArgument", BindingFlags.NonPublic | BindingFlags.Instance).IsUnsafeToMock(), Is.True);
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
