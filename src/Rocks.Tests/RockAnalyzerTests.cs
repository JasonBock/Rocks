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
	public static async Task AnalyzeWhenTypeIsSealedAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<SealedService>]

			public sealed class SealedService
			{ 
				public void Serve() { }
			}
			""";

		var diagnostic = new DiagnosticResult(CannotMockSealedTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 37);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenTypeIsSealedUsingRockAttributeAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(SealedService), BuildType.Create)]

			public sealed class SealedService
			{ 
				public void Serve() { }
			}
			""";

		var diagnostic = new DiagnosticResult(CannotMockSealedTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 57);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic]);
	}
}