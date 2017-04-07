using System;

namespace Rocks.Construction.InMemory
{
	internal sealed class InMemoryNameGenerator
		: NameGenerator
	{
		private static string DefaultAssemblyName = "RockQuarry";

		internal InMemoryNameGenerator()
		{
#if !NETCOREAPP1_1
			this.AssemblyName = InMemoryNameGenerator.DefaultAssemblyName;
#else
			this.AssemblyName = $"{InMemoryNameGenerator.DefaultAssemblyName}{Guid.NewGuid().ToString("N")}";
#endif
		}
	}
}
