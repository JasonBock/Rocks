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
					//throw new CompilationException(results.Diagnostics);
				}

				this.ProcessStreams(assemblyStream, pdbStream);
			}

			this.Complete();
		}

#if !NETCOREAPP1_1
		private MetadataReference[] GetReferences()
		{
			var references = new List<MetadataReference>(
				this.ReferencedAssemblies.Select(_ => MetadataReference.CreateFromFile(_.Location)));
			references.AddRange(new[]
				{
					MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
					MetadataReference.CreateFromFile(typeof(IMock).GetTypeInfo().Assembly.Location),
					MetadataReference.CreateFromFile(typeof(Action<,,,,,,,,>).GetTypeInfo().Assembly.Location),
				});
			return references.ToArray();
		}
#else
		private static HashSet<Assembly> GetAssembliesList()
		{
			var path = AppContext.BaseDirectory;
			var assemblyFiles = Directory.EnumerateFiles(path, "*.dll");
			var assemblyNames = assemblyFiles.Select(Path.GetFileNameWithoutExtension).ToList();
			var assemblies = new HashSet<Assembly>();

			foreach (var assemblyName in assemblyNames)
			{
				assemblies.Add(Assembly.Load(new AssemblyName(assemblyName)));
			}

			assemblies.Add(typeof(Exception).GetTypeInfo().Assembly);

			foreach (var assembly in assemblies.ToList())
			{
				LoadDependencies(assemblies, assembly);
			}

			return assemblies;
		}

		private static void LoadDependencies(HashSet<Assembly> loadedAssemblies, Assembly fromAssembly)
		{
			foreach (var reference in fromAssembly.GetReferencedAssemblies())
			{
				var asm = Assembly.Load(reference);
				if (loadedAssemblies.Add(asm))
				{
					LoadDependencies(loadedAssemblies, asm);
				}
			}
		}

		private MetadataReference[] GetReferences()
		{
			// TODO: Should seriously give this a try:
			// https://github.com/dotnet/orleans/blob/master/src/OrleansCodeGenerator/CodeGeneratorCommon.cs#L65
			// https://github.com/dotnet/orleans/blob/master/vNext/src/Orleans/Shims/AppDomain.cs
			return GetAssembliesList()
				.Where(asm => !asm.IsDynamic && !string.IsNullOrWhiteSpace(asm.Location))
				.Select(asm => MetadataReference.CreateFromFile(asm.Location))
				.Cast<MetadataReference>()
				.ToArray();

			// Lifted from:
			// http://stackoverflow.com/questions/38943899/net-core-cs0012-object-is-defined-in-an-assembly-that-is-not-referenced
			// http://stackoverflow.com/questions/39257074/net-core-amd-roslyn-csharpcompilation-the-type-object-is-defined-in-an-assem
			//var netCoreAppLocationDirectory = Path.GetDirectoryName(typeof(object).GetTypeInfo().Assembly.Location);

			//var assemblyPaths = new HashSet<string>(
			//	this.ReferencedAssemblies.Select(_ => _.Location));
			//assemblyPaths.Add(typeof(object).GetTypeInfo().Assembly.Location);
			//assemblyPaths.Add(typeof(IMock).GetTypeInfo().Assembly.Location);
			////assemblyPaths.Add(typeof(Action<,,,,,,,,>).GetTypeInfo().Assembly.Location);
			//assemblyPaths.Add(Path.Combine(netCoreAppLocationDirectory, "mscorlib.dll"));
			////assemblyPaths.Add(Path.Combine(netCoreAppLocationDirectory, "System.ObjectModel.dll"));
			//assemblyPaths.Add(Path.Combine(netCoreAppLocationDirectory, "System.Runtime.dll"));

			//return assemblyPaths.Select(_ => MetadataReference.CreateFromFile(_)).ToArray();

			//var references = new List<MetadataReference>(
			//	this.ReferencedAssemblies.Select(_ => MetadataReference.CreateFromFile(_.Location)));
			//var netCoreAppLocationDirectory = Directory.GetParent(typeof(object).GetTypeInfo().Assembly.Location);
			//references.AddRange(new[]
			//	{
			//		MetadataReference.CreateFromFile(Path.Combine(netCoreAppLocationDirectory.FullName, "mscorlib.dll")),
			//		MetadataReference.CreateFromFile(Path.Combine(netCoreAppLocationDirectory.FullName, "System.ObjectModel.dll")),
			//		MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
			//		MetadataReference.CreateFromFile(typeof(IMock).GetTypeInfo().Assembly.Location),
			//		MetadataReference.CreateFromFile(typeof(Action<,,,,,,,,>).GetTypeInfo().Assembly.Location),
			//		MetadataReference.CreateFromFile(typeof(ReadOnlyDictionary<,>).GetTypeInfo().Assembly.Location)
			//	});
			//return references.ToArray();
		}
#endif
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
