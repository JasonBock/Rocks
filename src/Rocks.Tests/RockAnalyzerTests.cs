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

			[assembly: Rock(typeof(IService), BuildType.Create | BuildType.Make)]

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

			[assembly: Rock(typeof(SealedService), BuildType.Create | BuildType.Make)]

			public sealed class SealedService
			{ 
				public void Serve() { }
			}
			""";

		var diagnostic = new DiagnosticResult(CannotMockSealedTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 74);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, diagnostic]);
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