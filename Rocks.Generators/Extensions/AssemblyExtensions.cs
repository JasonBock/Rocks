using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Text;

namespace Rocks.Extensions
{
	internal static class AssemblyExtensions
	{
		internal static (ImmutableDictionary<string, ITypeSymbol>, Compilation) LoadSymbols(
			this Assembly self, ImmutableArray<EmbeddedTypeInformation> embeddedTypes, GeneratorExecutionContext context)
		{
			static SourceText GetText(Assembly assembly, string typeResourceName)
			{
				using var stream = assembly.GetManifestResourceStream(typeResourceName);
				using var reader = new StreamReader(stream);
				return SourceText.From(reader.ReadToEnd(), Encoding.UTF8);
			}

			var trees = new List<SyntaxTree>();
			var options = (context.Compilation as CSharpCompilation)!.SyntaxTrees[0].Options as CSharpParseOptions;

			foreach (var embeddedType in embeddedTypes)
			{
				var code = GetText(self, embeddedType.TypeResourceName);
				context.AddSource(embeddedType.TypeResourceName, code);
				trees.Add(CSharpSyntaxTree.ParseText(code, options));
			}

			var compilation = context.Compilation.AddSyntaxTrees(trees);
			var embeddedSymbols = ImmutableDictionary.CreateBuilder<string, ITypeSymbol>();

			foreach(var embeddedType in embeddedTypes)
			{
				embeddedSymbols.Add(embeddedType.TypeName, compilation.GetTypeByMetadataName(embeddedType.TypeName)!);
			}

			return (embeddedSymbols.ToImmutable(), compilation);
		}
	}
}