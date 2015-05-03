using NUnit.Framework;
using static Rocks.Extensions.MethodBaseExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodBaseExtensionsGetExpectationsExceptionMessageTests
	{
		[Test]
		public void GetExpectationExceptionMessage()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenerics));
			Assert.AreEqual("TargetWithGenerics<T, U>({a}, {b}, {c})", target.GetExpectationExceptionMessage());
		}

		[Test]
		public void GetExpectationExceptionMessageWithPointerTypesInArguments()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithPointers));
			Assert.AreEqual("TargetWithPointers(Int32* a)", target.GetExpectationExceptionMessage());
		}

		public void TargetWithGenerics<T, U>(T a, string b, U c) where T : new() where U : T { }
		public unsafe void TargetWithPointers(int* a) { }
	}
}
