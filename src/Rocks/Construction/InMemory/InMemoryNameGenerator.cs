namespace Rocks.Construction.InMemory
{
	internal sealed class InMemoryNameGenerator
		: NameGenerator
	{
		private const string DefaultAssemblyName = "RockQuarry";

		internal InMemoryNameGenerator()
			: base(InMemoryNameGenerator.DefaultAssemblyName) { }
	}
}