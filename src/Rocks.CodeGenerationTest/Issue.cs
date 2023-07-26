using Microsoft.CodeAnalysis;

namespace Rocks.CodeGenerationTest;

public record Issue(string Id, DiagnosticSeverity Severity, string Description, Location Location);