using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Rocks.Extensions;

internal static class CompilationExtensions
{
   internal static ImmutableArray<string> GetAliases(this Compilation self) => 
		self.References.Where(_ => _.Properties.Aliases.Length > 0)
		   .Select(_ => _.Properties.Aliases[0])
		   .Distinct()
		   .OrderBy(_ => _).ToImmutableArray();
}