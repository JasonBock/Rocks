using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static Rocks.Extensions.MethodBaseExtensions;
using System;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodBaseExtensionsGetParametersTests
	{
		[Test]
		public void GetParameters()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("Int32 a, String c", this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithArguments)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithGenerics()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("T a, String b, U c", this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithGenerics)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithParams()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("params Int32[] a", this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithParamsArgument)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithUnsafeArgumentTypes()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("Byte* a", this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithUnsafeArguments)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithArrayTypes()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("Byte[] a", this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithArray)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithArrayOfPointerTypes()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("Byte*[] a", this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithArrayOfPointers)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithOutArray()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("out Byte[] a", this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithOutArray)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithOutArrayOfPointers()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("out Byte*[] a", this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithOutArrayOfPointers)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithInOutAttributes()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("[In, Out]Char[] buffer, Int32 index, Int32 count", this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithInOut)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithOutAttributeArray()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("[Out]String[] a", this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithOutArrayAttribute)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithComplexGenericArguments()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("UsedByBase<TKey, TValue>[] a", typeof(TargetUsingBase<,>).GetMethod("Target").GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithOptionalArguments()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("Int32 a = 0, String c = \"c\", String d = null", this.GetType().GetMethod(nameof(this.TargetWithOptionalArguments)).GetParameters(namespaces));
		}

		public void TargetWithOutArrayAttribute([Out] string[] a) { }
		public int TargetWithInOut([In, Out] char[] buffer, int index, int count) { return 0; }
		public void TargetWithArguments(int a, string c) { }
		public void TargetWithOptionalArguments(int a = 0, string c = "c", string d = null) { }
		public void TargetWithGenerics<T, U>(T a, string b, U c) where T : new() where U : T { }
		public void TargetWithParamsArgument(params int[] a) { }
		public unsafe void TargetWithUnsafeArguments(byte* a) { }
		public void TargetWithArray(byte[] a) { }
		public unsafe void TargetWithArrayOfPointers(byte*[] a) { }
		public void TargetWithOutArray(out byte[] a) { a = null; }
		public unsafe void TargetWithOutArrayOfPointers(out byte*[] a) { a = null; }
	}

	public class TargetUsingBase<TKey, TValue> : IBase<UsedByBase<TKey, TValue>>
	{
		public void Target(UsedByBase<TKey, TValue>[] a)
		{
			throw new NotImplementedException();
		}
	}

	public class UsedByBase<TKey, TValue> { }

	public interface IBase<T>
	{
		void Target(T[] a);
	}
}
