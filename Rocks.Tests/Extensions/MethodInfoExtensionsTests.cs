using NUnit.Framework;
using static Rocks.Extensions.MethodInfoExtensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodInfoExtensionsTests
	{
		[Test]
		public void ContainsOutInitializers()
		{
			Assert.AreEqual("a = default(Int32);", this.GetType()
				.GetMethod(nameof(this.TargetWithOutArgument)).GetOutInitializers());
		}

		[Test]
		public void ContainsRefArguments()
		{
			Assert.IsTrue(typeof(IHaveMethodWithRefArgument)
				.GetMethod(nameof(IHaveMethodWithRefArgument.Target)).ContainsRefAndOrOutParameters());
		}

		[Test]
		public void ContainsOutArguments()
		{
			Assert.IsTrue(typeof(IHaveMethodWithOutArgument)
				.GetMethod(nameof(IHaveMethodWithOutArgument.Target)).ContainsRefAndOrOutParameters());
		}

		[Test]
		public void ContainsByValArguments()
		{
			Assert.IsFalse(typeof(IHaveMethodWithByValArgument)
				.GetMethod(nameof(IHaveMethodWithByValArgument.Target)).ContainsRefAndOrOutParameters());
		}

		[Test]
		public void GetDelegateCastWithNoArguments()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithNoArguments));
			Assert.AreEqual("Action", target.GetDelegateCast());
		}

		[Test]
		public void GetDelegateCastWithNoArgumentsAndReturnValue()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithNoArgumentsAndReturnValue));
			Assert.AreEqual("Func<Int32>", target.GetDelegateCast());
		}

		[Test]
		public void GetDelegateCastWithArguments()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithArguments));
			Assert.AreEqual("Action<Int32, String>", target.GetDelegateCast());
		}

		[Test]
		public void GetDelegateCastWithArgumentsAndReturnValue()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithArgumentsAndReturnValue));
			Assert.AreEqual("Func<Int32, String, Int32>", target.GetDelegateCast());
		}

		[Test]
		public void GetDelegateCastWithGenerics()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenerics));
			Assert.AreEqual("Action<Int32, U, String, V>", target.GetDelegateCast());
		}

		[Test]
		public void GetDelegateCastWithGenericsAndReturnValue()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenericsAndReturnValue));
			Assert.AreEqual("Func<Int32, U, String, V, U>", target.GetDelegateCast());
		}

		[Test]
		public void GetExpectationChecks()
		{
			var expectedExpectation =
@"(methodHandler.Expectations[""a""] as ArgumentExpectation<Int32>).IsValid(a, ""a"") && (methodHandler.Expectations[""c""] as ArgumentExpectation<String>).IsValid(c, ""c"")";
         var target = this.GetType().GetMethod(nameof(this.TargetWithArguments));
			var expectations = target.GetExpectationChecks();
			Assert.AreEqual(expectedExpectation, expectations, nameof(expectations));
		}

		[Test]
		public void GetMethodDescription()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithArguments));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
         Assert.AreEqual("void TargetWithArguments(Int32 a, String c)", description, nameof(description));
			Assert.AreEqual(1, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains(typeof(object).Namespace), nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionForInterfaceMethod()
		{
			var target = typeof(IMethodInfoExtensionsTests).GetMethod(nameof(IMethodInfoExtensionsTests.Target));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("void Target()", description, nameof(description));
			Assert.AreEqual(1, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains(typeof(object).Namespace), nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithOutArgument()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithOutArgument));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("void TargetWithOutArgument(out Int32 a)", description, nameof(description));
			Assert.AreEqual(1, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains(typeof(object).Namespace), nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithParamsArgument()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithParamsArgument));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("void TargetWithParamsArgument(params Int32[] a)", description, nameof(description));
			Assert.AreEqual(1, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains(typeof(object).Namespace), nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithRefArgument()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithRefArgument));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("void TargetWithRefArgument(ref Int32 a)", description, nameof(description));
			Assert.AreEqual(1, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains(typeof(object).Namespace), nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithReturnValue()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithArgumentsAndReturnValue));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("Int32 TargetWithArgumentsAndReturnValue(Int32 a, String c)", description, nameof(description));
			Assert.AreEqual(1, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains(typeof(object).Namespace), nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithGenericArguments()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenericsAndReturnValue));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("U TargetWithGenericsAndReturnValue<U, V>(Int32 a, U b, String c, V d)", description, nameof(description));
			Assert.AreEqual(2, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains(typeof(object).Namespace), nameof(namespaces.Contains));
			Assert.IsTrue(namespaces.Contains(this.GetType().Namespace), nameof(namespaces.Contains));
      }

		[Test]
		public void GetMethodDescriptionWithDefinedGenericArguments()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenericsAndReturnValue)).MakeGenericMethod(typeof(Guid), typeof(double));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("U TargetWithGenericsAndReturnValue<U, V>(Int32 a, U b, String c, V d)", description, nameof(description));
			Assert.AreEqual(2, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains(typeof(object).Namespace), nameof(namespaces.Contains));
			Assert.IsTrue(namespaces.Contains(this.GetType().Namespace), nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithArrayArgumentss()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithArrayArguments));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("void TargetWithArrayArguments(Int32[] a, String[] b, ref Guid[] c, out Double[] d)",
				description, nameof(description));
			Assert.AreEqual(1, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains(typeof(object).Namespace), nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithConstraints()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithMultipleConstraints));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("void TargetWithMultipleConstraints<U, V, W, X>(U a, V b, W c, X d) where U : class, new() where V : MethodInfoExtensionsTests.Source, MethodInfoExtensionsTests.ISource where W : struct where X : V", 
				description, nameof(description));
			Assert.AreEqual(2, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains(typeof(object).Namespace), nameof(namespaces.Contains));
			Assert.IsTrue(namespaces.Contains(this.GetType().Namespace), nameof(namespaces.Contains));
		}

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

		public void TargetWithNoArguments() { }
		public void TargetWithOutArgument(out int a) { a = 0; }
		public void TargetWithRefArgument(ref int a) { }
		public void TargetWithParamsArgument(params int[] a) { }
		public int TargetWithNoArgumentsAndReturnValue() { return 0; }
		public void TargetWithArguments(int a, string c) { }
		public void TargetWithArrayArguments(int[] a, string[] b, ref Guid[] c, out double[] d) { d = null; }
		public int TargetWithArgumentsAndReturnValue(int a, string c) { return 0; }
		public void TargetWithGenerics<U, V>(int a, U b, string c, V d) { }
		public U TargetWithGenericsAndReturnValue<U, V>(int a, U b, string c, V d) { return default(U); }
		public void TargetWithMultipleConstraints<U, V, W, X>(U a, V b, W c, X d) where U : class, new() where V : Source, ISource where W : struct where X : V { }

		public interface ISource { }
		public class Source { }
	}

	public interface IMethodInfoExtensionsTests
	{
		void Target();
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
