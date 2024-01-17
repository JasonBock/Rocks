using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;

namespace Rocks.Descriptors;

internal static class InterfaceHasStaticAbstractMembersDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(InterfaceHasStaticAbstractMembersDescriptor.Id, InterfaceHasStaticAbstractMembersDescriptor.Title,
			InterfaceHasStaticAbstractMembersDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				InterfaceHasStaticAbstractMembersDescriptor.Id, InterfaceHasStaticAbstractMembersDescriptor.Title));

	internal const string Id = "ROCK7";
	internal const string Message = "The type {0} has static abstract members and cannot be mocked";
	internal const string Title = "Interface Has Static Abstract Members";
}