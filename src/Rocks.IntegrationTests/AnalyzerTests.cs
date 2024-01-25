namespace Rocks.IntegrationTests;

// This is an odd one, but all I'm doing
// is making sure that the analyzer is 
// working correctly. So, I create a type
// that I know the analyzer should create a diagnostic for,
// and then put a #pragma around it.
// Now, there is an informational diagnostic
// that will state if a #pragma is not needed (IDE0079),
// but...I can't make that a "warning" (and then an error),
// which would be nice because then it would tell me
// if the analyzer wasn't firing. At least
// I can't figure out how to do that. But this
// is sufficient for now.

#pragma warning disable ROCK1 // Cannot Mock Sealed Types
[RockCreate<AnalyzerTests>]
#pragma warning restore ROCK1 // Cannot Mock Sealed Types
public sealed class AnalyzerTests { }