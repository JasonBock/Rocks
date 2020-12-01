using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Descriptors
{
	public static class CannotMockObsoleteTypeDescriptor
	{
		internal static Diagnostic Create(ITypeSymbol type) =>
			Diagnostic.Create(new(CannotMockObsoleteTypeDescriptor.Id, CannotMockObsoleteTypeDescriptor.Title,
				string.Format(CultureInfo.CurrentCulture, CannotMockObsoleteTypeDescriptor.Message, 
					type.GetName()),
				DescriptorConstants.Usage, DiagnosticSeverity.Error, true,
				helpLinkUri: HelpUrlBuilder.Build(
					CannotMockObsoleteTypeDescriptor.Id, CannotMockObsoleteTypeDescriptor.Title)), type.Locations[0]);

		public const string Id = "ROCK2";
		public const string Message = "The type {0} is obsolete and cannot be mocked";
		public const string Title = "Cannot Mock Obsolete Types";
	}
}