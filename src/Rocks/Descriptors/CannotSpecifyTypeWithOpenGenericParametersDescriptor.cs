using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;

namespace Rocks.Descriptors;

internal static class CannotSpecifyTypeWithOpenGenericParametersDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(CannotSpecifyTypeWithOpenGenericParametersDescriptor.Id, CannotSpecifyTypeWithOpenGenericParametersDescriptor.Title,
			CannotSpecifyTypeWithOpenGenericParametersDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				CannotSpecifyTypeWithOpenGenericParametersDescriptor.Id, CannotSpecifyTypeWithOpenGenericParametersDescriptor.Title));

	internal const string Id = "ROCK5";
	internal const string Message = "The type {0} has an open generic parameter and cannot be mocked";
	internal const string Title = "Cannot Specify Type With Open Generic Parameters";
}