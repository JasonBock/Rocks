using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;

namespace Rocks.Descriptors;

internal static class TypeHasInaccessibleAbstractMembersDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(TypeHasInaccessibleAbstractMembersDescriptor.Id, TypeHasInaccessibleAbstractMembersDescriptor.Title,
			TypeHasInaccessibleAbstractMembersDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				TypeHasInaccessibleAbstractMembersDescriptor.Id, TypeHasInaccessibleAbstractMembersDescriptor.Title));

	internal const string Id = "ROCK8";
	internal const string Message = "The type {0} has inaccessible abstract members and cannot be mocked";
	internal const string Title = "Type Has Inaccessible Abstract Members";
}