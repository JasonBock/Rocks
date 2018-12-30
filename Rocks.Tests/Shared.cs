using NUnit.Framework;
using Rocks;
using Rocks.Options;
using Rocks.Tests;

[assembly: Parallelizable(ParallelScope.Fixtures)]

[SetUpFixture]
public class Shared
{
	[OneTimeSetUp]
	public void RunBeforeAnyTests()
	{
		var rock = Rock.Create<IWarmup>(new RockOptions(codeFile: CodeFileOptions.Create));
		rock.Handle(_ => _.Warmup());

		var chunk = rock.Make();
		chunk.Warmup();

		rock.Verify();
	}
}

namespace Rocks.Tests
{
	public interface IWarmup
	{
		void Warmup();
	}
}