using Microsoft.CodeAnalysis;
using System.ComponentModel;

namespace Rocks.Analysis.Tests.Extensions;

internal static class DiagnosticSeverityExtensions
{
	public static ReportDiagnostic ToReportDiagnostic(this DiagnosticSeverity severity) =>
		severity switch
		{
			DiagnosticSeverity.Hidden => ReportDiagnostic.Hidden,
			DiagnosticSeverity.Info => ReportDiagnostic.Info,
			DiagnosticSeverity.Warning => ReportDiagnostic.Warn,
			DiagnosticSeverity.Error => ReportDiagnostic.Error,
			_ => throw new InvalidEnumArgumentException(nameof(severity), (int)severity, typeof(DiagnosticSeverity)),
		};
}