using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace Rocks.Analysis.Extensions;

internal static class IncrementalGeneratorInitializationContextExtensions
{
   internal static ImmutableArray<(string fileName, string code)> GetOutputCode() => 
		[];

   internal static void RegisterTypes(this IncrementalGeneratorInitializationContext self) => 
		self.RegisterPostInitializationOutput(static postInitializationContext =>
		{
			foreach(var (fileName, code) in GetOutputCode())
			{
				postInitializationContext.AddSource(fileName, SourceText.From(code, Encoding.UTF8));
			}
		});
}
