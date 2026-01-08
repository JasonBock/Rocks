using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;

namespace Rocks.Analysis.Descriptors;

internal static class TypeHasInaccessibleAbstractMembersDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(Id, Title,
			Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				Id, Title));

	internal const string Id = "ROCK8";
	internal const string Message = "The type {0} has inaccessible abstract members and cannot be mocked";
	internal const string Title = "Type Has Inaccessible Abstract Members";
}