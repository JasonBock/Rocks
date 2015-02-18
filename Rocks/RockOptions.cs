using Microsoft.CodeAnalysis;

namespace Rocks
{
	public sealed class RockOptions
	{
		public RockOptions()
			: this(OptimizationLevel.Release, false)
		{ }

		public RockOptions(OptimizationLevel level, bool shouldCreateCodeFile)
		{
			this.Level = level;
			this.ShouldCreateCodeFile = shouldCreateCodeFile;
		}

		public bool ShouldCreateCodeFile { get; private set; }
		public OptimizationLevel Level { get; private set; }
	}
}
