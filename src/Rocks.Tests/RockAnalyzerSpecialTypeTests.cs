using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Descriptors;

namespace Rocks.Tests;

public static class RockAnalyzerSpecialTypeTests
{
	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithEnumTypeAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(System.Enum), BuildType.Create | BuildType.Make)]
			""";

		var diagnostic = new DiagnosticResult(CannotMockSpecialTypesDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 72);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithStructAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(System.ValueType), BuildType.Create | BuildType.Make)]
			""";

		var diagnostic = new DiagnosticResult(CannotMockSpecialTypesDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 77);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithDelegateAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(System.Delegate), BuildType.Create | BuildType.Make)]
			""";

		var diagnostic = new DiagnosticResult(CannotMockSpecialTypesDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 76);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithMulticastDelegateAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(System.MulticastDelegate), BuildType.Create | BuildType.Make)]
			""";

		var diagnostic = new DiagnosticResult(CannotMockSpecialTypesDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 85);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic]);
	}
}