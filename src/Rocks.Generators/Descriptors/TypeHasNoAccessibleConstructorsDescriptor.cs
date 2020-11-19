using Microsoft.CodeAnalysis;
using System.Globalization;

namespace Rocks.Descriptors
{
	public static class TypeHasNoAccessibleConstructorsDescriptor
	{
		internal static Diagnostic Create(ITypeSymbol type) =>
			Diagnostic.Create(new(TypeHasNoAccessibleConstructorsDescriptor.Id, TypeHasNoAccessibleConstructorsDescriptor.Title,
				string.Format(CultureInfo.CurrentCulture, TypeHasNoAccessibleConstructorsDescriptor.Message, 
					type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)),
				DescriptorConstants.Usage, DiagnosticSeverity.Error, true,
				helpLinkUri: HelpUrlBuilder.Build(
					TypeHasNoAccessibleConstructorsDescriptor.Id, TypeHasNoAccessibleConstructorsDescriptor.Title)), type.Locations[0]);

		public const string Id = "ROCK4";
		public const string Message = "The type {0} has no constructors that are accessible";
		public const string Title = "Type Has No Accessible Constructors";
	}
}