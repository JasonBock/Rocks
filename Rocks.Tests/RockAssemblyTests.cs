using NUnit.Framework;
using Rocks.RockAssemblyTestContainer;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using static Rocks.Extensions.IMockExtensions;

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

		[Test]
		public void GenerateRock()
		{
			var rock = typeof(Rock).GetMethod(nameof(Rock.Create), Type.EmptyTypes).MakeGenericMethod(
				new[] { this.assembly.GetType("Rocks.RockAssemblyTestContainer.RockClass1") }).Invoke(null, null);
			Assert.IsTrue(typeof(AssemblyRock<>).IsAssignableFrom(rock.GetType().GetGenericTypeDefinition()));
		}

		[Test]
		public void GenerateMockWithHandlers()
		{
			var rock = Rock.Create<Class1>();
			rock.HandleAction(_ => _.Method1());

			var handlers = rock.GetType().GetMethod("CreateReadOnlyHandlerDictionary", BindingFlags.Instance | BindingFlags.NonPublic)
				.Invoke(rock, null) as ReadOnlyDictionary<string, ReadOnlyCollection<HandlerInformation>>;
         var mock = Activator.CreateInstance(this.assembly.GetType("Rocks.RockAssemblyTestContainer.RockClass1"), handlers) as Class1;

			mock.Method1();

			Assert.AreEqual(0, (mock as IMock).GetVerificationFailures().Count);
		}
	}
}
