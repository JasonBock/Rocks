using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
#if NETCOREAPP1_1
using System.Runtime.Loader;
#endif
using Microsoft.CodeAnalysis;
using Rocks.Options;

namespace Rocks.Construction.InMemory
{
	internal sealed class InMemoryCompiler
		: Compiler<MemoryStream>
	{
		internal InMemoryCompiler(IEnumerable<SyntaxTree> trees, OptimizationSetting optimization, ReadOnlyCollection<Assembly> referencedAssemblies,
			bool allowUnsafe, AllowWarnings allowWarnings)
			: base(trees, optimization, new InMemoryNameGenerator().AssemblyName, referencedAssemblies, allowUnsafe, allowWarnings)
		{ }

		protected override MemoryStream GetAssemblyStream() => new MemoryStream();

		protected override MemoryStream GetPdbStream() => new MemoryStream();

#if !NETCOREAPP1_1
		protected override void ProcessStreams(MemoryStream assemblyStream, MemoryStream pdbStream) =>
			this.Result = Assembly.Load(assemblyStream.ToArray(), pdbStream.ToArray());
#else
		protected override void ProcessStreams(MemoryStream assemblyStream, MemoryStream pdbStream)
		{
			assemblyStream.Position = 0;
			pdbStream.Position = 0;
			this.Result = AssemblyLoadContext.Default.LoadFromStream(assemblyStream, pdbStream);
		}
#endif
	}
}
