using Microsoft.Build.Framework;
using NUnit.Framework;
using Rocks.Options;
using Rocks.RockTaskTestContainer;

namespace Rocks.Tests
{
	public static class RocksTaskTests
	{
		[Test]
		public static void Create()
		{
			var engineMock = Rock.Create<IBuildEngine>(
				new RockOptions(codeFileDirectory: TestContext.CurrentContext.TestDirectory));
			engineMock.Handle(_ => _.LogMessageEvent(Arg.IsAny<BuildMessageEventArgs>()), 2);

			var task = new RocksTask
			{
				AssemblyLocation = typeof(TaskTarget).Assembly.Location,
				CodeFileDirectory = TestContext.CurrentContext.TestDirectory
			};
			task.BuildEngine = engineMock.Make();
			task.Execute();

			Assert.That(task.Result, Is.Not.Null);

			engineMock.Verify();
		}
	}
}