using Microsoft.Build.Framework;
using NUnit.Framework;
using Rocks.RockAssemblyTestContainer;

namespace Rocks.Task.Tests
{
	[TestFixture]
	public sealed class RocksTaskTests
	{
		[Test]
		public void Create()
		{
			var engineMock = Rock.Create<IBuildEngine>();
			engineMock.Handle(_ => _.LogMessageEvent(Arg.IsAny<BuildMessageEventArgs>()), 2);

			var task = new RocksTask { AssemblyLocation = typeof(Class1).Assembly.Location };
			task.BuildEngine = engineMock.Make();
			task.Execute();

			Assert.IsNotNull(task.Result);

			engineMock.Verify();
		}
	}
}
