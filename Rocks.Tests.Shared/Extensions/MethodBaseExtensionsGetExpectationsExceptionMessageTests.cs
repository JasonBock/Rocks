using NUnit.Framework;
using System.Reflection;
using static Rocks.Extensions.MethodBaseExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodBaseExtensionsGetExpectationsExceptionMessageTests
	{
		[Test]
		public void GetExpectationExceptionMessage()
		{
			var target = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithGenerics));
			Assert.That(target.GetExpectationExceptionMessage(), 
				Is.EqualTo("TargetWithGenerics<T, U>({a}, {b}, {c})"));
		}

		[Test]
		public void GetExpectationExceptionMessageWithPointerTypesInArguments()
		{
			var target = this.GetType().GetTypeInfo().GetMethod(nameof(this.TargetWithPointers));
			Assert.That(target.GetExpectationExceptionMessage(),
				Is.EqualTo("TargetWithPointers(Int32* a)"));
		}

		public void TargetWithGenerics<T, U>(T a, string b, U c) where T : new() where U : T { }
		public unsafe void TargetWithPointers(int* a) { }
	}
}
