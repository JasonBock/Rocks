using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;

namespace Rocks.Analysis.Descriptors;

internal static class TypeHasNoMockableMembersDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(DescriptorIdentifiers.TypeHasNoMockableMembersId, TypeHasNoMockableMembersDescriptor.Title,
			TypeHasNoMockableMembersDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				DescriptorIdentifiers.TypeHasNoMockableMembersId, TypeHasNoMockableMembersDescriptor.Title));

	internal const string Message = "The type {0} has no members that can be overriden";
	internal const string Title = "Type Has No Mockable Members";
}