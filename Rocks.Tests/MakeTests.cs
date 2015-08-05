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
			// TODO need to support all method and property scenarios.
			var chunk = Rock.Make<IAmForMaking>(new Options(OptimizationLevel.Debug, CodeFileOptions.Create));
			chunk.TargetMethod();
			chunk.TargetProperty = 44;
			var x = chunk.TargetProperty;
		}
	}

	public interface IAmForMaking
	{
		void TargetMethod();
		int TargetProperty { get; set; }
	}
}
