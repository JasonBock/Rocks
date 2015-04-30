using NUnit.Framework;
using static Rocks.Extensions.MethodInfoExtensions;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodInfoExtensionsTests
	{
		[Test]
		public void ContainsOutInitializers()
		{
			Assert.AreEqual("a = default(Int32);", this.GetType()
				.GetMethod(nameof(this.TargetWithOutArgument)).GetOutInitializers());
		}

		[Test]
		public void GetExpectationChecks()
		{
			var expectedExpectation =
@"(methodHandler.Expectations[""a""] as ArgumentExpectation<Int32>).IsValid(a, ""a"") && (methodHandler.Expectations[""c""] as ArgumentExpectation<String>).IsValid(c, ""c"")";
         var target = this.GetType().GetMethod(nameof(this.TargetWithArguments));
			var expectations = target.GetExpectationChecks();
			Assert.AreEqual(expectedExpectation, expectations, nameof(expectations));
		}

		public void TargetWithOutArgument(out int a) { a = 0; }
		public void TargetWithArguments(int a, string c) { }
	}
}
