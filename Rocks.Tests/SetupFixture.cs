using NUnit.Framework;
using System.IO;

namespace Rocks.Tests
{
	[SetUpFixture]
	public sealed class SetupFixture
	{
		[OneTimeSetUp]
		public static void SetUp() => SetupFixture.CleanUpRockFiles();

		[OneTimeTearDown]
		public static void TearDown() => SetupFixture.CleanUpRockFiles();

		private static void CleanUpRockFiles()
		{
			// TODO: need to change this to include subdirectories.
			foreach (var rockFile in Directory.GetFiles(Directory.GetCurrentDirectory(), "Rock*.cs"))
			{
				File.Delete(rockFile);
			}
		}
	}
}
