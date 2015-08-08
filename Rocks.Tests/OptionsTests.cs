using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class OptionsTests
	{
		[Test]
		public void Create()
		{
			var options = new Options();
			Assert.AreEqual(OptimizationSetting.Release, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.UseCache, options.Caching, nameof(options.Caching));
			Assert.AreEqual(4, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithCodeFile()
		{
			var options = new Options(CodeFileOptions.Create);
			Assert.AreEqual(OptimizationSetting.Release, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.Create, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.UseCache, options.Caching, nameof(options.Caching));
			Assert.AreEqual(5, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithCodeFileAndSerialization()
		{
			var options = new Options(CodeFileOptions.Create, SerializationOptions.Supported);
			Assert.AreEqual(OptimizationSetting.Release, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.Create, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.Supported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.UseCache, options.Caching, nameof(options.Caching));
			Assert.AreEqual(7, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithCodeFileAndCaching()
		{
			var options = new Options(CodeFileOptions.Create, CachingOptions.GenerateNewVersion);
			Assert.AreEqual(OptimizationSetting.Release, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.Create, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.GenerateNewVersion, options.Caching, nameof(options.Caching));
			Assert.AreEqual(13, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithCodeFileAndSerializationAndCaching()
		{
			var options = new Options(CodeFileOptions.Create, SerializationOptions.Supported, CachingOptions.GenerateNewVersion);
			Assert.AreEqual(OptimizationSetting.Release, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.Create, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.Supported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.GenerateNewVersion, options.Caching, nameof(options.Caching));
			Assert.AreEqual(15, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithLevel()
		{
			var options = new Options(OptimizationSetting.Debug);
			Assert.AreEqual(OptimizationSetting.Debug, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.UseCache, options.Caching, nameof(options.Caching));
			Assert.AreEqual(0, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithLevelAndCaching()
		{
			var options = new Options(OptimizationSetting.Debug, CachingOptions.GenerateNewVersion);
			Assert.AreEqual(OptimizationSetting.Debug, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.GenerateNewVersion, options.Caching, nameof(options.Caching));
			Assert.AreEqual(8, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithLevelAndCodeFile()
		{
			var options = new Options(OptimizationSetting.Debug, CodeFileOptions.Create);
			Assert.AreEqual(OptimizationSetting.Debug, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.Create, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.UseCache, options.Caching, nameof(options.Caching));
			Assert.AreEqual(1, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithLevelAndCodeFileAndCaching()
		{
			var options = new Options(OptimizationSetting.Debug, CodeFileOptions.Create, CachingOptions.GenerateNewVersion);
			Assert.AreEqual(OptimizationSetting.Debug, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.Create, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.GenerateNewVersion, options.Caching, nameof(options.Caching));
			Assert.AreEqual(9, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithLevelAndSerialization()
		{
			var options = new Options(OptimizationSetting.Debug, SerializationOptions.Supported);
			Assert.AreEqual(OptimizationSetting.Debug, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.Supported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.UseCache, options.Caching, nameof(options.Caching));
			Assert.AreEqual(2, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithLevelAndSerializationAndCaching()
		{
			var options = new Options(OptimizationSetting.Debug, SerializationOptions.Supported, CachingOptions.GenerateNewVersion);
			Assert.AreEqual(OptimizationSetting.Debug, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.Supported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.GenerateNewVersion, options.Caching, nameof(options.Caching));
			Assert.AreEqual(10, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithLevelAndCodeFileAndSerialization()
		{
			var options = new Options(OptimizationSetting.Debug, CodeFileOptions.Create, SerializationOptions.Supported);
			Assert.AreEqual(OptimizationSetting.Debug, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.Create, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.Supported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.UseCache, options.Caching, nameof(options.Caching));
			Assert.AreEqual(3, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithLevelAndCodeFileAndSerializationAndCaching()
		{
			var options = new Options(OptimizationSetting.Debug, CodeFileOptions.Create, SerializationOptions.Supported, CachingOptions.GenerateNewVersion);
			Assert.AreEqual(OptimizationSetting.Debug, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.Create, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.Supported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.GenerateNewVersion, options.Caching, nameof(options.Caching));
			Assert.AreEqual(11, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithSerialization()
		{
			var options = new Options(SerializationOptions.Supported);
			Assert.AreEqual(OptimizationSetting.Release, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.Supported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.UseCache, options.Caching, nameof(options.Caching));
			Assert.AreEqual(6, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithSerializationAndCaching()
		{
			var options = new Options(SerializationOptions.Supported, CachingOptions.GenerateNewVersion);
			Assert.AreEqual(OptimizationSetting.Release, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.Supported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.GenerateNewVersion, options.Caching, nameof(options.Caching));
			Assert.AreEqual(14, options.GetHashCode(), nameof(options.GetHashCode));
		}

		[Test]
		public void CreateWithCaching()
		{
			var options = new Options(CachingOptions.GenerateNewVersion);
			Assert.AreEqual(OptimizationSetting.Release, options.Optimization, nameof(options.Optimization));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
			Assert.AreEqual(CachingOptions.GenerateNewVersion, options.Caching, nameof(options.Caching));
			Assert.AreEqual(12, options.GetHashCode(), nameof(options.GetHashCode));
		}
	}
}
