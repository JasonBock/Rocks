using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace Rocks.Tests
{
	[TestFixture]
	public sealed class MakeTests
	{
		[Test, Ignore("Still working on feature...")]
		public void Make()
		{
			var chunk = Rock.Make<IAmForMaking>(new Options(OptimizationLevel.Debug, CodeFileOptions.Create));
			chunk.TargetMethod();
			chunk.TargetProperty = 42;
			var x = chunk.TargetProperty;
		}
	}

	public interface IAmForMaking
	{
		void TargetMethod();
		int TargetProperty { get; set; }
	}
}
