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
		public void ContainsOutInitializersWhenArgumentTypeIsArray()
		{
			Assert.AreEqual("a = default(Int32[]);", this.GetType()
				.GetMethod(nameof(this.TargetWithOutArrayArgument)).GetOutInitializers());
		}

		public void TargetWithOutArgument(out int a) { a = 0; }
		public void TargetWithOutArrayArgument(out int[] a) { a = null; }
	}
}
