using NUnit.Framework;
using Rocks.Extensions;
using System;
using System.Collections.Generic;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodInfoExtensionsTests
	{
		[Test]
		public void GetArgumentNameList()
		{
			var target = this.GetType().GetMethod(nameof(this.Target));
			Assert.AreEqual("a, c", target.GetArgumentNameList());
		}

		[Test]
		public void GetMethodDescription()
		{
			var target = this.GetType().GetMethod(nameof(this.Target));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
         Assert.AreEqual("void Target(Int32 a, String c)", description, nameof(description));
			Assert.AreEqual(1, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains("System"), nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithReturnValue()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithReturnValue));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("Int32 TargetWithReturnValue(Int32 a, String c)", description, nameof(description));
			Assert.AreEqual(1, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains("System"), nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithGenericArguments()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenerics));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("U TargetWithGenerics<U, V>(Int32 a, U b, String c, V d)", description, nameof(description));
			Assert.AreEqual(2, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains("System"), nameof(namespaces.Contains));
			Assert.IsTrue(namespaces.Contains("Rocks.Tests.Extensions"), nameof(namespaces.Contains));
      }

		[Test]
		public void GetMethodDescriptionWithDefinedGenericArguments()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenerics)).MakeGenericMethod(typeof(Guid), typeof(double));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("U TargetWithGenerics<U, V>(Int32 a, U b, String c, V d)", description, nameof(description));
			Assert.AreEqual(2, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains("System"), nameof(namespaces.Contains));
			Assert.IsTrue(namespaces.Contains("Rocks.Tests.Extensions"), nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithConstraints()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithMultipleConstraints));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("void TargetWithMultipleConstraints<U, V, W, X>(U a, V b, W c, X d) where U : class, new() where V : Source, ISource where W : struct where X : V", 
				description, nameof(description));
			Assert.AreEqual(2, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains("System"), nameof(namespaces.Contains));
			Assert.IsTrue(namespaces.Contains("Rocks.Tests.Extensions"), nameof(namespaces.Contains));
		}

		public void Target(int a, string c) { }
		public int TargetWithReturnValue(int a, string c) { return 0; }
		public U TargetWithGenerics<U, V>(int a, U b, string c, V d) { return default(U); }
		public void TargetWithMultipleConstraints<U, V, W, X>(U a, V b, W c, X d) where U : class, new() where V : Source, ISource where W : struct where X : V { }

		public interface ISource { }
		public class Source { }
	}
}
