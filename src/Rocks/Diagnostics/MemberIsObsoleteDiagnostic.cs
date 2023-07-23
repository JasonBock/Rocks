using Microsoft.CodeAnalysis;
using System.Globalization;

namespace Rocks.Diagnostics;

internal static class MemberIsObsoleteDiagnostic
{
	internal static Diagnostic Create(ISymbol symbol) =>
		Diagnostic.Create(new(MemberIsObsoleteDiagnostic.Id, MemberIsObsoleteDiagnostic.Title,
			string.Format(CultureInfo.CurrentCulture, MemberIsObsoleteDiagnostic.Message,
				symbol.Name),
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				MemberIsObsoleteDiagnostic.Id, MemberIsObsoleteDiagnostic.Title)),
			symbol.Locations.Length > 0 ? symbol.Locations[0] : null);

	internal const string Id = "ROCK10";
	internal const string Message = "The member {0} is obsolete";
	internal const string Title = "Member Is Obsolete";
}