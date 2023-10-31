using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Diagnostics;

internal static class NoConstructorsDiagnostic
{
	internal static Diagnostic Create(InvocationExpressionSyntax invocation, ITypeSymbol type) =>
		Diagnostic.Create(new(NoConstructorsDiagnostic.Id, NoConstructorsDiagnostic.Title,
			string.Format(CultureInfo.CurrentCulture, NoConstructorsDiagnostic.Message,
				type.GetName()),
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				NoConstructorsDiagnostic.Id, NoConstructorsDiagnostic.Title)),
			invocation.GetLocation());

	internal const string Id = "ROCK13";
	internal const string Message = "The type {0} has no accessible constructors.";
	internal const string Title = "No Constructors";
}