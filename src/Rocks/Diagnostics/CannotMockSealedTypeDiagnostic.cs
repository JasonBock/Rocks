using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Diagnostics;

internal static class CannotMockSealedTypeDiagnostic
{
	internal static Diagnostic Create(SyntaxNode node, ITypeSymbol type) =>
		Diagnostic.Create(new(CannotMockSealedTypeDiagnostic.Id, CannotMockSealedTypeDiagnostic.Title,
			string.Format(CultureInfo.CurrentCulture, CannotMockSealedTypeDiagnostic.Message,
				type.GetName()),
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				CannotMockSealedTypeDiagnostic.Id, CannotMockSealedTypeDiagnostic.Title)),
			node.GetLocation());

	internal const string Id = "ROCK1";
	internal const string Message = "The type {0} is sealed and cannot be mocked";
	internal const string Title = "Cannot Mock Sealed Types";
}