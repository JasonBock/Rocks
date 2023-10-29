using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Globalization;

namespace Rocks.Diagnostics;

internal static class MemberIsObsoleteDiagnostic
{
	internal static Diagnostic Create(InvocationExpressionSyntax invocation, ISymbol symbol) =>
		Diagnostic.Create(new(MemberIsObsoleteDiagnostic.Id, MemberIsObsoleteDiagnostic.Title,
			string.Format(CultureInfo.CurrentCulture, MemberIsObsoleteDiagnostic.Message,
				symbol.Name),
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				MemberIsObsoleteDiagnostic.Id, MemberIsObsoleteDiagnostic.Title)),
			invocation.GetLocation());

	internal const string Id = "ROCK10";
	internal const string Message = "The member {0} is obsolete";
	internal const string Title = "Member Is Obsolete";
}