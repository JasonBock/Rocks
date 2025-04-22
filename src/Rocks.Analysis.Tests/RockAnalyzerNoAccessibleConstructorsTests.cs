using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Descriptors;

namespace Rocks.Analysis.Tests;

public static class RockAnalyzerNoAccessibleConstructorsTests
{
	[Test]
	public static async Task AnalyzeWhenOnlyConstructorsIsObsoleteAsync()
	{
		var code =
			"""
			using System; 
			using Rocks.Runtime; 
			
			[assembly: Rock(typeof(Constructable), BuildType.Create | BuildType.Make)] 
			
			public class Constructable
			{
				[Obsolete("Old", true)]
				public Constructable() { }
			
				public virtual object GetValue() => new();			
			}
			""";

		var diagnostic = new DiagnosticResult(TypeHasNoAccessibleConstructorsDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(4, 12, 4, 74).WithArguments("Constructable");
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenClassNoMockableMembersAsync()
	{
		var code =
			"""
			using Rocks.Runtime;

			[assembly: Rock(typeof(NoConstructors), BuildType.Create | BuildType.Make)]

			public class NoConstructors
			{
				private NoConstructors() { }
			}
			""";

		var diagnostic = new DiagnosticResult(TypeHasNoAccessibleConstructorsDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 75);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic]);
	}
}