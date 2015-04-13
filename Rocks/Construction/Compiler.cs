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
	internal abstract class Compiler<T>
		where T : Stream
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

			using (T assemblyStream = this.GetAssemblyStream(),
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
					MetadataReference.CreateFromAssembly(typeof(IMock).Assembly),
					MetadataReference.CreateFromAssembly(typeof(Action<,,,,,,,,>).Assembly),
				});
			return references.ToArray();
		}

		protected abstract T GetAssemblyStream();
		protected abstract T GetPdbStream();
		protected virtual void ProcessStreams(T assemblyStream, T pdbStream) { }

		internal string AssemblyName { get; }
		internal OptimizationLevel Level { get; }
		internal IEnumerable<SyntaxTree> Trees { get; }
		internal ReadOnlyCollection<Assembly> ReferencedAssemblies { get; }
		internal Assembly Result { get; set; }
	}
}
