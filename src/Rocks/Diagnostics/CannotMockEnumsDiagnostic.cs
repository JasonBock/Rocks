using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Diagnostics;

public static class CannotMockEnumsDiagnostic
{
	internal static Diagnostic Create(ITypeSymbol type) =>
		Diagnostic.Create(new(CannotMockEnumsDiagnostic.Id, CannotMockEnumsDiagnostic.Title,
			string.Format(CultureInfo.CurrentCulture, CannotMockEnumsDiagnostic.Message,
				type.GetName()),
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				CannotMockEnumsDiagnostic.Id, CannotMockEnumsDiagnostic.Title)),
			type.Locations.Length > 0 ? type.Locations[0] : null);

	public const string Id = "ROCK7";
	public const string Message = "The type {0} is an enum and cannot be mocked";
	public const string Title = "Cannot Mock Enums";
}