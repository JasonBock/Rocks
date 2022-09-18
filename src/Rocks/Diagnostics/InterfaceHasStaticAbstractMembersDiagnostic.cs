using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Diagnostics;

public static class InterfaceHasStaticAbstractMembersDiagnostic
{
	internal static Diagnostic Create(ITypeSymbol type) =>
		Diagnostic.Create(new(InterfaceHasStaticAbstractMembersDiagnostic.Id, InterfaceHasStaticAbstractMembersDiagnostic.Title,
			string.Format(CultureInfo.CurrentCulture, InterfaceHasStaticAbstractMembersDiagnostic.Message,
				type.GetName()),
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				InterfaceHasStaticAbstractMembersDiagnostic.Id, InterfaceHasStaticAbstractMembersDiagnostic.Title)),
			type.Locations.Length > 0 ? type.Locations[0] : null);

	public const string Id = "ROCK7";
	public const string Message = "The type {0} has static abstract members and cannot be mocked";
	public const string Title = "Interface Has Static Abstract Members";
}