using NUnit.Framework;
using static Rocks.Extensions.MethodBaseExtensions;

namespace Rocks.Tests.Extensions
{
	public sealed class MethodBaseExtensionsIsExternTests
	{
		[Test]
		public void IsExtern() =>
			Assert.That(this.GetType().GetMethod(nameof(MethodBaseExtensionsIsExternTests.IsExtern)).IsExtern(), Is.False);

		[Test]
		public void IsExternForGetType() =>
			Assert.That(typeof(object).GetMethod(nameof(object.GetType)).IsExtern(), Is.True);
	}
}
