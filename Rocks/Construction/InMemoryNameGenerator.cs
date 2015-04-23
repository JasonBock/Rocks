using System;

namespace Rocks.Construction
{
	internal sealed class InMemoryNameGenerator
		: NameGenerator
	{
		private static string DefaultAssemblyName = "RockQuarry";

		internal InMemoryNameGenerator()
		{
			this.AssemblyName = InMemoryNameGenerator.DefaultAssemblyName;
		}
	}
}
