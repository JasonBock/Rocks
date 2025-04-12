namespace Rocks.CodeGenerationTest;

// TODO: Maybe break this up a bit to have the 2nd parameter
// be a "params ReadOnlySpan<string>" and then assign that to
// a readonly/immutable array of strings as a property.
#pragma warning disable CA1819 // Properties should not return arrays
public sealed record TypeAliasesMapping(Type type, string[] aliases);
#pragma warning restore CA1819 // Properties should not return arrays