using System.IO;

namespace Rocks.Options
{
	public sealed class RockOptions
	{
#if !NETCOREAPP1_1
		public RockOptions(OptimizationSetting level = OptimizationSetting.Release, 
			CodeFileOptions codeFile = CodeFileOptions.None, 
			SerializationOptions serialization = SerializationOptions.NotSupported,
			CachingOptions caching = CachingOptions.UseCache, 
			AllowWarnings allowWarnings = AllowWarnings.No,
			string codeFileDirectory = null)
		{
			this.Optimization = level;
			this.CodeFile = codeFile;
			this.Serialization = serialization;
			this.Caching = caching;
			this.AllowWarnings = allowWarnings;
			this.CodeFileDirectory = codeFileDirectory ?? Directory.GetCurrentDirectory();
		}

		public override int GetHashCode() =>
			this.CodeFile.GetHashCode() ^
				((int)this.Serialization << 1).GetHashCode() ^
				((int)this.Optimization << 2).GetHashCode() ^
				((int)this.Caching << 3).GetHashCode() ^
				((int)this.AllowWarnings << 4).GetHashCode() ^
				this.CodeFileDirectory.GetHashCode();
#else
		public RockOptions(OptimizationSetting level = OptimizationSetting.Release, 
			CodeFileOptions codeFile = CodeFileOptions.None, 
			CachingOptions caching = CachingOptions.UseCache, 
			AllowWarnings allowWarnings = AllowWarnings.No,
			string codeFileDirectory = null)
		{
			this.Optimization = level;
			this.CodeFile = codeFile;
			this.Caching = caching;
			this.AllowWarnings = allowWarnings;
			this.CodeFileDirectory = codeFileDirectory ?? Directory.GetCurrentDirectory();
		}

		public override int GetHashCode() =>
			this.CodeFile.GetHashCode() ^
				((int)this.Optimization << 2).GetHashCode() ^
				((int)this.Caching << 3).GetHashCode() ^
				((int)this.AllowWarnings << 4).GetHashCode() ^
				this.CodeFileDirectory.GetHashCode();
#endif

		public AllowWarnings AllowWarnings { get; }
		public CachingOptions Caching { get; }
		public CodeFileOptions CodeFile { get; }
		public string CodeFileDirectory { get; }
		public OptimizationSetting Optimization { get; }
#if !NETCOREAPP1_1
		public SerializationOptions Serialization { get; }
#endif
	}
}
