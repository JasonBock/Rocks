using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;

namespace Rocks.Analysis.Descriptors;

internal static class CannotMockObsoleteTypeDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(CannotMockObsoleteTypeDescriptor.Id, CannotMockObsoleteTypeDescriptor.Title,
			CannotMockObsoleteTypeDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				CannotMockObsoleteTypeDescriptor.Id, CannotMockObsoleteTypeDescriptor.Title));

	internal const string Id = "ROCK2";
	internal const string Message = "The type {0} is obsolete and cannot be mocked";
	internal const string Title = "Cannot Mock Obsolete Types";
}