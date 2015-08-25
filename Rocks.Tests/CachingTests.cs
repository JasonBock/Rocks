using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class CachingTests
	{
		[Test]
		public void CreateTwoMocksWithCaching()
		{
			var rock1 = Rock.Create<IWillBeCached>(new Options(caching: CachingOptions.UseCache));
			rock1.Handle(_ => _.Target(Arg.IsAny<string>()));

			var mock1 = rock1.Make();

			var rock2 = Rock.Create<IWillBeCached>(new Options(caching: CachingOptions.UseCache));
			rock2.Handle(_ => _.Target(Arg.IsAny<string>()));

			var mock2 = rock2.Make();

			Assert.AreEqual(mock2.GetType(), mock1.GetType());
		}

		[Test]
		public void CreateTwoMocksWithoutCaching()
		{
			var rock1 = Rock.Create<IWillNotBeCached>(new Options(caching: CachingOptions.UseCache));
			rock1.Handle(_ => _.Target(Arg.IsAny<string>()));

			var mock1 = rock1.Make();

			var rock2 = Rock.Create<IWillNotBeCached>(new Options(caching: CachingOptions.GenerateNewVersion));
			rock2.Handle(_ => _.Target(Arg.IsAny<string>()));

			var mock2 = rock2.Make();

			Assert.AreNotEqual(mock2.GetType(), mock1.GetType());
		}
	}

	public interface IWillBeCached
	{
		void Target(string a);
	}

	public interface IWillNotBeCached
	{
		void Target(string a);
	}
}
