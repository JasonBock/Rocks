using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Diagnostics;
using System.Reflection;

namespace Rocks.Task
{
	public sealed class RocksTask
		: AppDomainIsolatedTask
	{
		public override bool Execute()
		{
			this.Log.LogMessage($"Generating mocks for assembly {this.AssemblyLocation}...");
			var stopwatch = Stopwatch.StartNew();
			this.Result = new RockAssembly(Assembly.LoadFile(this.AssemblyLocation)).Result;
			stopwatch.Stop();
			this.Log.LogMessage($"Generating mocks for {this.AssemblyLocation} complete, results are in {this.Result.FullName} - total time: {stopwatch.Elapsed}.");
			return true;
		}

		public Assembly Result { get; private set; }

		[Required]
		public string AssemblyLocation { get; set; }
	}
}
