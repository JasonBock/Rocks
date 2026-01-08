using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;

namespace Rocks.Analysis.Descriptors;

internal static class TypeIsClosedGenericDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(Id, Title,
			Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				Id, Title));

	internal const string Id = "ROCK14";
	internal const string Message = "The type {0} is a closed generic";
	internal const string Title = "Type Is a Closed Generic";
}