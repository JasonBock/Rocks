using Microsoft.CodeAnalysis;
using System.Globalization;

namespace Rocks.Descriptors
{
	public static class CannotMockObsoleteTypeDescriptor
	{
		internal static Diagnostic Create(ITypeSymbol type) =>
			Diagnostic.Create(new DiagnosticDescriptor(
				CannotMockObsoleteTypeDescriptor.Id, CannotMockObsoleteTypeDescriptor.Title,
				string.Format(CultureInfo.CurrentCulture, CannotMockObsoleteTypeDescriptor.Message, type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)),
				DescriptorConstants.Usage, DiagnosticSeverity.Info, true,
				helpLinkUri: HelpUrlBuilder.Build(
					CannotMockObsoleteTypeDescriptor.Id, CannotMockObsoleteTypeDescriptor.Title)), type.Locations[0]);

		public const string Id = "ROCK0002";
		public const string Message = "The type {0} is obsolete and cannot be mocked.";
		public const string Title = "Cannot Mock Obsolete Types";
	}
}