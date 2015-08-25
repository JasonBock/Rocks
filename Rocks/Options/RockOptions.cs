namespace Rocks.Options
{
	public sealed class RockOptions
	{
		public RockOptions(OptimizationSetting level = OptimizationSetting.Release, 
			CodeFileOptions codeFile = CodeFileOptions.None, 
			SerializationOptions serialization = SerializationOptions.NotSupported,
			CachingOptions caching = CachingOptions.UseCache, 
			AllowWarnings allowWarnings = AllowWarnings.No)
		{
			this.Optimization = level;
			this.CodeFile = codeFile;
			this.Serialization = serialization;
			this.Caching = caching;
			this.AllowWarnings = allowWarnings;
      }

		public override int GetHashCode()
		{
			return this.CodeFile.GetHashCode() ^
				((int)this.Serialization << 1).GetHashCode() ^
				((int)this.Optimization << 2).GetHashCode() ^
				((int)this.Caching << 3).GetHashCode() ^
				((int)this.AllowWarnings << 4).GetHashCode();
		}

		public AllowWarnings AllowWarnings { get; }
		public CachingOptions Caching { get; }
		public CodeFileOptions CodeFile { get; }
		public OptimizationSetting Optimization { get; }
		public SerializationOptions Serialization { get; }
	}
}
