using System;
using System.IO;

namespace Rocks.Options
{
	public sealed class RockOptions
	{
		public RockOptions(OptimizationSetting level = OptimizationSetting.Release,
			CodeFileOption codeFile = CodeFileOption.None,
			SerializationOption serialization = SerializationOption.NotSupported,
			CachingOption caching = CachingOption.UseCache,
			AllowWarning allowWarning = AllowWarning.No,
			string? codeFileDirectory = null) =>
			(this.Optimization, this.CodeFile, this.Serialization, this.Caching, this.AllowWarning, this.CodeFileDirectory) =
				(level, codeFile, serialization, caching, allowWarning, codeFileDirectory ?? Directory.GetCurrentDirectory());

		public override int GetHashCode() =>
			HashCode.Combine(this.CodeFile, this.Serialization, this.Optimization,
				this.Caching, this.AllowWarning, this.CodeFileDirectory);

		public AllowWarning AllowWarning { get; }
		public CachingOption Caching { get; }
		public CodeFileOption CodeFile { get; }
		public string CodeFileDirectory { get; }
		public OptimizationSetting Optimization { get; }
		public SerializationOption Serialization { get; }
	}
}