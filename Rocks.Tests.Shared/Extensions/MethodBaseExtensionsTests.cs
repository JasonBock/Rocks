using NUnit.Framework;
using static Rocks.Extensions.MethodBaseExtensions;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Reflection;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodBaseExtensionsTests
	{
		[Test]
		public void GetArgumentNameList()
		{
			var target = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithArguments));
			Assert.That(target.GetArgumentNameList(), Is.EqualTo("a, c"));
		}

		[Test]
		public void GetArgumentNameListWithParams()
		{
			var target = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithParamsArgument));
			Assert.That(target.GetArgumentNameList(), Is.EqualTo("a"));
		}

		[Test]
		public void GetLiteralArgumentNameList()
		{
			var target = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithArguments));
			Assert.That(target.GetLiteralArgumentNameList(), Is.EqualTo("{a}, {c}"));
		}

		[Test]
		public void GetGenericArguments()
		{
			var namespaces = new SortedSet<string>();
			var target = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithGenerics));
			var arguments = target.GetGenericArguments(namespaces);

         Assert.That(arguments.Arguments, Is.EqualTo("<T, U>"), nameof(arguments.Arguments));
			Assert.That(arguments.Constraints, Is.EqualTo("where T : new() where U : T"), nameof(arguments.Constraints));
			Assert.That(namespaces.Count, Is.EqualTo(1), nameof(namespaces.Count));
			Assert.That(namespaces.Contains(this.GetType().Namespace), nameof(namespaces.Contains), Is.True);
		}

		[Test]
		public void GetGenericArgumentsForMethodWithNoGenerics()
		{
			var namespaces = new SortedSet<string>();
			var target = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithArguments));
			var arguments = target.GetGenericArguments(namespaces);

			Assert.That(arguments.Arguments, Is.Empty, nameof(arguments.Arguments));
			Assert.That(arguments.Constraints, Is.Empty, nameof(arguments.Constraints));
			Assert.That(namespaces.Count, Is.EqualTo(0), nameof(namespaces.Count));
		}

		public void TargetWithOutArrayAttribute([Out] string[] a) { }
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
