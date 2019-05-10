using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using static Rocks.Extensions.MethodBaseExtensions;

namespace Rocks.Tests.Extensions
{
	public sealed class MethodBaseExtensionsGetParametersTests
	{
		[Test]
		public void GetParameters()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithArguments)).GetParameters(namespaces),
				Is.EqualTo("int a, string c"));
		}

		[Test]
		public void GetParametersWithGenerics()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithGenerics)).GetParameters(namespaces),
				Is.EqualTo("T a, string b, U c"));
		}

		[Test]
		public void GetParametersWithParams()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithParamsArgument)).GetParameters(namespaces),
				Is.EqualTo("params int[] a"));
		}

		[Test]
		public void GetParametersWithUnsafeArgumentTypes()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithUnsafeArguments)).GetParameters(namespaces),
				Is.EqualTo("byte* a"));
		}

		[Test]
		public void GetParametersWithArrayTypes()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithArray)).GetParameters(namespaces),
				Is.EqualTo("byte[] a"));
		}

		[Test]
		public void GetParametersWithArrayOfPointerTypes()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithArrayOfPointers)).GetParameters(namespaces),
				Is.EqualTo("byte*[] a"));
		}

		[Test]
		public void GetParametersWithOutArray()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithOutArray)).GetParameters(namespaces),
				Is.EqualTo("out byte[] a"));
		}

		[Test]
		public void GetParametersWithOutArrayOfPointers()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithOutArrayOfPointers)).GetParameters(namespaces),
				Is.EqualTo("out byte*[] a"));
		}

		[Test]
		public void GetParametersWithInOutAttributes()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithInOut)).GetParameters(namespaces),
				Is.EqualTo("[In, Out]char[] buffer, int index, int count"));
		}

		[Test]
		public void GetParametersWithOutAttributeArray()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetMethod(nameof(MethodBaseExtensionsGetParametersTests.TargetWithOutArrayAttribute)).GetParameters(namespaces),
				Is.EqualTo("[Out]string[] a"));
		}

		[Test]
		public void GetParametersWithComplexGenericArguments()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(typeof(TargetUsingBase<,>).GetMethod("Target").GetParameters(namespaces),
				Is.EqualTo("UsedByBase<TKey, TValue>[] a"));
		}

		[Test]
		public void GetParametersWithOptionalArguments()
		{
			var namespaces = new SortedSet<string>();
			Assert.That(this.GetType().GetMethod(nameof(this.TargetWithOptionalArguments)).GetParameters(namespaces),
				Is.EqualTo("int a = 0, string c = \"c\", string? d = null"));
		}

		public void TargetWithOutArrayAttribute([Out] string[] a) { }
		public int TargetWithInOut([In, Out] char[] buffer, int index, int count) => 0; 
		public void TargetWithArguments(int a, string c) { }
		public void TargetWithOptionalArguments(int a = 0, string c = "c", string? d = null) { }
		public void TargetWithGenerics<T, U>(T a, string b, U c) where T : new() where U : T { }
		public void TargetWithParamsArgument(params int[] a) { }
		public unsafe void TargetWithUnsafeArguments(byte* a) { }
		public void TargetWithArray(byte[] a) { }
		public unsafe void TargetWithArrayOfPointers(byte*[] a) { }
		public void TargetWithOutArray(out byte[] a) => a = Array.Empty<byte>(); 
		public unsafe void TargetWithOutArrayOfPointers(out byte*[] a) => a = new byte*[0]; 
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
