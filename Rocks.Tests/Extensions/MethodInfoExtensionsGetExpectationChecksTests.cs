using NUnit.Framework;
using System;
using System.Reflection;
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
@"(methodHandler.Expectations[""a""] as R.ArgumentExpectation<int>).IsValid(a) && (methodHandler.Expectations[""c""] as R.ArgumentExpectation<string>).IsValid(c)";
			var target = this.GetType().GetMethod(nameof(this.TargetWithArguments));
			var expectations = target.GetExpectationChecks();
			Assert.That(expectations, Is.EqualTo(expectedExpectation), nameof(expectations));
		}

		[Test]
		public void GetExpectationChecksWithPointerTypes()
		{
			var expectedExpectation =
@"(methodHandler.Expectations[""a""] as R.ArgumentExpectation<int>).IsValid(a) && (methodHandler.Expectations[""c""] as R.ArgumentExpectation<string>).IsValid(c)";
			var target = this.GetType().GetMethod(nameof(this.TargetWithPointers));
			var expectations = target.GetExpectationChecks();
			Assert.That(expectations, Is.EqualTo(expectedExpectation), nameof(expectations));
		}

		[Test]
		public void GetExpectationChecksWithGenericTypes()
		{
			var expectedExpectation =
@"(methodHandler.Expectations[""a""] as R.ArgumentExpectation<IEquatable<int>>).IsValid(a) && (methodHandler.Expectations[""c""] as R.ArgumentExpectation<string>).IsValid(c)";
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenerics));
			var expectations = target.GetExpectationChecks();
			Assert.That(expectations, Is.EqualTo(expectedExpectation), nameof(expectations));
		}

		public void TargetWithArguments(int a, string c) { }
		public void TargetWithGenerics(IEquatable<int> a, string c) { }
		public unsafe void TargetWithPointers(int a, Guid* b, string c) { }
	}
}
