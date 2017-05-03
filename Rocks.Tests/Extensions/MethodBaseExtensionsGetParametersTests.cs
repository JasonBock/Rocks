using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static Rocks.Extensions.MethodBaseExtensions;
using System;
using System.Reflection;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodBaseExtensionsGetParametersTests
	{
		[Test]
		public void GetParameters()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetTypeInfo().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithArguments)).GetParameters(namespaces),
				Is.EqualTo("Int32 a, String c"));
		}

		[Test]
		public void GetParametersWithGenerics()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetTypeInfo().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithGenerics)).GetParameters(namespaces),
				Is.EqualTo("T a, String b, U c"));
		}

		[Test]
		public void GetParametersWithParams()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetTypeInfo().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithParamsArgument)).GetParameters(namespaces),
				Is.EqualTo("params Int32[] a"));
		}

		[Test]
		public void GetParametersWithUnsafeArgumentTypes()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetTypeInfo().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithUnsafeArguments)).GetParameters(namespaces),
				Is.EqualTo("Byte* a"));
		}

		[Test]
		public void GetParametersWithArrayTypes()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetTypeInfo().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithArray)).GetParameters(namespaces),
				Is.EqualTo("Byte[] a"));
		}

		[Test]
		public void GetParametersWithArrayOfPointerTypes()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetTypeInfo().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithArrayOfPointers)).GetParameters(namespaces),
				Is.EqualTo("Byte*[] a"));
		}

		[Test]
		public void GetParametersWithOutArray()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetTypeInfo().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithOutArray)).GetParameters(namespaces),
				Is.EqualTo("out Byte[] a"));
		}

		[Test]
		public void GetParametersWithOutArrayOfPointers()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetTypeInfo().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithOutArrayOfPointers)).GetParameters(namespaces),
				Is.EqualTo("out Byte*[] a"));
		}

		[Test]
		public void GetParametersWithInOutAttributes()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetTypeInfo().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithInOut)).GetParameters(namespaces),
				Is.EqualTo("[In, Out]Char[] buffer, Int32 index, Int32 count"));
		}

		[Test]
		public void GetParametersWithOutAttributeArray()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetTypeInfo().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithOutArrayAttribute)).GetParameters(namespaces),
				Is.EqualTo("[Out]String[] a"));
		}

		[Test]
		public void GetParametersWithComplexGenericArguments()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(typeof(TargetUsingBase<,>).GetTypeInfo().GetMethod("Target").GetParameters(namespaces),
				Is.EqualTo("UsedByBase<TKey, TValue>[] a"));
		}

		[Test]
		public void GetParametersWithOptionalArguments()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithOptionalArguments)).GetParameters(namespaces),
				Is.EqualTo("Int32 a = 0, String c = \"c\", String d = null"));
		}

		public void TargetWithOutArrayAttribute([Out] string[] a) { }
		public int TargetWithInOut([In, Out] char[] buffer, int index, int count) => 0; 
		public void TargetWithArguments(int a, string c) { }
		public void TargetWithOptionalArguments(int a = 0, string c = "c", string d = null) { }
		public void TargetWithGenerics<T, U>(T a, string b, U c) where T : new() where U : T { }
		public void TargetWithParamsArgument(params int[] a) { }
		public unsafe void TargetWithUnsafeArguments(byte* a) { }
		public void TargetWithArray(byte[] a) { }
		public unsafe void TargetWithArrayOfPointers(byte*[] a) { }
		public void TargetWithOutArray(out byte[] a) => a = null; 
		public unsafe void TargetWithOutArrayOfPointers(out byte*[] a) => a = null; 
	}

	public class TargetUsingBase<TKey, TValue> : IBase<UsedByBase<TKey, TValue>>
	{
		public void Target(UsedByBase<TKey, TValue>[] a) =>
			throw new NotImplementedException();
	}

	public class UsedByBase<TKey, TValue> { }

	public interface IBase<T>
	{
		void Target(T[] a);
	}
}
