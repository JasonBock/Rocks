using Microsoft.Build.Framework;
using Rocks.Options;
using System.Diagnostics;
using System.Reflection;
using MBU = Microsoft.Build.Utilities;

namespace Rocks
{
	public sealed class RocksTask
		: MBU.Task
	{
		public override bool Execute()
		{
			this.Log.LogMessage($"Generating mocks for assembly {this.AssemblyLocation}...");
			var stopwatch = Stopwatch.StartNew();
			this.Result = new RockAssembly(
				Assembly.LoadFile(this.AssemblyLocation),
				new RockOptions(codeFileDirectory: this.CodeFileDirectory)).Result;
			stopwatch.Stop();
			this.Log.LogMessage($"Generating mocks for {this.AssemblyLocation} complete, results are in {this.Result.FullName} - total time: {stopwatch.Elapsed}.");
			return true;
		}

		public Assembly? Result { get; private set; }

		[Required]
		public string? AssemblyLocation { get; set; }

		[Required]
		public string? CodeFileDirectory { get; set; }
	}
}
