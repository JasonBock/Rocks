using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class MultipleExpectationsTests
	{
		[Test, Ignore("Need to fix casting")]
		public void HandleMultiple()
		{
			var rock = Rock.Create<IMultiple>(new Options(Microsoft.CodeAnalysis.OptimizationLevel.Debug, true));
			rock.HandleAction(_ => _.Target("a", 44));
			rock.HandleAction(_ => _.Target("b", "44"));
			rock.HandleAction(_ => _.Target("a", "44"));

			var chunk = rock.Make();
			chunk.Target("a", "44");
			chunk.Target("b", "44");
			chunk.Target("a", 44);

			rock.Verify();
		}
	}

	public interface IMultiple
	{
		void Target(string a, object b);
	}
}
