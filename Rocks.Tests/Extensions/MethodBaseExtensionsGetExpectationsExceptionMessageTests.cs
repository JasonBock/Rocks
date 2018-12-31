using NUnit.Framework;
using static Rocks.Extensions.MethodBaseExtensions;

namespace Rocks.Tests.Extensions
{
	public sealed class MethodBaseExtensionsGetExpectationsExceptionMessageTests
	{
		[Test]
		public void GetExpectationExceptionMessage()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithGenerics));
			Assert.That(target.GetExpectationExceptionMessage(), 
				Is.EqualTo("TargetWithGenerics<T, U>({a}, {b}, {c})"));
		}

		[Test]
		public void GetExpectationExceptionMessageWithPointerTypesInArguments()
		{
			var target = this.GetType().GetMethod(nameof(this.TargetWithPointers));
			Assert.That(target.GetExpectationExceptionMessage(),
				Is.EqualTo("TargetWithPointers(int* a)"));
		}

		public void TargetWithGenerics<T, U>(T a, string b, U c) where T : new() where U : T { }
		public unsafe void TargetWithPointers(int* a) { }
	}
}
