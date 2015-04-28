using NUnit.Framework;
using static Rocks.Extensions.MethodBaseExtensions;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodBaseExtensionTests
	{
		[Test]
		public void GetArgumentNameList()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithArguments));
			Assert.AreEqual("a, c", target.GetArgumentNameList());
		}

		[Test]
		public void GetArgumentNameListWithParams()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithParamsArgument));
			Assert.AreEqual("a", target.GetArgumentNameList());
		}

		[Test]
		public void GetLiteralArgumentNameList()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithArguments));
			Assert.AreEqual("{a}, {c}", target.GetLiteralArgumentNameList());
		}

		[Test]
		public void GetExpectationExceptionMessage()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenerics));
			Assert.AreEqual("TargetWithGenerics<T, U>({a}, {b}, {c})", target.GetExpectationExceptionMessage());
		}

		[Test]
		public void GetGenericArguments()
		{
			var namespaces = new SortedSet<string>();
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenerics));
			var arguments = target.GetGenericArguments(namespaces);

         Assert.AreEqual("<T, U>", arguments.Arguments, nameof(arguments.Arguments));
			Assert.AreEqual("where T : new() where U : T", arguments.Constraints, nameof(arguments.Constraints));
			Assert.AreEqual(1, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains(this.GetType().Namespace), nameof(namespaces.Contains));
		}

		[Test]
		public void GetGenericArgumentsForMethodWithNoGenerics()
		{
			var namespaces = new SortedSet<string>();
			var target = this.GetType().GetMethod(nameof(this.TargetWithArguments));
			var arguments = target.GetGenericArguments(namespaces);

			Assert.AreEqual(string.Empty, arguments.Arguments, nameof(arguments.Arguments));
			Assert.AreEqual(string.Empty, arguments.Constraints, nameof(arguments.Constraints));
			Assert.AreEqual(0, namespaces.Count, nameof(namespaces.Count));
		}

		[Test]
		public void GetParameters()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("Int32 a, String c", this.GetType().GetMethod(nameof(MethodBaseExtensionTests.TargetWithArguments)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithGenerics()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("T a, String b, U c", this.GetType().GetMethod(nameof(MethodBaseExtensionTests.TargetWithGenerics)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithParams()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("params Int32[] a", this.GetType().GetMethod(nameof(MethodBaseExtensionTests.TargetWithParamsArgument)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithUnsafeArgumentTypes()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("Byte* a", this.GetType().GetMethod(nameof(MethodBaseExtensionTests.TargetWithUnsafeArguments)).GetParameters(namespaces));
      }

		[Test]
		public void GetParametersWithArrayTypes()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("Byte[] a", this.GetType().GetMethod(nameof(MethodBaseExtensionTests.TargetWithArray)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithArrayOfPointerTypes()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("Byte*[] a", this.GetType().GetMethod(nameof(MethodBaseExtensionTests.TargetWithArrayOfPointers)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithOutArray()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("out Byte[] a", this.GetType().GetMethod(nameof(MethodBaseExtensionTests.TargetWithOutArray)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithOutArrayOfPointers()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("out Byte*[] a", this.GetType().GetMethod(nameof(MethodBaseExtensionTests.TargetWithOutArrayOfPointers)).GetParameters(namespaces));
		}

		[Test]
		public void GetParametersWithInOutAttributes()
		{
			var namespaces = new SortedSet<string>();
			Assert.AreEqual("Char[] buffer, Int32 index, Int32 count", this.GetType().GetMethod(nameof(MethodBaseExtensionTests.TargetWithInOut)).GetParameters(namespaces));
		}

		public int TargetWithInOut([In, Out] char[] buffer, int index, int count) { return 0; }
		public void TargetWithArguments(int a, string c) { }
		public void TargetWithGenerics<T, U>(T a, string b, U c) where T : new() where U : T { }
		public void TargetWithParamsArgument(params int[] a) { }
		public unsafe void TargetWithUnsafeArguments(byte* a) { }
		public void TargetWithArray(byte[] a) { }
		public unsafe void TargetWithArrayOfPointers(byte*[] a) { }
		public void TargetWithOutArray(out byte[] a) { a = null; }
		public unsafe void TargetWithOutArrayOfPointers(out byte*[] a) { a = null; }
	}
}
