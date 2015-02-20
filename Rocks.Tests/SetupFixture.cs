using NUnit.Framework;
using System.IO;

namespace Rocks.Tests
{
	[SetUpFixture]
	public sealed class SetupFixture
	{
		[SetUp]
		public void SetUp()
		{
			SetupFixture.CleanUpRockFiles();
		}

		[TearDown]
		public void TearDown()
		{
			SetupFixture.CleanUpRockFiles();
		}

		private static void CleanUpRockFiles()
		{
			foreach (var rockFile in Directory.GetFiles(Directory.GetCurrentDirectory(), "Rock*.cs"))
			{
				File.Delete(rockFile);
			}
		}
	}
}
