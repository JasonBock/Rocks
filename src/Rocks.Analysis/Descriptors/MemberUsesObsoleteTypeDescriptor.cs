using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;

namespace Rocks.Analysis.Descriptors;

internal static class MemberUsesObsoleteTypeDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(Id, Title,
			Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				Id, Title));

	internal const string Id = "ROCK9";
	internal const string Message = "The member {0} uses an obsolete type";
	internal const string Title = "Member Uses Obsolete Type";
}