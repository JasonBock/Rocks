using System;
using System.IO;

namespace Rocks.Options
{
	public sealed class RockOptions
	{
		public RockOptions(OptimizationSetting level = OptimizationSetting.Release,
			CodeFileOptions codeFile = CodeFileOptions.None,
			SerializationOptions serialization = SerializationOptions.NotSupported,
			CachingOptions caching = CachingOptions.UseCache,
			AllowWarnings allowWarnings = AllowWarnings.No,
			string? codeFileDirectory = null) =>
			(this.Optimization, this.CodeFile, this.Serialization, this.Caching, this.AllowWarnings, this.CodeFileDirectory) =
				(level, codeFile, serialization, caching, allowWarnings, codeFileDirectory ?? Directory.GetCurrentDirectory());

		public override int GetHashCode() =>
			HashCode.Combine(this.CodeFile, this.Serialization, this.Optimization,
				this.Caching, this.AllowWarnings, this.CodeFileDirectory);

		public AllowWarnings AllowWarnings { get; }
		public CachingOptions Caching { get; }
		public CodeFileOptions CodeFile { get; }
		public string CodeFileDirectory { get; }
		public OptimizationSetting Optimization { get; }
		public SerializationOptions Serialization { get; }
	}
}