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
				OptimizationSetting.Release, CodeFileOption.None,
				SerializationOption.NotSupported,
				CachingOption.UseCache, AllowWarning.No,
				Directory.GetCurrentDirectory());

		[Test]
		public static void CreateWithCodeFileDirectory() =>
			OptionsTests.AssertOptions(new RockOptions(codeFileDirectory: "directory"),
				OptimizationSetting.Release, CodeFileOption.None,
				SerializationOption.NotSupported,
				CachingOption.UseCache, AllowWarning.No,
				"directory");

		[Test]
		public static void CreateWithOptimizationLevelDebug() =>
			OptionsTests.AssertOptions(new RockOptions(level: OptimizationSetting.Debug), 
				OptimizationSetting.Debug, CodeFileOption.None,
				SerializationOption.NotSupported,
				CachingOption.UseCache, AllowWarning.No,
				Directory.GetCurrentDirectory());

		[Test]
		public static void CreateWithCodeFileOptionsCreate() =>
			OptionsTests.AssertOptions(new RockOptions(codeFile: CodeFileOption.Create),
				OptimizationSetting.Release, CodeFileOption.Create,
				SerializationOption.NotSupported,
				CachingOption.UseCache, AllowWarning.No,
				Directory.GetCurrentDirectory());

		[Test]
		public static void CreateWithSerializationOptionsSupported() =>
			OptionsTests.AssertOptions(new RockOptions(serialization: SerializationOption.Supported),
				OptimizationSetting.Release, CodeFileOption.None,
				SerializationOption.Supported,
				CachingOption.UseCache, AllowWarning.No,
				Directory.GetCurrentDirectory());

		[Test]
		public static void CreateWithCachingOptionsGenerateNewVersion() =>
			OptionsTests.AssertOptions(new RockOptions(caching: CachingOption.GenerateNewVersion),
				OptimizationSetting.Release, CodeFileOption.None,
				SerializationOption.NotSupported,
				CachingOption.GenerateNewVersion, AllowWarning.No,
				Directory.GetCurrentDirectory());

		[Test]
		public static void CreateWithAllowWarningsYes() =>
			OptionsTests.AssertOptions(new RockOptions(allowWarning: AllowWarning.Yes),
				OptimizationSetting.Release, CodeFileOption.None,
				SerializationOption.NotSupported,
				CachingOption.UseCache, AllowWarning.Yes,
				Directory.GetCurrentDirectory());

		private static void AssertOptions(RockOptions options,
			OptimizationSetting level, CodeFileOption codeFile, SerializationOption serialization,
			CachingOption caching, AllowWarning allowWarnings, string codeFileDirectory)
		{
			Assert.That(options.Optimization, Is.EqualTo(level), nameof(options.Optimization));
			Assert.That(options.CodeFile, Is.EqualTo(codeFile), nameof(options.CodeFile));
			Assert.That(options.Serialization, Is.EqualTo(serialization), nameof(options.Serialization));
			Assert.That(options.Caching, Is.EqualTo(caching), nameof(options.Caching));
			Assert.That(options.AllowWarning, Is.EqualTo(allowWarnings), nameof(options.AllowWarning));
			Assert.That(options.CodeFileDirectory, Is.EqualTo(codeFileDirectory), nameof(options.CodeFileDirectory));
		}
	}
}
