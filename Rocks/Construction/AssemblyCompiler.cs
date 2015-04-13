using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace Rocks.Construction
{
	internal sealed class AssemblyCompiler
		: Compiler<FileStream>
	{
		internal AssemblyCompiler(IEnumerable<SyntaxTree> trees, OptimizationLevel level, 
			string assemblyName, ReadOnlyCollection<Assembly> referencedAssemblies)
			: base(trees, level, assemblyName, referencedAssemblies)
		{ }

		protected override FileStream GetAssemblyStream()
		{
			return new FileStream($"{this.AssemblyName}.dll", FileMode.Create);
		}

		protected override FileStream GetPdbStream()
		{
			return new FileStream($"{this.AssemblyName}.pdb", FileMode.Create);
		}
	}
}
