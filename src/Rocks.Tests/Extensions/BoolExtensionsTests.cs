using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions
{
	public static class BoolExtensionsTests
	{
		[Test]
		public static void GetTrueValue() => Assert.That(true.GetValue(), Is.EqualTo("true"));

		[Test]
		public static void GetFalseValue() => Assert.That(false.GetValue(), Is.EqualTo("false"));
	}
}
