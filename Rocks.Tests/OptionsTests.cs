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
			Assert.AreEqual(OptimizationLevel.Release, options.Level, nameof(options.Level));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
		}

		[Test]
		public void CreateWithCodeFile()
		{
			var options = new Options(CodeFileOptions.Create);
			Assert.AreEqual(OptimizationLevel.Release, options.Level, nameof(options.Level));
			Assert.AreEqual(CodeFileOptions.Create, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
		}

		[Test]
		public void CreateWithCodeFileAndSerialization()
		{
			var options = new Options(CodeFileOptions.Create, SerializationOptions.Supported);
			Assert.AreEqual(OptimizationLevel.Release, options.Level, nameof(options.Level));
			Assert.AreEqual(CodeFileOptions.Create, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.Supported, options.Serialization, nameof(options.Serialization));
		}

		[Test]
		public void CreateWithLevel()
		{
			var options = new Options(OptimizationLevel.Debug);
			Assert.AreEqual(OptimizationLevel.Debug, options.Level, nameof(options.Level));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
		}

		[Test]
		public void CreateWithLevelAndCodeFile()
		{
			var options = new Options(OptimizationLevel.Debug, CodeFileOptions.Create);
			Assert.AreEqual(OptimizationLevel.Debug, options.Level, nameof(options.Level));
			Assert.AreEqual(CodeFileOptions.Create, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.NotSupported, options.Serialization, nameof(options.Serialization));
		}

		[Test]
		public void CreateWithLevelAndSerialization()
		{
			var options = new Options(OptimizationLevel.Debug, SerializationOptions.Supported);
			Assert.AreEqual(OptimizationLevel.Debug, options.Level, nameof(options.Level));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.Supported, options.Serialization, nameof(options.Serialization));
		}

		[Test]
		public void CreateWithLevelAndCodeFileAndSerialization()
		{
			var options = new Options(OptimizationLevel.Debug, CodeFileOptions.Create, SerializationOptions.Supported);
			Assert.AreEqual(OptimizationLevel.Debug, options.Level, nameof(options.Level));
			Assert.AreEqual(CodeFileOptions.Create, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.Supported, options.Serialization, nameof(options.Serialization));
		}

		[Test]
		public void CreateWithSerialization()
		{
			var options = new Options(SerializationOptions.Supported);
			Assert.AreEqual(OptimizationLevel.Release, options.Level, nameof(options.Level));
			Assert.AreEqual(CodeFileOptions.None, options.CodeFile, nameof(options.CodeFile));
			Assert.AreEqual(SerializationOptions.Supported, options.Serialization, nameof(options.Serialization));
		}
	}
}
