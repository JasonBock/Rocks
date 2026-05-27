using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;

namespace Rocks.Analysis.Descriptors;

internal static class TypeIsClosedGenericDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(DescriptorIdentifiers.TypeIsClosedGenericId, TypeIsClosedGenericDescriptor.Title,
			TypeIsClosedGenericDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				DescriptorIdentifiers.TypeIsClosedGenericId, TypeIsClosedGenericDescriptor.Title));

	internal const string Message = "The type {0} is a closed generic";
	internal const string Title = "Type Is a Closed Generic";
}