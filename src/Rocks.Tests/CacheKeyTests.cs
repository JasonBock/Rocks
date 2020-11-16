using NUnit.Framework;
using Rocks.Options;
using System;

namespace Rocks.Tests
{
	public sealed class CacheKeyTests
	{
		[Test]
		public void CompareHashCodesWhenEqual()
		{
			var cacheKey1 = new CacheKey(this.GetType(),
				new RockOptions(OptimizationSetting.Release, CodeFileOption.Create, allowWarning: AllowWarning.No));
			var cacheKey2 = new CacheKey(this.GetType(),
				new RockOptions(OptimizationSetting.Release, CodeFileOption.Create, allowWarning: AllowWarning.No));

			Assert.That(cacheKey2, Is.EqualTo(cacheKey1));
		}

		[Test]
		public void CompareHashCodesWhenNotEqualViaDifferentOptions()
		{
			var cacheKey1 = new CacheKey(this.GetType(),
				new RockOptions(OptimizationSetting.Release, CodeFileOption.Create, allowWarning: AllowWarning.No));
			var cacheKey2 = new CacheKey(this.GetType(),
				new RockOptions(OptimizationSetting.Release, CodeFileOption.None, allowWarning: AllowWarning.No));

			Assert.That(cacheKey2, Is.Not.EqualTo(cacheKey1));
		}

		[Test]
		public void CompareHashCodesWhenNotEqualViaDifferentTypes()
		{
			var cacheKey1 = new CacheKey(this.GetType(),
				new RockOptions(OptimizationSetting.Release, CodeFileOption.Create, allowWarning: AllowWarning.No));
			var cacheKey2 = new CacheKey(typeof(Guid),
				new RockOptions(OptimizationSetting.Release, CodeFileOption.Create, allowWarning: AllowWarning.No));

			Assert.That(cacheKey2, Is.Not.EqualTo(cacheKey1));
		}
	}
}
