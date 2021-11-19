using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Diagnostics;

public static class CannotMockObsoleteTypeDiagnostic
{
	internal static Diagnostic Create(ITypeSymbol type) =>
		Diagnostic.Create(new(CannotMockObsoleteTypeDiagnostic.Id, CannotMockObsoleteTypeDiagnostic.Title,
			string.Format(CultureInfo.CurrentCulture, CannotMockObsoleteTypeDiagnostic.Message,
				type.GetName()),
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				CannotMockObsoleteTypeDiagnostic.Id, CannotMockObsoleteTypeDiagnostic.Title)),
			type.Locations.Length > 0 ? type.Locations[0] : null);

	public const string Id = "ROCK2";
	public const string Message = "The type {0} is obsolete and cannot be mocked";
	public const string Title = "Cannot Mock Obsolete Types";
}