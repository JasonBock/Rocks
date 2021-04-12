using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Diagnostics
{
	public static class CannotSpecifyTypeWithOpenGenericParametersDiagnostic
	{
		internal static Diagnostic Create(ITypeSymbol type) =>
			Diagnostic.Create(new(CannotSpecifyTypeWithOpenGenericParametersDiagnostic.Id, CannotSpecifyTypeWithOpenGenericParametersDiagnostic.Title,
				string.Format(CultureInfo.CurrentCulture, CannotSpecifyTypeWithOpenGenericParametersDiagnostic.Message, 
					type.GetName()),
				DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
				helpLinkUri: HelpUrlBuilder.Build(
					CannotSpecifyTypeWithOpenGenericParametersDiagnostic.Id, CannotSpecifyTypeWithOpenGenericParametersDiagnostic.Title)),
				type.Locations.Length > 0 ? type.Locations[0] : null);

		public const string Id = "ROCK5";
		public const string Message = "The type {0} has an open generic parameter and cannot be mocked";
		public const string Title = "Cannot Specify Type With Open Generic Parameters";
	}
}