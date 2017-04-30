using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class CreateResultsTests
	{
		[Test]
		public void Create()
		{
			var rock = Rock.Create<ICreateResultsTests>();
         var result = new CreateResult<ICreateResultsTests>(true, rock);
			Assert.That(result.IsSuccessful, Is.True, nameof(result.IsSuccessful));
			Assert.That(result.Result, Is.SameAs(rock), nameof(result.Result));
		}
	}

	public interface ICreateResultsTests
	{
		void Member();
	}
}
