using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocks.Exceptions;
using Rocks.Extensions;
using Rocks.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Rocks.Construction
{
	internal abstract class Compiler
	{
		// Lifted from:
		// https://github.com/dotnet/roslyn/wiki/Runtime-code-generation-using-Roslyn-compilations-in-.NET-Core-App
		protected static Lazy<HashSet<MetadataReference>> assemblyReferences =
			new Lazy<HashSet<MetadataReference>>(() =>
			{
				void LoadDependencies(HashSet<Assembly> loadedAssemblies, Assembly fromAssembly)
				{
					foreach (var reference in fromAssembly.GetReferencedAssemblies())
					{
						try
						{
							var assembly = Assembly.Load(reference);

							if (loadedAssemblies.Add(assembly))
							{
								LoadDependencies(loadedAssemblies, assembly);
							}
						}
						catch (FileNotFoundException) { }
					}
				}

				var assemblies = new HashSet<Assembly>();

				var trustedPlatformAssemblies =
					(AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string)?.Split(Path.PathSeparator);

				if (trustedPlatformAssemblies != null)
				{
					var platformAssemblyPaths = new HashSet<string>(trustedPlatformAssemblies);
					var platformAssemblyNames = platformAssemblyPaths.Select(Path.GetFileNameWithoutExtension);

					foreach (var platformAssemblyName in platformAssemblyNames)
					{
						assemblies.Add(Assembly.Load(new AssemblyName(platformAssemblyName)));
					}

					assemblies.Add(typeof(Exception).Assembly);
				}
				else
				{
					assemblies.Add(typeof(object).Assembly);
					assemblies.Add(typeof(IMock).Assembly);
					assemblies.Add(typeof(Action<,,,,,,,,>).Assembly);
				}

				foreach (var assembly in assemblies.ToList())
				{
					LoadDependencies(assemblies, assembly);
				}

				return new HashSet<MetadataReference>(assemblies.Transform());
			});
	}

	internal abstract class Compiler<T>
		: Compiler
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

		internal Assembly Compile()
		{
			var options = new CSharpCompilationOptions(
				OutputKind.DynamicallyLinkedLibrary,
				optimizationLevel: this.Optimization == OptimizationSetting.Release ?
					OptimizationLevel.Release : OptimizationLevel.Debug,
				allowUnsafe: this.AllowUnsafe);

			var compilation = CSharpCompilation.Create(this.AssemblyName, 
				options: options,
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

				return this.ProcessStreams(assemblyStream, pdbStream);
			}
		}

		private MetadataReference[] GetReferences()
		{
			var references = Compiler.assemblyReferences.Value;

			foreach (var reference in this.ReferencedAssemblies.Transform())
			{
				references.Add(reference);
			}

			return references.ToArray();
		}

		protected abstract T GetAssemblyStream();
		protected abstract T GetPdbStream();
		protected abstract Assembly ProcessStreams(T assemblyStream, T pdbStream);

		internal AllowWarnings AllowWarnings { get; }
		internal string AssemblyName { get; }
		internal OptimizationSetting Optimization { get; }
		internal IEnumerable<SyntaxTree> Trees { get; }
		internal ReadOnlyCollection<Assembly> ReferencedAssemblies { get; }
		protected bool AllowUnsafe { get; private set; }
	}
}