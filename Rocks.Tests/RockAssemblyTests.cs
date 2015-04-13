using NUnit.Framework;
using Rocks.RockAssemblyTestContainer;
using System.Reflection;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class RockAssemblyTests
	{
		private Assembly assembly;

		public RockAssemblyTests()
		{
			this.assembly = new RockAssembly(typeof(Class1).Assembly,
				new Options(CodeFileOptions.Create)).Result;
		}

		[Test]
		public void Create()
		{
			Assert.IsNotNull(this.assembly.GetType("Rocks.RockAssemblyTestContainer.RockClass1"));
			Assert.IsNotNull(this.assembly.GetType("Rocks.RockAssemblyTestContainer.Contracts.RockInterface1`1"));
			Assert.IsNull(this.assembly.GetType("Rocks.RockAssemblyTestContainer.Extensions.RockClass3"));
		}
   }
}
