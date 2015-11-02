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
			Assert.IsTrue(result.IsSuccessful, nameof(result.IsSuccessful));
			Assert.AreSame(rock, result.Result, nameof(result.Result));
		}
	}

	public interface IMakeResultsTests
	{
		void Member();
	}
}
