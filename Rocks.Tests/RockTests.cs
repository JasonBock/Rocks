using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class RockTests
	{
		[Test]
		public void CreateWhenTypeIsSealed()
		{
			Assert.Throws<RockValidationException>(() => Rock.Create<string>());
		}

		[Test]
		public void TryCreateWhenTypeIsSealed()
		{
			Rock<string> result = null;
			Assert.IsFalse(Rock.TryCreate<string>(out result));
		}
	}
}
