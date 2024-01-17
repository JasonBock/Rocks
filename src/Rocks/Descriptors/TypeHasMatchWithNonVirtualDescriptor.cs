using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;

namespace Rocks.Descriptors;

internal static class TypeHasMatchWithNonVirtualDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(TypeHasMatchWithNonVirtualDescriptor.Id, TypeHasMatchWithNonVirtualDescriptor.Title,
			TypeHasMatchWithNonVirtualDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				TypeHasMatchWithNonVirtualDescriptor.Id, TypeHasMatchWithNonVirtualDescriptor.Title));

	internal const string Id = "ROCK11";
	internal const string Message = "The type {0} has a mockable member that matches a non-virtual member";
	internal const string Title = "Type Has Match With Non Virtual";
}