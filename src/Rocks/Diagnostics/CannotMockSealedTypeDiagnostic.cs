using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Diagnostics;

internal static class CannotMockSealedTypeDiagnostic
{
	internal static Diagnostic Create(ITypeSymbol type) =>
		Diagnostic.Create(new(CannotMockSealedTypeDiagnostic.Id, CannotMockSealedTypeDiagnostic.Title,
			string.Format(CultureInfo.CurrentCulture, CannotMockSealedTypeDiagnostic.Message,
				type.GetName()),
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				CannotMockSealedTypeDiagnostic.Id, CannotMockSealedTypeDiagnostic.Title)),
			type.Locations.Length > 0 ? type.Locations[0] : null);

	internal const string Id = "ROCK1";
	internal const string Message = "The type {0} is sealed and cannot be mocked";
	internal const string Title = "Cannot Mock Sealed Types";
}