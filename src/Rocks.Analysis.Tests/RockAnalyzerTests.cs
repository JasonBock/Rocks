using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Descriptors;

namespace Rocks.Analysis.Tests;

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
			using Rocks.Runtime;

			[assembly: Rock(typeof(IService), BuildType.Create | BuildType.Make)]

			public interface IService
			{ 
				void Serve();
			}
			""";

		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, []);
	}

	[Test]
	public static async Task AnalyzePartialWhenMockIsCreatedAsync()
	{
		var code =
			"""
			using Rocks.Runtime;

			public interface IService
			{ 
				void Serve();
			}

			[RockPartial(typeof(IService), BuildType.Create)]
			public sealed partial class IServerTarget;
			""";

		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, []);
	}

	[Test]
	public static async Task AnalyzeWhenDiagnosticIsCreatedAsync()
	{
		var code =
			"""
			using Rocks.Runtime;

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
	public static async Task AnalyzePartialWhenDiagnosticIsCreatedAsync()
	{
		var code =
			"""
			using Rocks.Runtime;

			public sealed class SealedService
			{ 
				public void Serve() { }
			}

			[RockPartial(typeof(SealedService), BuildType.Create)]			
			public sealed partial class SealedServiceTarget;
			""";

		var diagnostic = new DiagnosticResult(CannotMockSealedTypeDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(8, 2, 8, 54);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic]);
	}
}