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
			Assert.That(options.Optimization, Is.EqualTo(OptimizationSetting.Release), nameof(options.Optimization));
			Assert.That(options.CodeFile, Is.EqualTo(CodeFileOptions.None), nameof(options.CodeFile));
			Assert.That(options.Serialization, Is.EqualTo(SerializationOptions.NotSupported), nameof(options.Serialization));
			Assert.That(options.Caching, Is.EqualTo(CachingOptions.UseCache), nameof(options.Caching));
			Assert.That(options.AllowWarnings, Is.EqualTo(AllowWarnings.No), nameof(options.AllowWarnings));
			Assert.That(options.GetHashCode(), Is.EqualTo(4), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithOptimizationLevelDebug()
		{
			var options = new RockOptions(level: OptimizationSetting.Debug);
			Assert.That(options.Optimization, Is.EqualTo(OptimizationSetting.Debug), nameof(options.Optimization));
			Assert.That(options.CodeFile, Is.EqualTo(CodeFileOptions.None), nameof(options.CodeFile));
			Assert.That(options.Serialization, Is.EqualTo(SerializationOptions.NotSupported), nameof(options.Serialization));
			Assert.That(options.Caching, Is.EqualTo(CachingOptions.UseCache), nameof(options.Caching));
			Assert.That(options.AllowWarnings, Is.EqualTo(AllowWarnings.No), nameof(options.AllowWarnings));
			Assert.That(options.GetHashCode(), Is.EqualTo(0), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithCodeFileOptionsCreate()
		{
			var options = new RockOptions(codeFile: CodeFileOptions.Create);
			Assert.That(options.Optimization, Is.EqualTo(OptimizationSetting.Release), nameof(options.Optimization));
			Assert.That(options.CodeFile, Is.EqualTo(CodeFileOptions.Create), nameof(options.CodeFile));
			Assert.That(options.Serialization, Is.EqualTo(SerializationOptions.NotSupported), nameof(options.Serialization));
			Assert.That(options.Caching, Is.EqualTo(CachingOptions.UseCache), nameof(options.Caching));
			Assert.That(options.AllowWarnings, Is.EqualTo(AllowWarnings.No), nameof(options.AllowWarnings));
			Assert.That(options.GetHashCode(), Is.EqualTo(5), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithSerializationOptionsSupported()
		{
			var options = new RockOptions(serialization: SerializationOptions.Supported);
			Assert.That(options.Optimization, Is.EqualTo(OptimizationSetting.Release), nameof(options.Optimization));
			Assert.That(options.CodeFile, Is.EqualTo(CodeFileOptions.None), nameof(options.CodeFile));
			Assert.That(options.Serialization, Is.EqualTo(SerializationOptions.Supported), nameof(options.Serialization));
			Assert.That(options.Caching, Is.EqualTo(CachingOptions.UseCache), nameof(options.Caching));
			Assert.That(options.AllowWarnings, Is.EqualTo(AllowWarnings.No), nameof(options.AllowWarnings));
			Assert.That(options.GetHashCode(), Is.EqualTo(6), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithCachingOptionsGenerateNewVersion()
		{
			var options = new RockOptions(caching: CachingOptions.GenerateNewVersion);
			Assert.That(options.Optimization, Is.EqualTo(OptimizationSetting.Release), nameof(options.Optimization));
			Assert.That(options.CodeFile, Is.EqualTo(CodeFileOptions.None), nameof(options.CodeFile));
			Assert.That(options.Serialization, Is.EqualTo(SerializationOptions.NotSupported), nameof(options.Serialization));
			Assert.That(options.Caching, Is.EqualTo(CachingOptions.GenerateNewVersion), nameof(options.Caching));
			Assert.That(options.AllowWarnings, Is.EqualTo(AllowWarnings.No), nameof(options.AllowWarnings));
			Assert.That(options.GetHashCode(), Is.EqualTo(12), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithAllowWarningsYes()
		{
			var options = new RockOptions(allowWarnings: AllowWarnings.Yes);
			Assert.That(options.Optimization, Is.EqualTo(OptimizationSetting.Release), nameof(options.Optimization));
			Assert.That(options.CodeFile, Is.EqualTo(CodeFileOptions.None), nameof(options.CodeFile));
			Assert.That(options.Serialization, Is.EqualTo(SerializationOptions.NotSupported), nameof(options.Serialization));
			Assert.That(options.Caching, Is.EqualTo(CachingOptions.UseCache), nameof(options.Caching));
			Assert.That(options.AllowWarnings, Is.EqualTo(AllowWarnings.Yes), nameof(options.AllowWarnings));
			Assert.That(options.GetHashCode(), Is.EqualTo(20), nameof(options.GetHashCode));
		}
	}
}
