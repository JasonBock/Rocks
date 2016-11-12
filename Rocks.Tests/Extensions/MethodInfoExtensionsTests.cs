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
			Assert.That(this.GetType().GetMethod(
				nameof(this.TargetWithOutArgument)).GetOutInitializers(), 
				Is.EqualTo("a = default(Int32);"));
		}

		[Test]
		public void ContainsOutInitializersWhenArgumentTypeIsArray()
		{
			Assert.That(this.GetType().GetMethod(
				nameof(this.TargetWithOutArrayArgument)).GetOutInitializers(),
				Is.EqualTo("a = default(Int32[]);"));
		}

		public void TargetWithOutArgument(out int a) { a = 0; }
		public void TargetWithOutArrayArgument(out int[] a) { a = null; }
	}
}
