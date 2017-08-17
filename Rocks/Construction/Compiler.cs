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
	internal abstract class Compiler
	{
		// Lifted from:
		// https://github.com/dotnet/roslyn/wiki/Runtime-code-generation-using-Roslyn-compilations-in-.NET-Core-App
		protected static Lazy<HashSet<Assembly>> assemblyReferences =
			new Lazy<HashSet<Assembly>>(() =>
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

					assemblies.Add(typeof(Exception).GetTypeInfo().Assembly);

					foreach (var assembly in assemblies.ToList())
					{
						LoadDependencies(assemblies, assembly);
					}
				}

				return assemblies;
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

		internal void Compile()
		{
			var options = new CSharpCompilationOptions(
					OutputKind.DynamicallyLinkedLibrary,
					optimizationLevel: this.Optimization == OptimizationSetting.Release ?
						OptimizationLevel.Release : OptimizationLevel.Debug,
					allowUnsafe: this.AllowUnsafe);

#if NETCOREAPP1_1
			// CoreFX bug https://github.com/dotnet/corefx/issues/5540 
			// to workaround it, we are calling internal WithTopLevelBinderFlags(BinderFlags.IgnoreCorLibraryDuplicatedTypes) 
			// TODO: this API will be public in the future releases of Roslyn. 
			// This work is tracked in https://github.com/dotnet/roslyn/issues/5855 
			// Once it's public, we should replace the internal reflection API call by the public one. 
			var method = typeof(CSharpCompilationOptions).GetMethod("WithTopLevelBinderFlags", BindingFlags.NonPublic | BindingFlags.Instance);
			// we need to pass BinderFlags.IgnoreCorLibraryDuplicatedTypes, but it's an internal class 
			// http://source.roslyn.io/#Microsoft.CodeAnalysis.CSharp/Binder/BinderFlags.cs,00f268571bb66b73 
			options = (CSharpCompilationOptions)method.Invoke(options, new object[] { 1u << 26 });

			options = options.WithSpecificDiagnosticOptions(new Dictionary<string, ReportDiagnostic>
			{
				{ "CS1702", ReportDiagnostic.Hidden }
			});
#endif

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

				this.ProcessStreams(assemblyStream, pdbStream);
			}

			this.Complete();
		}

		private MetadataReference[] GetReferences()
		{
			var references = Compiler.assemblyReferences.Value;

			if (references.Count == 0)
			{
				var referencedAssemblies = new List<MetadataReference>(
					this.ReferencedAssemblies.Select(_ => MetadataReference.CreateFromFile(_.Location)));
				referencedAssemblies.AddRange(new[]
					{
						MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
						MetadataReference.CreateFromFile(typeof(IMock).GetTypeInfo().Assembly.Location),
						MetadataReference.CreateFromFile(typeof(Action<,,,,,,,,>).GetTypeInfo().Assembly.Location),
					});
				return referencedAssemblies.ToArray();
			}
			else
			{
				this.ReferencedAssemblies.ToList().ForEach(_ => references.Add(_));

				return references
					.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
					.Select(_ => MetadataReference.CreateFromFile(_.Location))
					.Cast<MetadataReference>()
					.ToArray();
			}
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
