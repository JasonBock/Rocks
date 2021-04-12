using Microsoft.CodeAnalysis;
using System;

namespace Rocks.Diagnostics
{
	public static class UnexpectedExceptionDiagnostic
	{
		internal static Diagnostic Create(Exception e) =>
			Diagnostic.Create(new(UnexpectedExceptionDiagnostic.Id, UnexpectedExceptionDiagnostic.Title,
				e.ToString().Replace(Environment.NewLine, " : "),
				DiagnosticConstants.Usage, DiagnosticSeverity.Error, true,
				helpLinkUri: HelpUrlBuilder.Build(
					UnexpectedExceptionDiagnostic.Id, UnexpectedExceptionDiagnostic.Title)), null);

		public const string Id = "ROCK6";
		public const string Message = "An unexpected exception has occurred";
		public const string Title = "Unexpected Exception";
	}
}