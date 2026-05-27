using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;

namespace Rocks.Analysis.Descriptors;

internal static class InterfaceHasStaticAbstractMembersDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(DescriptorIdentifiers.InterfaceHasStaticAbstractMembersId, InterfaceHasStaticAbstractMembersDescriptor.Title,
			InterfaceHasStaticAbstractMembersDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				DescriptorIdentifiers.InterfaceHasStaticAbstractMembersId, InterfaceHasStaticAbstractMembersDescriptor.Title));

	internal const string Message = "The type {0} has static abstract members and cannot be mocked";
	internal const string Title = "Interface Has Static Abstract Members";
}