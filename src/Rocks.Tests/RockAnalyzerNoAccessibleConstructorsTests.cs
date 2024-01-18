using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Descriptors;

namespace Rocks.Tests;

public static class RockAnalyzerNoAccessibleConstructorsTests
{
	[Test]
	public static async Task AnalyzeWhenOnlyConstructorsIsObsoleteAsync()
	{
		var code =
			"""
			using System; 
			using Rocks; 
			
			[assembly: RockCreate<Constructable>] 
			
			public class Constructable
			{
				[Obsolete("Old", true)]
				public Constructable() { }
			
				public virtual object GetValue() => new();			
			}
			""";

		var diagnostic = new DiagnosticResult(TypeHasNoAccessibleConstructorsDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(4, 12, 4, 37).WithArguments("Constructable");
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenClassNoMockableMembersAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<NoConstructors>]

			public class NoConstructors
			{
				private NoConstructors() { }
			}
			""";

		var diagnostic = new DiagnosticResult(TypeHasNoAccessibleConstructorsDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 38);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic]);
	}
}