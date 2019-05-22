using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocks.Exceptions;
using Rocks.Options;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Rocks.Construction.Persistence
{
	internal sealed class PersistenceCompiler
		: Compiler<FileStream>
	{
		private readonly string assemblyPath;

		internal PersistenceCompiler(IEnumerable<SyntaxTree> trees, OptimizationSetting optimization, 
			string assemblyName, ReadOnlyCollection<Assembly> referencedAssemblies, string assemblyPath,
			bool allowUnsafe, AllowWarnings allowWarnings)
			: base(trees, optimization, assemblyName, referencedAssemblies, allowUnsafe, allowWarnings) => this.assemblyPath = assemblyPath;

		private FileStream GetAssemblyStream() =>
			new FileStream($"{Path.Combine(this.assemblyPath, this.AssemblyName)}.dll", FileMode.Create);

		private FileStream GetPdbStream() =>
			new FileStream($"{Path.Combine(this.assemblyPath, this.AssemblyName)}.pdb", FileMode.Create);

		protected override Assembly Emit(CSharpCompilation compilation)
		{
			string assemblyFileName;

			using (FileStream assemblyStream = this.GetAssemblyStream(),
				pdbStream = this.GetPdbStream())
			{
				var results = compilation.Emit(assemblyStream,
					pdbStream: pdbStream);
				var diagnostics = results.Diagnostics;

				if (this.AllowWarnings == AllowWarnings.No &&
					diagnostics.Length > 0 &&
					diagnostics.Where(_ => _.Severity == DiagnosticSeverity.Hidden).ToArray().Length != diagnostics.Length)
				{
					throw new CompilationException(diagnostics);
				}

				assemblyFileName = assemblyStream.Name;
			}

			return Assembly.LoadFile(assemblyFileName);
		}
   }
}