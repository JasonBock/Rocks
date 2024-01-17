using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;

namespace Rocks.Descriptors;

internal static class MemberUsesObsoleteTypeDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(MemberUsesObsoleteTypeDescriptor.Id, MemberUsesObsoleteTypeDescriptor.Title,
			MemberUsesObsoleteTypeDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				MemberUsesObsoleteTypeDescriptor.Id, MemberUsesObsoleteTypeDescriptor.Title));

	internal const string Id = "ROCK9";
	internal const string Message = "The member {0} uses an obsolete type";
	internal const string Title = "Member Uses Obsolete Type";
}