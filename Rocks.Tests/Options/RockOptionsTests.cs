using NUnit.Framework;
using Rocks.Options;
using System.IO;

namespace Rocks.Tests.Options
{
	public static class OptionsTests
	{
		[Test]
		public static void CreateWithDefaults() =>
			OptionsTests.AssertOptions(new RockOptions(), 
				OptimizationSetting.Release, CodeFileOptions.None,
				SerializationOptions.NotSupported,
				CachingOptions.UseCache, AllowWarnings.No,
				Directory.GetCurrentDirectory());

		[Test]
		public static void CreateWithCodeFileDirectory() =>
			OptionsTests.AssertOptions(new RockOptions(codeFileDirectory: "directory"),
				OptimizationSetting.Release, CodeFileOptions.None,
				SerializationOptions.NotSupported,
				CachingOptions.UseCache, AllowWarnings.No,
				"directory");

		[Test]
		public static void CreateWithOptimizationLevelDebug() =>
			OptionsTests.AssertOptions(new RockOptions(level: OptimizationSetting.Debug), 
				OptimizationSetting.Debug, CodeFileOptions.None,
				SerializationOptions.NotSupported,
				CachingOptions.UseCache, AllowWarnings.No,
				Directory.GetCurrentDirectory());

		[Test]
		public static void CreateWithCodeFileOptionsCreate() =>
			OptionsTests.AssertOptions(new RockOptions(codeFile: CodeFileOptions.Create),
				OptimizationSetting.Release, CodeFileOptions.Create,
				SerializationOptions.NotSupported,
				CachingOptions.UseCache, AllowWarnings.No,
				Directory.GetCurrentDirectory());

		[Test]
		public static void CreateWithSerializationOptionsSupported() =>
			OptionsTests.AssertOptions(new RockOptions(serialization: SerializationOptions.Supported),
				OptimizationSetting.Release, CodeFileOptions.None,
				SerializationOptions.Supported,
				CachingOptions.UseCache, AllowWarnings.No,
				Directory.GetCurrentDirectory());

		[Test]
		public static void CreateWithCachingOptionsGenerateNewVersion() =>
			OptionsTests.AssertOptions(new RockOptions(caching: CachingOptions.GenerateNewVersion),
				OptimizationSetting.Release, CodeFileOptions.None,
				SerializationOptions.NotSupported,
				CachingOptions.GenerateNewVersion, AllowWarnings.No,
				Directory.GetCurrentDirectory());

		[Test]
		public static void CreateWithAllowWarningsYes() =>
			OptionsTests.AssertOptions(new RockOptions(allowWarnings: AllowWarnings.Yes),
				OptimizationSetting.Release, CodeFileOptions.None,
				SerializationOptions.NotSupported,
				CachingOptions.UseCache, AllowWarnings.Yes,
				Directory.GetCurrentDirectory());

		private static void AssertOptions(RockOptions options,
			OptimizationSetting level, CodeFileOptions codeFile, SerializationOptions serialization,
			CachingOptions caching, AllowWarnings allowWarnings, string codeFileDirectory)
		{
			Assert.That(options.Optimization, Is.EqualTo(level), nameof(options.Optimization));
			Assert.That(options.CodeFile, Is.EqualTo(codeFile), nameof(options.CodeFile));
			Assert.That(options.Serialization, Is.EqualTo(serialization), nameof(options.Serialization));
			Assert.That(options.Caching, Is.EqualTo(caching), nameof(options.Caching));
			Assert.That(options.AllowWarnings, Is.EqualTo(allowWarnings), nameof(options.AllowWarnings));
			Assert.That(options.CodeFileDirectory, Is.EqualTo(codeFileDirectory), nameof(options.CodeFileDirectory));
		}
	}
}
