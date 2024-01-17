using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;

namespace Rocks.Descriptors;

internal static class CannotMockSpecialTypesDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(CannotMockSpecialTypesDescriptor.Id, CannotMockSpecialTypesDescriptor.Title,
			CannotMockSpecialTypesDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				CannotMockSpecialTypesDescriptor.Id, CannotMockSpecialTypesDescriptor.Title));

	internal const string Id = "ROCK6";
	internal const string Message = "The type {0} is a special type and cannot be mocked";
	internal const string Title = "Cannot Mock Special Types";
}