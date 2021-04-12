using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Diagnostics
{
	public static class CannotMockSealedTypeDiagnostic
	{
		internal static Diagnostic Create(ITypeSymbol type) =>
			Diagnostic.Create(new(CannotMockSealedTypeDiagnostic.Id, CannotMockSealedTypeDiagnostic.Title,
				string.Format(CultureInfo.CurrentCulture, CannotMockSealedTypeDiagnostic.Message, 
					type.GetName()),
				DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
				helpLinkUri: HelpUrlBuilder.Build(
					CannotMockSealedTypeDiagnostic.Id, CannotMockSealedTypeDiagnostic.Title)),
				type.Locations.Length > 0 ? type.Locations[0] : null);

		public const string Id = "ROCK1";
		public const string Message = "The type {0} is sealed and cannot be mocked";
		public const string Title = "Cannot Mock Sealed Types";
	}
}