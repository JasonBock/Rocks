using System.Collections.Immutable;

namespace Rocks.CodeGenerationTest;

public sealed record GeneratorResults(
	string? AssemblyName, int DiscoveredTypesCount, ImmutableArray<Issue> CreateIssues, ImmutableArray<Issue> MakeIssues);