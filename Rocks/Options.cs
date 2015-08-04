using Microsoft.CodeAnalysis;

namespace Rocks
{
	public sealed class Options
	{
		public Options()
			: this(OptimizationLevel.Release, CodeFileOptions.None, SerializationOptions.NotSupported, 
				CachingOptions.UseCache)
		{ }

		public Options(CodeFileOptions codeFile)
			: this(OptimizationLevel.Release, codeFile, SerializationOptions.NotSupported, 
				CachingOptions.UseCache)
		{ }

		public Options(OptimizationLevel level)
			: this(level, CodeFileOptions.None, SerializationOptions.NotSupported,
				CachingOptions.UseCache)
		{ }

		public Options(SerializationOptions serialization)
			: this(OptimizationLevel.Release, CodeFileOptions.None, serialization,
				CachingOptions.UseCache)
		{ }

		public Options(CachingOptions caching)
			: this(OptimizationLevel.Release, CodeFileOptions.None, SerializationOptions.NotSupported, 
				caching)
		{ }

		public Options(OptimizationLevel level, CodeFileOptions codeFile)
			: this(level, codeFile, SerializationOptions.NotSupported,
				CachingOptions.UseCache)
		{ }

		public Options(OptimizationLevel level, SerializationOptions serialization)
			: this(level, CodeFileOptions.None, serialization,
				CachingOptions.UseCache)
		{ }

		public Options(OptimizationLevel level, CachingOptions caching)
			: this(level, CodeFileOptions.None, SerializationOptions.NotSupported, caching)
		{ }

		public Options(CodeFileOptions codeFile, SerializationOptions serialization)
			: this(OptimizationLevel.Release, codeFile, serialization,
				CachingOptions.UseCache)
		{ }

		public Options(CodeFileOptions codeFile, CachingOptions caching)
			: this(OptimizationLevel.Release, codeFile, SerializationOptions.NotSupported, caching)
		{ }

		public Options(SerializationOptions serialization, CachingOptions caching)
			: this(OptimizationLevel.Release, CodeFileOptions.None, serialization, caching)
		{ }

		public Options(OptimizationLevel level, CodeFileOptions codeFile, SerializationOptions serialization)
			: this(level, codeFile, serialization, CachingOptions.UseCache)
		{ }

		public Options(OptimizationLevel level, SerializationOptions serialization, CachingOptions caching)
			: this(level, CodeFileOptions.None, serialization, caching)
		{ }

		public Options(OptimizationLevel level, CodeFileOptions codeFile, CachingOptions caching)
			: this(level, codeFile, SerializationOptions.NotSupported, caching)
		{ }

		public Options(CodeFileOptions codeFile, SerializationOptions serialization, CachingOptions caching)
			: this(OptimizationLevel.Release, codeFile, serialization, caching)
		{ }

		public Options(OptimizationLevel level, CodeFileOptions codeFile, SerializationOptions serialization,
			CachingOptions caching)
		{
			this.Level = level;
			this.CodeFile = codeFile;
			this.Serialization = serialization;
			this.Caching = caching;
		}

		public override int GetHashCode()
		{
			return this.CodeFile.GetHashCode() ^
				((int)this.Serialization << 1).GetHashCode() ^
				((int)this.Level << 2).GetHashCode() ^
				((int)this.Caching << 3).GetHashCode();
		}

		public CachingOptions Caching { get; }
		public CodeFileOptions CodeFile { get; }
		public OptimizationLevel Level { get; }
		public SerializationOptions Serialization { get; }
	}
}
