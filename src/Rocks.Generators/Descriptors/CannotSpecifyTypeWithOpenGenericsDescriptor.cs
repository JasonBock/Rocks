using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Descriptors
{
	public static class CannotSpecifyTypeWithOpenGenericsDescriptor
	{
		internal static Diagnostic Create(ITypeSymbol type) =>
			Diagnostic.Create(new(CannotSpecifyTypeWithOpenGenericsDescriptor.Id, CannotSpecifyTypeWithOpenGenericsDescriptor.Title,
				string.Format(CultureInfo.CurrentCulture, CannotSpecifyTypeWithOpenGenericsDescriptor.Message, 
					type.GetName()),
				DescriptorConstants.Usage, DiagnosticSeverity.Error, true,
				helpLinkUri: HelpUrlBuilder.Build(
					CannotSpecifyTypeWithOpenGenericsDescriptor.Id, CannotSpecifyTypeWithOpenGenericsDescriptor.Title)), type.Locations[0]);

		public const string Id = "ROCK5";
		public const string Message = "The type {0} has an open generic value and cannot be mocked";
		public const string Title = "Cannot Mock Open Generic Types";
	}
}