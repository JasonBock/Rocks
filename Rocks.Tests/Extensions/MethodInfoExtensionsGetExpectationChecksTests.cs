using NUnit.Framework;
using System;
using static Rocks.Extensions.MethodInfoExtensions;

namespace Rocks.Tests.Extensions
{
	public sealed class MethodInfoExtensionsGetExpectationChecksTests
	{
		[Test]
		public void GetExpectationChecks()
		{
			var expectedExpectation =
@"((R.ArgumentExpectation<int>)methodHandler.Expectations[""a""]).IsValid(a) && ((R.ArgumentExpectation<string>)methodHandler.Expectations[""c""]).IsValid(c)";
			var target = this.GetType().GetMethod(nameof(this.TargetWithArguments))!;
			var expectations = target.GetExpectationChecks();
			Assert.That(expectations, Is.EqualTo(expectedExpectation), nameof(expectations));
		}

		[Test]
		public void GetExpectationChecksWithPointerTypes()
		{
			var expectedExpectation =
@"((R.ArgumentExpectation<int>)methodHandler.Expectations[""a""]).IsValid(a) && ((R.ArgumentExpectation<string>)methodHandler.Expectations[""c""]).IsValid(c)";
			var target = this.GetType().GetMethod(nameof(this.TargetWithPointers))!;
			var expectations = target.GetExpectationChecks();
			Assert.That(expectations, Is.EqualTo(expectedExpectation), nameof(expectations));
		}

		[Test]
		public void GetExpectationChecksWithGenericTypes()
		{
			var expectedExpectation =
@"((R.ArgumentExpectation<IEquatable<int>>)methodHandler.Expectations[""a""]).IsValid(a) && ((R.ArgumentExpectation<string>)methodHandler.Expectations[""c""]).IsValid(c)";
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenerics))!;
			var expectations = target.GetExpectationChecks();
			Assert.That(expectations, Is.EqualTo(expectedExpectation), nameof(expectations));
		}

		public void TargetWithArguments(int a, string c) { }
		public void TargetWithGenerics(IEquatable<int> a, string c) { }
		public unsafe void TargetWithPointers(int a, Guid* b, string c) { }
	}
}