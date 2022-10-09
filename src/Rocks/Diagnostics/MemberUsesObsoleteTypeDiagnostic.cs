using Microsoft.CodeAnalysis;
using System.Globalization;

namespace Rocks.Diagnostics;

public static class MemberUsesObsoleteTypeDiagnostic
{
	internal static Diagnostic Create(ISymbol symbol) =>
		Diagnostic.Create(new(MemberUsesObsoleteTypeDiagnostic.Id, MemberUsesObsoleteTypeDiagnostic.Title,
			string.Format(CultureInfo.CurrentCulture, MemberUsesObsoleteTypeDiagnostic.Message,
				symbol.Name),
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				MemberUsesObsoleteTypeDiagnostic.Id, MemberUsesObsoleteTypeDiagnostic.Title)),
			symbol.Locations.Length > 0 ? symbol.Locations[0] : null);

	public const string Id = "ROCK9";
	public const string Message = "The member {0} uses an obsolete type";
	public const string Title = "Member Uses Obsolete Type";
}