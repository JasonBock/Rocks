using Microsoft.CodeAnalysis;
using Rocks.Diagnostics;

namespace Rocks.Descriptors;

internal static class TypeErrorDescriptor
{
	internal static DiagnosticDescriptor Create() =>
		new(TypeErrorDescriptor.Id, TypeErrorDescriptor.Title,
			TypeErrorDescriptor.Message,
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				TypeErrorDescriptor.Id, TypeErrorDescriptor.Title));

	internal const string Id = "ROCK13";
	internal const string Message = "The type {0} has type errors";
	internal const string Title = "Type Error";
}