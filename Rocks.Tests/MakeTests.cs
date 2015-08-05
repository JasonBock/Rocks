using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class MakeTests
	{
		[Test]
		public void Make()
		{
			var chunk = Rock.Make<IAmForMaking>(new Options(OptimizationLevel.Debug, CodeFileOptions.Create));
			chunk.TargetMethod();

			// TODO need to support all method and property scenarios.
		}
	}

	public interface IAmForMaking
	{
		void TargetMethod();
		int TargetProperty { get; set; }
	}
}
