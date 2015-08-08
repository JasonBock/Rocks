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
		private string assemblyFileName;
		private string assemblyPath;

		internal AssemblyCompiler(IEnumerable<SyntaxTree> trees, OptimizationSetting optimization, 
			string assemblyName, ReadOnlyCollection<Assembly> referencedAssemblies, string assemblyPath,
			bool allowUnsafe)
			: base(trees, optimization, assemblyName, referencedAssemblies, allowUnsafe)
		{
			this.assemblyPath = assemblyPath;
		}

		protected override FileStream GetAssemblyStream()
		{
			return new FileStream($"{Path.Combine(this.assemblyPath, this.AssemblyName)}.dll", FileMode.Create);
		}

		protected override FileStream GetPdbStream()
		{
			return new FileStream($"{Path.Combine(this.assemblyPath, this.AssemblyName)}.pdb", FileMode.Create);
		}

		protected override void ProcessStreams(FileStream assemblyStream, FileStream pdbStream)
		{
			this.assemblyFileName = assemblyStream.Name;
		}

		protected override void Complete()
		{
			this.Result = Assembly.LoadFile(this.assemblyFileName);
		}
	}
}
