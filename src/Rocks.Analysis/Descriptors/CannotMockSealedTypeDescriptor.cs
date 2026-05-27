using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;

namespace Rocks.Analysis.Descriptors;

internal static class CannotMockSealedTypeDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(DescriptorIdentifiers.CannotMockSealedTypeId, CannotMockSealedTypeDescriptor.Title,
			CannotMockSealedTypeDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				DescriptorIdentifiers.CannotMockSealedTypeId, CannotMockSealedTypeDescriptor.Title));

	internal const string Message = "The type {0} is sealed and cannot be mocked";
	internal const string Title = "Cannot Mock Sealed Types";
}