using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Descriptors;

namespace Rocks.Tests;

public static class RockAnalyzerDuplicateConstructorsTests
{
	[Test]
	public static async Task AnalyzeWhenOnlyConstructorsIsObsoleteAsync()
	{
		var code =
			"""
			using Rocks;
			
			[assembly: RockCreate<AnyOf<object, object>>]
						
			#nullable enable
			
			public class AnyOf<T1, T2>
			{
				public AnyOf(T1 value) { }
			
				public AnyOf(T2 value) { }
			
				public virtual object GetValue() => new();			
			}
			""";

		var diagnostic = new DiagnosticResult(DuplicateConstructorsDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 45).WithArguments("AnyOf<object, object>");
		var closedGenericDiagnostic = new DiagnosticResult(TypeIsClosedGenericDescriptor.Id, DiagnosticSeverity.Warning)
			.WithSpan(3, 12, 3, 45);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [closedGenericDiagnostic, diagnostic]);
	}
}