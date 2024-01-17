using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;

namespace Rocks.Descriptors;

internal static class DuplicateConstructorsDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(DuplicateConstructorsDescriptor.Id, DuplicateConstructorsDescriptor.Title,
			DuplicateConstructorsDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				DuplicateConstructorsDescriptor.Id, DuplicateConstructorsDescriptor.Title));

	internal const string Id = "ROCK12";
	internal const string Message = "The type {0} will have duplicate constructors generated in the mock";
	internal const string Title = "Duplicate Constructors";
}