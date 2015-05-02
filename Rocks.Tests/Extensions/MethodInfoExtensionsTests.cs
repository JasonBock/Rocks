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

		public void TargetWithOutArgument(out int a) { a = 0; }
	}
}
