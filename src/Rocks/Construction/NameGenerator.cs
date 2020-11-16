namespace Rocks.Construction
{
	internal abstract class NameGenerator
	{
		protected NameGenerator(string assemblyName) => 
			this.AssemblyName = assemblyName;

		public string AssemblyName { get; }
	}
}