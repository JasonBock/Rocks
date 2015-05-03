using NUnit.Framework;
using System;
using static Rocks.Extensions.MethodInfoExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodInfoExtensionsGetExpectationChecksTests
	{
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
		public void GetExpectationChecksWithPointerTypes()
		{
			var expectedExpectation =
@"(methodHandler.Expectations[""a""] as ArgumentExpectation<Int32>).IsValid(a, ""a"") && (methodHandler.Expectations[""c""] as ArgumentExpectation<String>).IsValid(c, ""c"")";
			var target = this.GetType().GetMethod(nameof(this.TargetWithPointers));
			var expectations = target.GetExpectationChecks();
			Assert.AreEqual(expectedExpectation, expectations, nameof(expectations));
		}

		[Test]
		public void GetExpectationChecksWithGenericTypes()
		{
			var expectedExpectation =
@"(methodHandler.Expectations[""a""] as ArgumentExpectation<IEquatable<Int32>>).IsValid(a, ""a"") && (methodHandler.Expectations[""c""] as ArgumentExpectation<String>).IsValid(c, ""c"")";
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenerics));
			var expectations = target.GetExpectationChecks();
			Assert.AreEqual(expectedExpectation, expectations, nameof(expectations));
		}

		public void TargetWithArguments(int a, string c) { }
		public void TargetWithGenerics(IEquatable<int> a, string c) { }
		public unsafe void TargetWithPointers(int a, Guid* b, string c) { }
	}
}
