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
			var target = this.GetType().GetMethod(nameof(this.TargetWithArguments));
			Assert.AreEqual("a, c", target.GetArgumentNameList());
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
@"(handler.Expectations[""a""] as ArgumentExpectation<Int32>).Validate(a, ""a"");
(handler.Expectations[""c""] as ArgumentExpectation<String>).Validate(c, ""c"");";
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
			Assert.IsTrue(namespaces.Contains("System"), nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithOutArgument()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithOutArgument));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("void TargetWithOutArgument(out Int32 a)", description, nameof(description));
			Assert.AreEqual(1, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains("System"), nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithRefArgument()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithRefArgument));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("void TargetWithRefArgument(ref Int32 a)", description, nameof(description));
			Assert.AreEqual(1, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains("System"), nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithReturnValue()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithArgumentsAndReturnValue));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("Int32 TargetWithArgumentsAndReturnValue(Int32 a, String c)", description, nameof(description));
			Assert.AreEqual(1, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains("System"), nameof(namespaces.Contains));
		}

		[Test]
		public void GetMethodDescriptionWithGenericArguments()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenericsAndReturnValue));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("U TargetWithGenericsAndReturnValue<U, V>(Int32 a, U b, String c, V d)", description, nameof(description));
			Assert.AreEqual(2, namespaces.Count, nameof(namespaces.Count));
			Assert.IsTrue(namespaces.Contains("System"), nameof(namespaces.Contains));
			Assert.IsTrue(namespaces.Contains("Rocks.Tests.Extensions"), nameof(namespaces.Contains));
      }

		[Test]
		public void GetMethodDescriptionWithDefinedGenericArguments()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenericsAndReturnValue)).MakeGenericMethod(typeof(Guid), typeof(double));
			var namespaces = new SortedSet<string>();
			var description = target.GetMethodDescription(namespaces);
			Assert.AreEqual("U TargetWithGenericsAndReturnValue<U, V>(Int32 a, U b, String c, V d)", description, nameof(description));
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

		public void TargetWithNoArguments() { }
		public void TargetWithOutArgument(out int a) { a = 0; }
		public void TargetWithRefArgument(ref int a) { }
		public int TargetWithNoArgumentsAndReturnValue() { return 0; }
		public void TargetWithArguments(int a, string c) { }
		public int TargetWithArgumentsAndReturnValue(int a, string c) { return 0; }
		public void TargetWithGenerics<U, V>(int a, U b, string c, V d) { }
		public U TargetWithGenericsAndReturnValue<U, V>(int a, U b, string c, V d) { return default(U); }
		public void TargetWithMultipleConstraints<U, V, W, X>(U a, V b, W c, X d) where U : class, new() where V : Source, ISource where W : struct where X : V { }

		public interface ISource { }
		public class Source { }
	}
}
