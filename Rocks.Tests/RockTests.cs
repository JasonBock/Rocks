using NUnit.Framework;
using Rocks.Exceptions;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class RockTests
	{
		[Test]
		public void Create()
		{
			Assert.IsNotNull(Rock.Create<IRockTests>(), nameof(Rock.Create));
		}

		[Test]
		public void CreateWhenTypeIsSealed()
		{
			Assert.Throws<ValidationException>(() => Rock.Create<string>());
		}

		[Test]
		public void TryCreate()
		{
			var result = Rock.TryCreate<IRockTests>();
			Assert.IsTrue(result.IsSuccessful, nameof(result.IsSuccessful));
			Assert.IsNotNull(result.Result, nameof(result.Result));
		}

		[Test]
		public void TryCreateWhenTypeIsSealed()
		{
			var result = Rock.TryCreate<string>();
         Assert.IsFalse(result.IsSuccessful);
			Assert.IsNull(result.Result, nameof(result.Result));
		}
	}

	public interface IRockTests { }
}
