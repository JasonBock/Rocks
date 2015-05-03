using NUnit.Framework;
using static Rocks.Extensions.MethodBaseExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodBaseExtensionsIsExternTests
	{
		[Test]
		public void IsExtern()
		{
			Assert.IsFalse(this.GetType().GetMethod(nameof(MethodBaseExtensionsIsExternTests.IsExtern)).IsExtern());
		}

		[Test]
		public void IsExternForGetType()
		{
			Assert.IsTrue(typeof(object).GetMethod(nameof(object.GetType)).IsExtern());
		}
	}
}
