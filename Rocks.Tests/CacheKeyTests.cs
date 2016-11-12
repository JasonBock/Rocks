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

			Assert.That(cacheKey2, Is.EqualTo(cacheKey1));
		}

		[Test]
		public void CompareHashCodesWhenNotEqualViaDifferentOptions()
		{
			var cacheKey1 = new CacheKey(this.GetType(),
				new RockOptions(OptimizationSetting.Release, CodeFileOptions.Create, SerializationOptions.Supported));
			var cacheKey2 = new CacheKey(this.GetType(),
				new RockOptions(OptimizationSetting.Release, CodeFileOptions.None, SerializationOptions.Supported));

			Assert.That(cacheKey2, Is.Not.EqualTo(cacheKey1));
		}

		[Test]
		public void CompareHashCodesWhenNotEqualViaDifferentTypes()
		{
			var cacheKey1 = new CacheKey(this.GetType(),
				new RockOptions(OptimizationSetting.Release, CodeFileOptions.Create, SerializationOptions.Supported));
			var cacheKey2 = new CacheKey(typeof(Guid),
				new RockOptions(OptimizationSetting.Release, CodeFileOptions.Create, SerializationOptions.Supported));

			Assert.That(cacheKey2, Is.Not.EqualTo(cacheKey1));
		}
	}
}
