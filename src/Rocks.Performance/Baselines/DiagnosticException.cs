using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Rocks.Performance.Baselines;

internal sealed class DiagnosticException 
	: Exception
{
	public DiagnosticException()
		: base() { }

   public DiagnosticException(ImmutableArray<Diagnostic> diagnostics)
		: base() => this.Diagnostics = diagnostics;

	public DiagnosticException(string? message)
		: base(message) { }

	public DiagnosticException(string? message, Exception? innerException)
		: base(message, innerException) { }

	public ImmutableArray<Diagnostic> Diagnostics { get; private init; }
}