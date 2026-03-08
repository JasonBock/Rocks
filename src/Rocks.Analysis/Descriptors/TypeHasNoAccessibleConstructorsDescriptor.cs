using Microsoft.CodeAnalysis;
using Rocks.Analysis.Diagnostics;

namespace Rocks.Analysis.Descriptors;

internal static class TypeHasNoAccessibleConstructorsDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(TypeHasNoAccessibleConstructorsDescriptor.Id, TypeHasNoAccessibleConstructorsDescriptor.Title,
			TypeHasNoAccessibleConstructorsDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				TypeHasNoAccessibleConstructorsDescriptor.Id, TypeHasNoAccessibleConstructorsDescriptor.Title));

	internal const string Id = "ROCK4";
	internal const string Message = "The type {0} has no constructors that are accessible";
	internal const string Title = "Type Has No Accessible Constructors";
}