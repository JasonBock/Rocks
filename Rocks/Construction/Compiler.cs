using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocks.Exceptions;
using Rocks.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Rocks.Construction
{
	internal abstract class Compiler<T>
		where T : Stream
	{
		protected Compiler(IEnumerable<SyntaxTree> trees, OptimizationSetting optimization, string assemblyName,
			ReadOnlyCollection<Assembly> referencedAssemblies, bool allowUnsafe, AllowWarnings allowWarnings)
		{
			this.Optimization = optimization;
			this.AssemblyName = assemblyName;
			this.Trees = trees;
			this.ReferencedAssemblies = referencedAssemblies;
			this.AllowUnsafe = allowUnsafe;
			this.AllowWarnings = allowWarnings;
      }

		internal void Compile()
		{
			var compilation = CSharpCompilation.Create(this.AssemblyName,
				options: new CSharpCompilationOptions(
					OutputKind.DynamicallyLinkedLibrary,
					optimizationLevel: this.Optimization == OptimizationSetting.Release ? 
						OptimizationLevel.Release : OptimizationLevel.Debug,
					allowUnsafe: this.AllowUnsafe),
				syntaxTrees: this.Trees,
				references: this.GetReferences());

			using (T assemblyStream = this.GetAssemblyStream(),
				pdbStream = this.GetPdbStream())
			{
				var results = compilation.Emit(assemblyStream,
					pdbStream: pdbStream);

				if (!results.Success || 
					(this.AllowWarnings == AllowWarnings.No && 
						results.Diagnostics.Length > 0 && 
						results.Diagnostics.Where(_ => _.Severity == DiagnosticSeverity.Hidden).ToArray().Length != results.Diagnostics.Length))
				{
					throw new CompilationException(results.Diagnostics);
				}

				this.ProcessStreams(assemblyStream, pdbStream);
			}

			this.Complete();
		}

		private MetadataReference[] GetReferences()
		{
			var references = new List<MetadataReference>(
				this.ReferencedAssemblies.Select(_ => MetadataReference.CreateFromFile(_.Location)));
			references.AddRange(new[]
				{
					MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
					MetadataReference.CreateFromFile(typeof(IMock).Assembly.Location),
					MetadataReference.CreateFromFile(typeof(Action<,,,,,,,,>).Assembly.Location),
				});
			return references.ToArray();
		}

		protected abstract T GetAssemblyStream();
		protected abstract T GetPdbStream();
		protected virtual void ProcessStreams(T assemblyStream, T pdbStream) { }
		protected virtual void Complete() { }

		internal AllowWarnings AllowWarnings { get; }
		internal string AssemblyName { get; }
		internal OptimizationSetting Optimization { get; }
		internal IEnumerable<SyntaxTree> Trees { get; }
		internal ReadOnlyCollection<Assembly> ReferencedAssemblies { get; }
		internal Assembly Result { get; set; }
		protected bool AllowUnsafe { get; private set; }
	}
}
