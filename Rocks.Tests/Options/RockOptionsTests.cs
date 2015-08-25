using NUnit.Framework;
using Rocks.Options;

namespace Rocks.Tests.Options
{
	[TestFixture]
	public sealed class OptionsTests
	{
		[Test]
		public void CreateWithDefaults()
		{
			var options = new RockOptions();
			Assert.AreEqual(OptimizationSetting.Release, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.UseCache, options.Caching, nameof(options.Caching));
			Assert.AreEqual(AllowWarnings.No, options.AllowWarnings, nameof(options.AllowWarnings));
			Assert.AreEqual(4, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithOptimizationLevelDebug()
		{
			var options = new RockOptions(level: OptimizationSetting.Debug);
			Assert.AreEqual(OptimizationSetting.Debug, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.UseCache, options.Caching, nameof(options.Caching));
			Assert.AreEqual(AllowWarnings.No, options.AllowWarnings, nameof(options.AllowWarnings));
			Assert.AreEqual(0, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithCodeFileOptionsCreate()
		{
			var options = new RockOptions(codeFile: CodeFileOptions.Create);
			Assert.AreEqual(OptimizationSetting.Release, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.Create, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.UseCache, options.Caching, nameof(options.Caching));
			Assert.AreEqual(AllowWarnings.No, options.AllowWarnings, nameof(options.AllowWarnings));
			Assert.AreEqual(5, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithSerializationOptionsSupported()
		{
			var options = new RockOptions(serialization: SerializationOptions.Supported);
			Assert.AreEqual(OptimizationSetting.Release, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.Supported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.UseCache, options.Caching, nameof(options.Caching));
			Assert.AreEqual(AllowWarnings.No, options.AllowWarnings, nameof(options.AllowWarnings));
			Assert.AreEqual(6, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithCachingOptionsGenerateNewVersion()
		{
			var options = new RockOptions(caching: CachingOptions.GenerateNewVersion);
			Assert.AreEqual(OptimizationSetting.Release, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.GenerateNewVersion, options.Caching, nameof(options.Caching));
			Assert.AreEqual(AllowWarnings.No, options.AllowWarnings, nameof(options.AllowWarnings));
			Assert.AreEqual(12, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithAllowWarningsYes()
		{
			var options = new RockOptions(allowWarnings: AllowWarnings.Yes);
			Assert.AreEqual(OptimizationSetting.Release, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.UseCache, options.Caching, nameof(options.Caching));
			Assert.AreEqual(AllowWarnings.Yes, options.AllowWarnings, nameof(options.AllowWarnings));
			Assert.AreEqual(20, options.GetHashCode(), nameof(options.GetHashCode));
		}
	}
}
