using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocks.Exceptions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace Rocks.Construction
{
	internal abstract class Compiler
	{
		protected Compiler(IEnumerable<SyntaxTree> trees, OptimizationLevel level, string assemblyName,
			ReadOnlyCollection<Assembly> referencedAssemblies)
		{
			this.Level = level;
			this.AssemblyName = assemblyName;
			this.Trees = trees;
			this.ReferencedAssemblies = referencedAssemblies;
		}

		internal void Compile()
		{
			var compilation = CSharpCompilation.Create(this.AssemblyName,
				options: new CSharpCompilationOptions(
					OutputKind.DynamicallyLinkedLibrary,
					optimizationLevel: this.Level),
				syntaxTrees: this.Trees,
				references: this.GetReferences());

			using (MemoryStream assemblyStream = this.GetAssemblyStream(),
				pdbStream = this.GetPdbStream())
			{
				var results = compilation.Emit(assemblyStream,
					pdbStream: pdbStream);

				if (!results.Success)
				{
					throw new CompilationException(results.Diagnostics);
				}

				this.ProcessStreams(assemblyStream, pdbStream);
			}
		}

		private MetadataReference[] GetReferences()
		{
			var references = new List<MetadataReference>(
				this.ReferencedAssemblies.Select(_ => MetadataReference.CreateFromAssembly(_)));
			references.AddRange(new[]
				{
					MetadataReference.CreateFromAssembly(typeof(object).Assembly),
					MetadataReference.CreateFromAssembly(typeof(IRock).Assembly),
					MetadataReference.CreateFromAssembly(typeof(Action<,,,,,,,,>).Assembly),
				});
			return references.ToArray();
		}

		protected abstract MemoryStream GetAssemblyStream();
		protected abstract MemoryStream GetPdbStream();
		protected virtual void ProcessStreams(MemoryStream assemblyStream, MemoryStream pdbStream) { }

		internal Assembly Assembly { get; set; }
		internal IEnumerable<SyntaxTree> Trees { get; private set; }
		internal ReadOnlyCollection<Assembly> ReferencedAssemblies { get; private set; }
		internal OptimizationLevel Level { get; private set; }
		internal string AssemblyName { get; private set; }
	}
}
