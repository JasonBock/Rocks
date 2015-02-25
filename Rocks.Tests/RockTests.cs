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
			Assert.Throws<ValidationException>(() => Rock.Create<string>());
		}

		[Test]
		public void TryCreateWhenTypeIsSealed()
		{
			var result = Rock.TryCreate<string>();
         Assert.IsFalse(result.IsSuccessful);
		}
	}
}
