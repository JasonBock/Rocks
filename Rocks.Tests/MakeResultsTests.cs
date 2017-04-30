using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class MakeResultsTests
	{
		[Test]
		public void Make()
		{
			var rock = Rock.Make<ICreateResultsTests>();
			var result = new MakeResult<ICreateResultsTests>(true, rock);
			Assert.That(result.IsSuccessful, Is.True, nameof(result.IsSuccessful));
			Assert.That(result.Result, Is.SameAs(rock), nameof(result.Result));
		}
	}

	public interface IMakeResultsTests
	{
		void Member();
	}
}
