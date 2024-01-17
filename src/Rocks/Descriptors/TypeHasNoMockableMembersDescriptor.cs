using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;

namespace Rocks.Descriptors;

internal static class TypeHasNoMockableMembersDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(TypeHasNoMockableMembersDescriptor.Id, TypeHasNoMockableMembersDescriptor.Title,
			TypeHasNoMockableMembersDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				TypeHasNoMockableMembersDescriptor.Id, TypeHasNoMockableMembersDescriptor.Title));

	internal const string Id = "ROCK3";
	internal const string Message = "The type {0} has no members that can be overriden";
	internal const string Title = "Type Has No Mockable Members";
}