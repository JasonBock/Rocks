using Microsoft.CodeAnalysis;
using System.Globalization;

namespace Rocks.Descriptors
{
	public static class TypeHasNoMockableMembersDescriptor
	{
		internal static Diagnostic Create(ITypeSymbol type) =>
			Diagnostic.Create(new DiagnosticDescriptor(
				TypeHasNoMockableMembersDescriptor.Id, TypeHasNoMockableMembersDescriptor.Title,
				string.Format(CultureInfo.CurrentCulture, TypeHasNoMockableMembersDescriptor.Message, type.Name),
				DescriptorConstants.Usage, DiagnosticSeverity.Info, true,
				helpLinkUri: HelpUrlBuilder.Build(
					TypeHasNoMockableMembersDescriptor.Id, TypeHasNoMockableMembersDescriptor.Title)), type.Locations[0]);

		public const string Id = "ROCK0003";
		public const string Message = "The type {0} has no members that can be overriden.";
		public const string Title = "Type Has No Mockable Members";
	}
}