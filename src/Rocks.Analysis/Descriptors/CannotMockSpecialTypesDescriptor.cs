using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;

namespace Rocks.Analysis.Descriptors;

internal static class CannotMockSpecialTypesDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(DescriptorIdentifiers.CannotMockSpecialTypesId, CannotMockSpecialTypesDescriptor.Title,
			CannotMockSpecialTypesDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				DescriptorIdentifiers.CannotMockSpecialTypesId, CannotMockSpecialTypesDescriptor.Title));

	internal const string Message = "The type {0} is a special type and cannot be mocked";
	internal const string Title = "Cannot Mock Special Types";
}