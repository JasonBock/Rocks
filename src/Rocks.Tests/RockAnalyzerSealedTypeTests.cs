using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Descriptors;

namespace Rocks.Tests;

public static class RockAnalyzerSealedTypeTests
{
	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithClassAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<Data>]

			public sealed class Data { }
			""";

		var diagnostic = new DiagnosticResult(CannotMockSealedTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 28);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithEnumAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<Data>]

			public enum Data { }
			""";

		var diagnostic = new DiagnosticResult(CannotMockSealedTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 28);
		var compilerDiagnostic = DiagnosticResult.CompilerError("CS0452").WithSpan(3, 23, 3, 27);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, compilerDiagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithStructAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<StructType>]

			public struct StructType { }
			""";

		var diagnostic = new DiagnosticResult(CannotMockSealedTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 34);
		var compilerDiagnostic = DiagnosticResult.CompilerError("CS0452").WithSpan(3, 23, 3, 33);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, compilerDiagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithMulticastDelegateAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<DelegateType>]

			public delegate void DelegateType();
			""";

		var diagnostic = new DiagnosticResult(CannotMockSealedTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 36);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic]);
	}
}