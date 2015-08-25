using NUnit.Framework;
using Rocks.Options;
using System;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class CacheKeyTests
	{
		[Test]
		public void CompareHashCodesWhenEqual()
		{
			var cacheKey1 = new CacheKey(this.GetType(),
				new RockOptions(OptimizationSetting.Release, CodeFileOptions.Create, SerializationOptions.Supported));
			var cacheKey2 = new CacheKey(this.GetType(),
				new RockOptions(OptimizationSetting.Release, CodeFileOptions.Create, SerializationOptions.Supported));

			Assert.AreEqual(cacheKey1, cacheKey2);
		}

		[Test]
		public void CompareHashCodesWhenNotEqualViaDifferentOptions()
		{
			var cacheKey1 = new CacheKey(this.GetType(),
				new RockOptions(OptimizationSetting.Release, CodeFileOptions.Create, SerializationOptions.Supported));
			var cacheKey2 = new CacheKey(this.GetType(),
				new RockOptions(OptimizationSetting.Release, CodeFileOptions.None, SerializationOptions.Supported));

			Assert.AreNotEqual(cacheKey1, cacheKey2);
		}

		[Test]
		public void CompareHashCodesWhenNotEqualViaDifferentTypes()
		{
			var cacheKey1 = new CacheKey(this.GetType(),
				new RockOptions(OptimizationSetting.Release, CodeFileOptions.Create, SerializationOptions.Supported));
			var cacheKey2 = new CacheKey(typeof(Guid),
				new RockOptions(OptimizationSetting.Release, CodeFileOptions.Create, SerializationOptions.Supported));

			Assert.AreNotEqual(cacheKey1, cacheKey2);
		}
	}
}
