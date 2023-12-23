using Microsoft.CodeAnalysis;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Diagnostics;

internal static class TypeHasInaccessibleAbstractMembersDiagnostic
{
	internal static Diagnostic Create(SyntaxNode node, ITypeSymbol type) =>
		Diagnostic.Create(new(TypeHasInaccessibleAbstractMembersDiagnostic.Id, TypeHasInaccessibleAbstractMembersDiagnostic.Title,
			string.Format(CultureInfo.CurrentCulture, TypeHasInaccessibleAbstractMembersDiagnostic.Message,
				type.GetName()),
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				TypeHasInaccessibleAbstractMembersDiagnostic.Id, TypeHasInaccessibleAbstractMembersDiagnostic.Title)),
			node.GetLocation());

	internal const string Id = "ROCK8";
	internal const string Message = "The type {0} has inaccessible abstract members and cannot be mocked";
	internal const string Title = "Type Has Inaccessible Abstract Members";
}