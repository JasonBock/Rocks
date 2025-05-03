using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Descriptors;

namespace Rocks.Analysis.Tests;

public static class RockAnalyzerSealedTypeTests
{
	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithClassAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(Data), BuildType.Create | BuildType.Make)]

			public sealed class Data { }
			""";

		var diagnostic = new DiagnosticResult(CannotMockSealedTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 65);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithEnumAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(Data), BuildType.Create | BuildType.Make)]

			public enum Data { }
			""";

		var diagnostic = new DiagnosticResult(CannotMockSealedTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 65);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithStructAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(StructType), BuildType.Create | BuildType.Make)]

			public struct StructType { }
			""";

		var diagnostic = new DiagnosticResult(CannotMockSealedTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 71);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithMulticastDelegateAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(DelegateType), BuildType.Create | BuildType.Make)]

			public delegate void DelegateType();
			""";

		var diagnostic = new DiagnosticResult(CannotMockSealedTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 73);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic]);
	}
}