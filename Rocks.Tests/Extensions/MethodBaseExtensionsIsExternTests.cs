using NUnit.Framework;
using System.Reflection;
using static Rocks.Extensions.MethodBaseExtensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class MethodBaseExtensionsIsExternTests
	{
		[Test]
		public void IsExtern() =>
			Assert.That(this.GetType().GetTypeInfo().GetMethod(nameof(MethodBaseExtensionsIsExternTests.IsExtern)).IsExtern(), Is.False);

		[Test]
		public void IsExternForGetType() =>
			Assert.That(typeof(object).GetTypeInfo().GetMethod(nameof(object.GetType)).IsExtern(), Is.True);
	}
}
