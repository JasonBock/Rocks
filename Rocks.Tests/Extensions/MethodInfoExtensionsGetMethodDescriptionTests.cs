using NUnit.Framework;
using System;
using System.Collections.Generic;
using static Rocks.Extensions.MethodInfoExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodInfoExtensionsGetMethodDescriptionTests
	{
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
			Assert.AreEqual("void TargetWithMultipleConstraints<U, V, W, X>(U a, V b, W c, X d) where U : class, new() where V : MethodInfoExtensionsGetMethodDescriptionTests.Source, MethodInfoExtensionsGetMethodDescriptionTests.ISource where W : struct where X : V",
				description, nameof(description));
			Assert.AreEqual(2, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains(typeof(object).Namespace), nameof(namespaces.Contains));
			Assert.IsTrue(namespaces.Contains(this.GetType().Namespace), nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithComplexGenericReturnTypeAndArgumentTypes()
		{
			var target = typeof(HaveMethodWithComplexGenericReturnType<>).GetMethod(nameof(HaveMethodWithComplexGenericReturnType<int>.Target));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("IEnumerable<KeyValuePair<Int64, TSource>> Target(IEnumerable<KeyValuePair<Int64, TSource>> a)", description, nameof(description));
			Assert.AreEqual(3, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains(typeof(object).Namespace), nameof(namespaces.Contains));
			Assert.IsTrue(namespaces.Contains(this.GetType().Namespace), nameof(namespaces.Contains));
			Assert.IsTrue(namespaces.Contains(typeof(IEnumerable<>).Namespace), nameof(namespaces.Contains));
		}

		public void TargetWithNoArguments() { }
		public int TargetWithNoArgumentsAndReturnValue() { return 0; }
		public void TargetWithArguments(int a, string c) { }
		public int TargetWithArgumentsAndReturnValue(int a, string c) { return 0; }
		public void TargetWithGenerics<U, V>(int a, U b, string c, V d) { }
		public U TargetWithGenericsAndReturnValue<U, V>(int a, U b, string c, V d) { return default(U); }
		public void TargetWithOutArgument(out int a) { a = 0; }
		public void TargetWithRefArgument(ref int a) { }
		public void TargetWithParamsArgument(params int[] a) { }
		public void TargetWithArrayArguments(int[] a, string[] b, ref Guid[] c, out double[] d) { d = null; }
		public void TargetWithMultipleConstraints<U, V, W, X>(U a, V b, W c, X d) where U : class, new() where V : Source, ISource where W : struct where X : V { }

		public interface ISource { }
		public class Source { }
	}

	public interface IMethodInfoExtensionsTests
	{
		void Target();
	}

	public class HaveMethodWithComplexGenericReturnType<TSource>
	{
		public virtual IEnumerable<KeyValuePair<long, TSource>> Target(IEnumerable<KeyValuePair<long, TSource>> a) { return null; }
   }
}
