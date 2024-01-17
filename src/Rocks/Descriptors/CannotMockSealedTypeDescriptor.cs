using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;

namespace Rocks.Descriptors;

internal static class CannotMockSealedTypeDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(CannotMockSealedTypeDescriptor.Id, CannotMockSealedTypeDescriptor.Title,
			CannotMockSealedTypeDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				CannotMockSealedTypeDescriptor.Id, CannotMockSealedTypeDescriptor.Title));

	internal const string Id = "ROCK1";
	internal const string Message = "The type {0} is sealed and cannot be mocked";
	internal const string Title = "Cannot Mock Sealed Types";
}