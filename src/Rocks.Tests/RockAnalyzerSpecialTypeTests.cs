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

			[assembly: RockCreate<System.Enum>]
			""";

		var diagnostic = new DiagnosticResult(CannotMockSpecialTypesDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 35);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithStructAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<System.ValueType>]
			""";

		var diagnostic = new DiagnosticResult(CannotMockSpecialTypesDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 40);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithDelegateAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<System.Delegate>]
			""";

		var diagnostic = new DiagnosticResult(CannotMockSpecialTypesDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 39);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithMulticastDelegateAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<System.MulticastDelegate>]
			""";

		var diagnostic = new DiagnosticResult(CannotMockSpecialTypesDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 48);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic]);
	}
}