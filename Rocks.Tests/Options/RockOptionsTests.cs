using NUnit.Framework;
using Rocks.Options;
using System.IO;

namespace Rocks.Tests.Options
{
	[TestFixture]
	public sealed class OptionsTests
	{
		[Test]
		public void CreateWithDefaults() =>
			OptionsTests.AssertOptions(new RockOptions(), 
				OptimizationSetting.Release, CodeFileOptions.None,
#if !NETCOREAPP1_1
				SerializationOptions.NotSupported,
#endif
				CachingOptions.UseCache, AllowWarnings.No,
				Directory.GetCurrentDirectory(), 4);

		[Test]
		public void CreateWithCodeFileDirectory() =>
			OptionsTests.AssertOptions(new RockOptions(codeFileDirectory: "directory"),
				OptimizationSetting.Release, CodeFileOptions.None,
#if !NETCOREAPP1_1
				SerializationOptions.NotSupported,
#endif
				CachingOptions.UseCache, AllowWarnings.No,
				"directory", 4);

		[Test]
		public void CreateWithOptimizationLevelDebug() =>
			OptionsTests.AssertOptions(new RockOptions(level: OptimizationSetting.Debug), 
				OptimizationSetting.Debug, CodeFileOptions.None,
#if !NETCOREAPP1_1
				SerializationOptions.NotSupported,
#endif
				CachingOptions.UseCache, AllowWarnings.No,
				Directory.GetCurrentDirectory(), 0);

		[Test]
		public void CreateWithCodeFileOptionsCreate() =>
			OptionsTests.AssertOptions(new RockOptions(codeFile: CodeFileOptions.Create),
				OptimizationSetting.Release, CodeFileOptions.Create,
#if !NETCOREAPP1_1
				SerializationOptions.NotSupported,
#endif
				CachingOptions.UseCache, AllowWarnings.No,
				Directory.GetCurrentDirectory(), 5);

#if !NETCOREAPP1_1
		[Test]
		public void CreateWithSerializationOptionsSupported() =>
			OptionsTests.AssertOptions(new RockOptions(serialization: SerializationOptions.Supported),
				OptimizationSetting.Release, CodeFileOptions.None,
				SerializationOptions.Supported,
				CachingOptions.UseCache, AllowWarnings.No,
				Directory.GetCurrentDirectory(), 6);
#endif

		[Test]
		public void CreateWithCachingOptionsGenerateNewVersion() =>
			OptionsTests.AssertOptions(new RockOptions(caching: CachingOptions.GenerateNewVersion),
				OptimizationSetting.Release, CodeFileOptions.None,
#if !NETCOREAPP1_1
				SerializationOptions.NotSupported,
#endif
				CachingOptions.GenerateNewVersion, AllowWarnings.No,
				Directory.GetCurrentDirectory(), 12);

		[Test]
		public void CreateWithAllowWarningsYes() =>
			OptionsTests.AssertOptions(new RockOptions(allowWarnings: AllowWarnings.Yes),
				OptimizationSetting.Release, CodeFileOptions.None,
#if !NETCOREAPP1_1
				SerializationOptions.NotSupported,
#endif
				CachingOptions.UseCache, AllowWarnings.Yes,
				Directory.GetCurrentDirectory(), 20);

#if !NETCOREAPP1_1
		private static void AssertOptions(RockOptions options,
			OptimizationSetting level, CodeFileOptions codeFile, SerializationOptions serialization,
			CachingOptions caching, AllowWarnings allowWarnings, string codeFileDirectory, int hashCode)
#else
		private static void AssertOptions(RockOptions options,
			OptimizationSetting level, CodeFileOptions codeFile,
			CachingOptions caching, AllowWarnings allowWarnings, string codeFileDirectory, int hashCode)
#endif
		{
			Assert.That(options.Optimization, Is.EqualTo(level), nameof(options.Optimization));
			Assert.That(options.CodeFile, Is.EqualTo(codeFile), nameof(options.CodeFile));
#if !NETCOREAPP1_1
			Assert.That(options.Serialization, Is.EqualTo(serialization), nameof(options.Serialization));
#endif
			Assert.That(options.Caching, Is.EqualTo(caching), nameof(options.Caching));
			Assert.That(options.AllowWarnings, Is.EqualTo(allowWarnings), nameof(options.AllowWarnings));
			Assert.That(options.CodeFileDirectory, Is.EqualTo(codeFileDirectory), nameof(options.CodeFileDirectory));
			Assert.That(options.GetHashCode(), Is.EqualTo(hashCode ^ options.CodeFileDirectory.GetHashCode()), nameof(options.GetHashCode));
		}
	}
}
