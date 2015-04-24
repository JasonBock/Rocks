using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace Rocks.Construction
{
	internal sealed class InMemoryCompiler
		: Compiler<MemoryStream>
	{
		internal InMemoryCompiler(IEnumerable<SyntaxTree> trees, OptimizationLevel level, ReadOnlyCollection<Assembly> referencedAssemblies,
			bool allowUnsafe)
			: base(trees, level, new InMemoryNameGenerator().AssemblyName, referencedAssemblies, allowUnsafe)
		{ }

		protected override MemoryStream GetAssemblyStream()
		{
			return new MemoryStream();
		}

		protected override MemoryStream GetPdbStream()
		{
			return new MemoryStream();
		}

		protected override void ProcessStreams(MemoryStream assemblyStream, MemoryStream pdbStream)
		{
			this.Result = Assembly.Load(assemblyStream.GetBuffer(), pdbStream.GetBuffer());
		}
	}
}
