using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Descriptors
{
	public static class CannotSpecifyTypeWithOpenGenericParametersDescriptor
	{
		internal static Diagnostic Create(ITypeSymbol type) =>
			Diagnostic.Create(new(CannotSpecifyTypeWithOpenGenericParametersDescriptor.Id, CannotSpecifyTypeWithOpenGenericParametersDescriptor.Title,
				string.Format(CultureInfo.CurrentCulture, CannotSpecifyTypeWithOpenGenericParametersDescriptor.Message, 
					type.GetName()),
				DescriptorConstants.Usage, DiagnosticSeverity.Error, true,
				helpLinkUri: HelpUrlBuilder.Build(
					CannotSpecifyTypeWithOpenGenericParametersDescriptor.Id, CannotSpecifyTypeWithOpenGenericParametersDescriptor.Title)),
				type.Locations.Length > 0 ? type.Locations[0] : null);

		public const string Id = "ROCK5";
		public const string Message = "The type {0} has an open generic parameter and cannot be mocked";
		public const string Title = "Cannot Specify Type With Open Generic Parameters";
	}
}