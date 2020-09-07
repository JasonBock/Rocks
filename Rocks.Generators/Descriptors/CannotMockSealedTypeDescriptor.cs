using Microsoft.CodeAnalysis;
using System.Globalization;

namespace Rocks.Descriptors
{
	public static class CannotMockSealedTypeDescriptor
	{
		internal static Diagnostic Create(ITypeSymbol type) =>
			Diagnostic.Create(new DiagnosticDescriptor(
				CannotMockSealedTypeDescriptor.Id, CannotMockSealedTypeDescriptor.Title,
				string.Format(CultureInfo.CurrentCulture, CannotMockSealedTypeDescriptor.Message, type.Name),
				DescriptorConstants.Usage, DiagnosticSeverity.Info, true,
				helpLinkUri: HelpUrlBuilder.Build(
					CannotMockSealedTypeDescriptor.Id, CannotMockSealedTypeDescriptor.Title)), type.Locations[0]);

		public const string Id = "ROCK0001";
		public const string Message = "The type {0} is sealed and cannot be mocked.";
		public const string Title = "Cannot Mock Sealed Types";
	}
}