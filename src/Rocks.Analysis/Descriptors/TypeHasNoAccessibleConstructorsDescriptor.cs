using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;

namespace Rocks.Analysis.Descriptors;

internal static class TypeHasNoAccessibleConstructorsDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(DescriptorIdentifiers.TypeHasNoAccessibleConstructorsId, TypeHasNoAccessibleConstructorsDescriptor.Title,
			TypeHasNoAccessibleConstructorsDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				DescriptorIdentifiers.TypeHasNoAccessibleConstructorsId, TypeHasNoAccessibleConstructorsDescriptor.Title));

	internal const string Message = "The type {0} has no constructors that are accessible";
	internal const string Title = "Type Has No Accessible Constructors";
}