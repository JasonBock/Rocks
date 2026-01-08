using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;

namespace Rocks.Analysis.Descriptors;

internal static class TypeHasNoMockableMembersDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(Id, Title,
			Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				Id, Title));

	internal const string Id = "ROCK3";
	internal const string Message = "The type {0} has no members that can be overriden";
	internal const string Title = "Type Has No Mockable Members";
}