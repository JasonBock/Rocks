using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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

		protected override Assembly Emit(CSharpCompilation compilation)
		{
			using (MemoryStream assemblyStream = new MemoryStream(),
				pdbStream = new MemoryStream())
			{
				var results = compilation.Emit(assemblyStream,
					pdbStream: pdbStream);

				return Assembly.Load(assemblyStream.ToArray(), pdbStream.ToArray());
			}
		}
   }
}