using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Diagnostics;

public static class CannotMockSpecialTypesDiagnostic
{
	internal static Diagnostic Create(ITypeSymbol type) =>
		Diagnostic.Create(new(CannotMockSpecialTypesDiagnostic.Id, CannotMockSpecialTypesDiagnostic.Title,
			string.Format(CultureInfo.CurrentCulture, CannotMockSpecialTypesDiagnostic.Message,
				type.GetName()),
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				CannotMockSpecialTypesDiagnostic.Id, CannotMockSpecialTypesDiagnostic.Title)),
			type.Locations.Length > 0 ? type.Locations[0] : null);

	public const string Id = "ROCK6";
	public const string Message = "The type {0} is a special type and cannot be mocked";
	public const string Title = "Cannot Mock Special Types";
}