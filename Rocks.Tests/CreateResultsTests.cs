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
			Assert.IsTrue(result.IsSuccessful, nameof(result.IsSuccessful));
			Assert.AreSame(rock, result.Result, nameof(result.Result));
		}
	}

	public interface ICreateResultsTests
	{
		void Member();
	}
}
