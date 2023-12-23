using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Diagnostics;

internal static class DuplicateConstructorsDiagnostic
{
	internal static Diagnostic Create(SyntaxNode node, ITypeSymbol type) =>
		Diagnostic.Create(new(DuplicateConstructorsDiagnostic.Id, DuplicateConstructorsDiagnostic.Title,
			string.Format(CultureInfo.CurrentCulture, DuplicateConstructorsDiagnostic.Message,
				type.GetName()),
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				DuplicateConstructorsDiagnostic.Id, DuplicateConstructorsDiagnostic.Title)),
			node.GetLocation());

	internal const string Id = "ROCK12";
	internal const string Message = "The type {0} will have duplicate constructors generated in the mock";
	internal const string Title = "Duplicate Constructors";
}