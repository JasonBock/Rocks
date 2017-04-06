using NUnit.Framework;
using System.IO;

namespace Rocks.Tests
{
	[SetUpFixture]
	public sealed class SetupFixture
	{
		[OneTimeSetUp]
		public static void SetUp() => SetupFixture.CleanUpCodeFilesAndFolders();

		[OneTimeTearDown]
		public static void TearDown() => SetupFixture.CleanUpCodeFilesAndFolders();

		private static void CleanUpCodeFilesAndFolders()
		{
			var testDirectory = TestContext.CurrentContext.TestDirectory;

			SetupFixture.DeleteFiles(testDirectory);

			foreach(var rockDirectory in Directory.GetDirectories(testDirectory))
			{
				Directory.Delete(rockDirectory, true);
			}
		}

		private static void DeleteFiles(string directory)
		{
			foreach (var rockFile in Directory.GetFiles(directory, "Rock*.cs"))
			{
				File.Delete(rockFile);
			}

			foreach (var rockDirectory in Directory.GetDirectories(directory))
			{
				SetupFixture.DeleteFiles(rockDirectory);
			}
		}
	}
}
