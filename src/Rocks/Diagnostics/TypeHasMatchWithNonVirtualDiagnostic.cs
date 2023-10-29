﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocks.Extensions;
using System.Globalization;

namespace Rocks.Diagnostics;

internal static class TypeHasMatchWithNonVirtualDiagnostic
{
	internal static Diagnostic Create(InvocationExpressionSyntax invocation, ITypeSymbol type) =>
		Diagnostic.Create(new(TypeHasMatchWithNonVirtualDiagnostic.Id, TypeHasMatchWithNonVirtualDiagnostic.Title,
			string.Format(CultureInfo.CurrentCulture, TypeHasMatchWithNonVirtualDiagnostic.Message,
				type.GetName()),
			DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
			helpLinkUri: HelpUrlBuilder.Build(
				TypeHasMatchWithNonVirtualDiagnostic.Id, TypeHasMatchWithNonVirtualDiagnostic.Title)),
			invocation.GetLocation());

	internal const string Id = "ROCK11";
	internal const string Message = "The type {0} has a mockable member that matches a non-virtual member";
	internal const string Title = "Type Has Match With Non Virtual";
}