using Microsoft.CodeAnalysis;

namespace Rocks
{
	public sealed class Options
	{
		public Options()
			: this(OptimizationLevel.Release, CodeFileOptions.None, SerializationOptions.NotSupported)
		{ }

		public Options(CodeFileOptions codeFile)
			: this(OptimizationLevel.Release, codeFile, SerializationOptions.NotSupported)
		{ }

		public Options(OptimizationLevel level)
			: this(level, CodeFileOptions.None, SerializationOptions.NotSupported)
		{ }

		public Options(SerializationOptions serialization)
			: this(OptimizationLevel.Release, CodeFileOptions.None, serialization)
		{ }

		public Options(CodeFileOptions codeFile, SerializationOptions serialization)
			: this(OptimizationLevel.Release, codeFile, serialization)
		{ }

		public Options(OptimizationLevel level, CodeFileOptions codeFile)
			: this(level, codeFile, SerializationOptions.NotSupported)
		{ }

		public Options(OptimizationLevel level, SerializationOptions serialization)
			: this(level, CodeFileOptions.None, serialization)
		{ }

		public Options(OptimizationLevel level, CodeFileOptions codeFile, SerializationOptions serialization)
		{
			this.Level = level;
			this.CodeFile = codeFile;
			this.Serialization = serialization;
		}

		public CodeFileOptions CodeFile { get; private set; }
		public OptimizationLevel Level { get; private set; }
		public SerializationOptions Serialization { get; private set; }
	}
}
