using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;
using Rocks.Analysis.Descriptors;

namespace Rocks.Analysis.Tests;

internal static class RockAnalyzerInterfaceAllowsRefStructRefReturnTests
{
	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithInterfaceWithAllowsRefStructAndRefReturnAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(IHaveRefStructRefReturn<>), BuildType.Create | BuildType.Make)]

			public interface IHaveRefStructRefReturn<T>
				where T : allows ref struct
			{ 
				ref T Foo();
			}
			""";

		var refStructRefReturnDiagnostic = new DiagnosticResult(DescriptorIdentifiers.InterfaceAllowsRefStructAsRefOrRefReadonlyReturnId, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 86).WithArguments("IHaveRefStructRefReturn<T>");
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(
			code,
			[refStructRefReturnDiagnostic, refStructRefReturnDiagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithInterfaceWithAllowsRefStructAndRefReadonlyReturnAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(IHaveRefStructRefReturn<>), BuildType.Create | BuildType.Make)]

			public interface IHaveRefStructRefReturn<T>
				where T : allows ref struct
			{ 
				ref readonly T Foo();
			}
			""";

		var refStructRefReturnDiagnostic = new DiagnosticResult(DescriptorIdentifiers.InterfaceAllowsRefStructAsRefOrRefReadonlyReturnId, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 86).WithArguments("IHaveRefStructRefReturn<T>");
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(
			code,
			[refStructRefReturnDiagnostic, refStructRefReturnDiagnostic]);
	}
}