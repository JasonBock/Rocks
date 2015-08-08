namespace Rocks
{
	public sealed class Options
	{
		public Options()
			: this(OptimizationSetting.Release, CodeFileOptions.None, SerializationOptions.NotSupported, 
				CachingOptions.UseCache)
		{ }

		public Options(CodeFileOptions codeFile)
			: this(OptimizationSetting.Release, codeFile, SerializationOptions.NotSupported, 
				CachingOptions.UseCache)
		{ }

		public Options(OptimizationSetting level)
			: this(level, CodeFileOptions.None, SerializationOptions.NotSupported,
				CachingOptions.UseCache)
		{ }

		public Options(SerializationOptions serialization)
			: this(OptimizationSetting.Release, CodeFileOptions.None, serialization,
				CachingOptions.UseCache)
		{ }

		public Options(CachingOptions caching)
			: this(OptimizationSetting.Release, CodeFileOptions.None, SerializationOptions.NotSupported, 
				caching)
		{ }

		public Options(OptimizationSetting level, CodeFileOptions codeFile)
			: this(level, codeFile, SerializationOptions.NotSupported,
				CachingOptions.UseCache)
		{ }

		public Options(OptimizationSetting level, SerializationOptions serialization)
			: this(level, CodeFileOptions.None, serialization,
				CachingOptions.UseCache)
		{ }

		public Options(OptimizationSetting level, CachingOptions caching)
			: this(level, CodeFileOptions.None, SerializationOptions.NotSupported, caching)
		{ }

		public Options(CodeFileOptions codeFile, SerializationOptions serialization)
			: this(OptimizationSetting.Release, codeFile, serialization,
				CachingOptions.UseCache)
		{ }

		public Options(CodeFileOptions codeFile, CachingOptions caching)
			: this(OptimizationSetting.Release, codeFile, SerializationOptions.NotSupported, caching)
		{ }

		public Options(SerializationOptions serialization, CachingOptions caching)
			: this(OptimizationSetting.Release, CodeFileOptions.None, serialization, caching)
		{ }

		public Options(OptimizationSetting level, CodeFileOptions codeFile, SerializationOptions serialization)
			: this(level, codeFile, serialization, CachingOptions.UseCache)
		{ }

		public Options(OptimizationSetting level, SerializationOptions serialization, CachingOptions caching)
			: this(level, CodeFileOptions.None, serialization, caching)
		{ }

		public Options(OptimizationSetting level, CodeFileOptions codeFile, CachingOptions caching)
			: this(level, codeFile, SerializationOptions.NotSupported, caching)
		{ }

		public Options(CodeFileOptions codeFile, SerializationOptions serialization, CachingOptions caching)
			: this(OptimizationSetting.Release, codeFile, serialization, caching)
		{ }

		public Options(OptimizationSetting level, CodeFileOptions codeFile, SerializationOptions serialization,
			CachingOptions caching)
		{
			this.Optimization = level;
			this.CodeFile = codeFile;
			this.Serialization = serialization;
			this.Caching = caching;
		}

		public override int GetHashCode()
		{
			return this.CodeFile.GetHashCode() ^
				((int)this.Serialization << 1).GetHashCode() ^
				((int)this.Optimization << 2).GetHashCode() ^
				((int)this.Caching << 3).GetHashCode();
		}

		public CachingOptions Caching { get; }
		public CodeFileOptions CodeFile { get; }
		public OptimizationSetting Optimization { get; }
		public SerializationOptions Serialization { get; }
	}
}
