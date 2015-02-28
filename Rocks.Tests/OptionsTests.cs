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
			Assert.IsFalse(options.ShouldCreateCodeFile, nameof(options.ShouldCreateCodeFile));
		}

		[Test]
		public void CreateWithShouldCreateCodeFile()
		{
			var options = new Options(true);
			Assert.AreEqual(OptimizationLevel.Release, options.Level, nameof(options.Level));
			Assert.IsTrue(options.ShouldCreateCodeFile, nameof(options.ShouldCreateCodeFile));
		}

		[Test]
		public void CreateWithLevel()
		{
			var options = new Options(OptimizationLevel.Debug);
			Assert.AreEqual(OptimizationLevel.Debug, options.Level, nameof(options.Level));
			Assert.IsFalse(options.ShouldCreateCodeFile, nameof(options.ShouldCreateCodeFile));
		}

		[Test]
		public void CreateWithShouldCreateCodeFileAndLevel()
		{
			var options = new Options(OptimizationLevel.Debug, true);
			Assert.AreEqual(OptimizationLevel.Debug, options.Level, nameof(options.Level));
			Assert.IsTrue(options.ShouldCreateCodeFile, nameof(options.ShouldCreateCodeFile));
		}
	}
}
