using NUnit.Framework;
using Rocks.Task.TestContainer.Contracts;
using System;

namespace Rocks.Task.TestContainer.Tests
{
	[TestFixture]
	public sealed class RockAssemblyGenerationTests
	{
		[Test]
		public void GenerateMockFromClass()
		{
			var rock = Rock.Create<RockClass1>();
			rock.HandleAction(_ => _.Method1());

			var chunk = rock.Make();
			chunk.Method1();

			rock.Verify();
		}

		[Test]
		public void GenerateMockFromInterface()
		{
			var rock = Rock.Create<RockInterface1<Guid>>();
			rock.HandleAction(_ => _.Method1());

			var chunk = rock.Make();
			chunk.Method1();

			rock.Verify();
		}
	}
}
