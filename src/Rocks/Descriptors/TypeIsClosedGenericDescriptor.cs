using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;

namespace Rocks.Descriptors;

internal static class TypeIsClosedGenericDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(TypeIsClosedGenericDescriptor.Id, TypeIsClosedGenericDescriptor.Title,
			TypeIsClosedGenericDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				TypeIsClosedGenericDescriptor.Id, TypeIsClosedGenericDescriptor.Title));

	internal const string Id = "ROCK14";
	internal const string Message = "The type {0} is a closed generic";
	internal const string Title = "Type Is a Closed Generic";
}