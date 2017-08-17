using NUnit.Framework;
using Rocks.Extensions;

namespace Rocks.Tests.Extensions
{
	[TestFixture]
	public sealed class BoolExtensionsTests
	{
		[Test]
		public void GetTrueValue() => Assert.That(true.GetValue(), Is.EqualTo("true"));

		[Test]
		public void GetFalseValue() => Assert.That(false.GetValue(), Is.EqualTo("false"));
	}
}
