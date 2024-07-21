using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;

namespace Rocks.Descriptors;

internal static class TypeIsClosedGenericDescriptor
{
	// TODO: In 9.0.0, this should change
	// to an Error level.
	internal static DiagnosticDescriptor Create() =>
		new(TypeIsClosedGenericDescriptor.Id, TypeIsClosedGenericDescriptor.Title,
			TypeIsClosedGenericDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Warning, true,
			helpLinkUri: HelpUrlBuilder.Build(
				TypeIsClosedGenericDescriptor.Id, TypeIsClosedGenericDescriptor.Title));

	internal const string Id = "ROCK14";
	internal const string Message = "The type {0} is a closed generic";
	internal const string Title = "Type Is a Closed Generic";
}