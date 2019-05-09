using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocks.Exceptions;
using Rocks.Extensions;
using Rocks.Options;
using Rocks.Templates;
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
						catch (FileLoadException) { }
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
						if(!string.IsNullOrWhiteSpace(platformAssemblyName))
						{
							assemblies.Add(Assembly.Load(new AssemblyName(platformAssemblyName)));
						}
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
				outputKind: OutputKind.DynamicallyLinkedLibrary,
				optimizationLevel: this.Optimization == OptimizationSetting.Release ?
					OptimizationLevel.Release : OptimizationLevel.Debug,
				allowUnsafe: this.AllowUnsafe,
				nullableContextOptions: NullableContextOptions.Enable);
			var compilation = CSharpCompilation.Create(this.AssemblyName,
				options: options,
				syntaxTrees: this.Trees,
				references: this.GetReferences());
			var diagnostics = compilation.GetDiagnostics();

			if (this.AllowWarnings == AllowWarnings.No &&
				diagnostics.Length > 0 &&
				diagnostics.Where(_ => _.Severity == DiagnosticSeverity.Hidden).ToArray().Length != diagnostics.Length)
			{
				throw new CompilationException(diagnostics);
			}

			return this.Emit(compilation);
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

		protected abstract Assembly Emit(CSharpCompilation compilation);

		internal AllowWarnings AllowWarnings { get; }
		internal string AssemblyName { get; }
		internal OptimizationSetting Optimization { get; }
		internal IEnumerable<SyntaxTree> Trees { get; }
		internal ReadOnlyCollection<Assembly> ReferencedAssemblies { get; }
		protected bool AllowUnsafe { get; }
	}
}