using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Diagnostics;

public static class CannotMockDelegatesDiagnostic
{
	internal static Diagnostic Create(ITypeSymbol type) =>
		Diagnostic.Create(new(CannotMockDelegatesDiagnostic.Id, CannotMockDelegatesDiagnostic.Title,
			string.Format(CultureInfo.CurrentCulture, CannotMockDelegatesDiagnostic.Message,
				type.GetName()),
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				CannotMockDelegatesDiagnostic.Id, CannotMockDelegatesDiagnostic.Title)),
			type.Locations.Length > 0 ? type.Locations[0] : null);

	public const string Id = "ROCK6";
	public const string Message = "The type {0} is a delegate and cannot be mocked";
	public const string Title = "Cannot Mock Delegates";
}