using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Diagnostics
{
	public static class TypeHasNoAccessibleConstructorsDiagnostic
	{
		internal static Diagnostic Create(ITypeSymbol type) =>
			Diagnostic.Create(new(TypeHasNoAccessibleConstructorsDiagnostic.Id, TypeHasNoAccessibleConstructorsDiagnostic.Title,
				string.Format(CultureInfo.CurrentCulture, TypeHasNoAccessibleConstructorsDiagnostic.Message, 
					type.GetName()),
				DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
				helpLinkUri: HelpUrlBuilder.Build(
					TypeHasNoAccessibleConstructorsDiagnostic.Id, TypeHasNoAccessibleConstructorsDiagnostic.Title)),
				type.Locations.Length > 0 ? type.Locations[0] : null);

		public const string Id = "ROCK4";
		public const string Message = "The type {0} has no constructors that are accessible";
		public const string Title = "Type Has No Accessible Constructors";
	}
}