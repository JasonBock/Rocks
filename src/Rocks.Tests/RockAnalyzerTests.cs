using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Descriptors;

namespace Rocks.Tests;

public static class RockAnalyzerTests
{
	[Test]
	public static async Task AnalyzeWhenDiagnosticsAreNotCreatedAsync()
	{
		var code =
			"""
			public sealed class Usage { }
			""";
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, []);
	}

	[Test]
	public static async Task AnalyzeWhenMockIsCreatedAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<IService>]

			public interface IService
			{ 
				void Serve();
			}
			""";
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, []);
	}

	[Test]
	public static async Task AnalyzeWhenTypeIsErrorAsync()
	{
		var code =
			"""
			using System;
			using Rocks;

			[assembly: RockCreate<IBase<,>>]

			public interface IBase<T1, T2> 
			{
				void Foo(T1 a, T2 b);
				void MockThis();
			}
			""";

		var diagnostic = new DiagnosticResult(TypeErrorDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(4, 12, 4, 32).WithArguments("IBase<T1, T2>");
		var compilerDiagnostic = DiagnosticResult.CompilerError("CS7003").WithSpan(4, 23, 4, 31);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, compilerDiagnostic]);
	}
}