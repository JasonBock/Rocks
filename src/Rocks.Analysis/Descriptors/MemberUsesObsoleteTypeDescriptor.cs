using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;

namespace Rocks.Analysis.Descriptors;

internal static class MemberUsesObsoleteTypeDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(DescriptorIdentifiers.MemberUsesObsoleteTypeId, MemberUsesObsoleteTypeDescriptor.Title,
			MemberUsesObsoleteTypeDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				DescriptorIdentifiers.MemberUsesObsoleteTypeId, MemberUsesObsoleteTypeDescriptor.Title));

	internal const string Message = "The member {0} uses an obsolete type";
	internal const string Title = "Member Uses Obsolete Type";
}