namespace Rocks.CodeGenerationTest;

#pragma warning disable CA1819 // Properties should not return arrays
public sealed record TypeAliasesMapping(Type type, string[] aliases);
#pragma warning restore CA1819 // Properties should not return arrays