using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Diagnostics;

internal static class CannotSpecifyTypeWithOpenGenericParametersDiagnostic
{
	internal static Diagnostic Create(InvocationExpressionSyntax invocation, ITypeSymbol type) =>
		Diagnostic.Create(new(CannotSpecifyTypeWithOpenGenericParametersDiagnostic.Id, CannotSpecifyTypeWithOpenGenericParametersDiagnostic.Title,
			string.Format(CultureInfo.CurrentCulture, CannotSpecifyTypeWithOpenGenericParametersDiagnostic.Message,
				type.GetName()),
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				CannotSpecifyTypeWithOpenGenericParametersDiagnostic.Id, CannotSpecifyTypeWithOpenGenericParametersDiagnostic.Title)),
			invocation.GetLocation());

	internal const string Id = "ROCK5";
	internal const string Message = "The type {0} has an open generic parameter and cannot be mocked";
	internal const string Title = "Cannot Specify Type With Open Generic Parameters";
}