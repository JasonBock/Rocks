using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;

namespace Rocks.Analysis.Descriptors;

internal static class CannotMockObsoleteTypeDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(Id, Title,
			Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				Id, Title));

	internal const string Id = "ROCK2";
	internal const string Message = "The type {0} is obsolete and cannot be mocked";
	internal const string Title = "Cannot Mock Obsolete Types";
}