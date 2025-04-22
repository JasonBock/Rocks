using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Rocks.CodeGenerationTest")]
[assembly: InternalsVisibleTo("Rocks.Performance")]
[assembly: InternalsVisibleTo("Rocks.Analysis.Tests")]

internal static class Constants
{
	internal const string RockAttributeName = "Rocks.Runtime.RockAttribute";
	internal const string RockPartialAttributeName = "Rocks.Runtime.RockPartialAttribute";
}