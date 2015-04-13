using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using System;

namespace Rocks.Construction
{
	internal sealed class InMemoryCompiler
		: Compiler<MemoryStream>
	{
		private static string DefaultAssemblyName = "RockQuarry";

		internal InMemoryCompiler(IEnumerable<SyntaxTree> trees, OptimizationLevel level, ReadOnlyCollection<Assembly> referencedAssemblies)
			: base(trees, level, InMemoryCompiler.DefaultAssemblyName, referencedAssemblies)
		{ }

		internal InMemoryCompiler(IEnumerable<SyntaxTree> trees, OptimizationLevel level, string assemblyName, ReadOnlyCollection<Assembly> referencedAssemblies)
			: base(trees, level, assemblyName, referencedAssemblies)
		{ }

		protected override MemoryStream GetAssemblyStream()
		{
			return new MemoryStream();
		}

		protected override MemoryStream GetPdbStream()
		{
			return new MemoryStream();
		}

		protected override void ProcessStreams(MemoryStream assemblyStream, MemoryStream pdbStream)
		{
			this.Result = Assembly.Load(assemblyStream.GetBuffer(), pdbStream.GetBuffer());
		}
	}
}
