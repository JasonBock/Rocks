using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Rocks.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Rocks
{
	internal sealed class Compiler
	{
		internal Compiler(Type baseType, IEnumerable<SyntaxTree> trees, Options options)
		{
			this.Options = options;
			this.BaseType = baseType;
			this.Trees = trees;
			this.Assembly = this.Create();
		}

		private Assembly Create()
		{
			var compilation = CSharpCompilation.Create("RockQuarry",
				options: new CSharpCompilationOptions(
					OutputKind.DynamicallyLinkedLibrary,
					optimizationLevel: this.Options.Level),
				syntaxTrees: this.Trees,
				references: new[]
				{
					MetadataReference.CreateFromAssembly(typeof(object).Assembly),
					MetadataReference.CreateFromAssembly(typeof(IRock).Assembly),
					MetadataReference.CreateFromAssembly(typeof(Action<,,,,,,,,>).Assembly),
					MetadataReference.CreateFromAssembly(this.BaseType.Assembly)
				});

			using (MemoryStream assemblyStream = new MemoryStream(),
				pdbStream = new MemoryStream())
			{
				var results = compilation.Emit(assemblyStream,
					pdbStream: pdbStream);

				if (!results.Success)
				{
					throw new CompilationException(results.Diagnostics);
				}

				return Assembly.Load(assemblyStream.GetBuffer(), pdbStream.GetBuffer());
			}
		}

		internal Options Options { get; private set; }
		internal Assembly Assembly { get; private set; }
		internal IEnumerable<SyntaxTree> Trees { get; private set; }
		internal Type BaseType { get; private set; }
	}
}
