using System;

namespace Rocks.Construction.InMemory
{
	internal sealed class InMemoryNameGenerator
		: NameGenerator
	{
		private static string DefaultAssemblyName = "RockQuarry";

		internal InMemoryNameGenerator() =>
			this.AssemblyName = InMemoryNameGenerator.DefaultAssemblyName;
	}
}
