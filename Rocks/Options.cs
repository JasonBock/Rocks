using Microsoft.CodeAnalysis;

namespace Rocks
{
	public sealed class Options
	{
		public Options()
			: this(OptimizationLevel.Release, false)
		{ }

		public Options(OptimizationLevel level)
			: this(level, false)
		{ }

		public Options(bool shouldCreateCodeFile)
			: this(OptimizationLevel.Release, shouldCreateCodeFile)
		{ }

		public Options(OptimizationLevel level, bool shouldCreateCodeFile)
		{
			this.Level = level;
			this.ShouldCreateCodeFile = shouldCreateCodeFile;
		}

		public bool ShouldCreateCodeFile { get; private set; }
		public OptimizationLevel Level { get; private set; }
	}
}
