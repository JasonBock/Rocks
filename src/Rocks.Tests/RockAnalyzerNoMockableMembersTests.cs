using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Descriptors;

namespace Rocks.Tests;

public static class RockAnalyzerNoMockableMembersTests
{
	[Test]
	public static async Task AnalyzeWhenClassNoMockableMembersAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<NoMockables>]

			public class NoMockables
			{
				public override sealed bool Equals(object? obj) => base.Equals(obj);
				public override sealed int GetHashCode() => base.GetHashCode();
				public override sealed string? ToString() => base.ToString();
			}
			""";

		var diagnostic = new DiagnosticResult(TypeHasNoMockableMembersDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 35);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenClassNoMockableMembersAndBuildTypeIsMakeAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockMake<NoMockables>]

			public class NoMockables
			{
				public override sealed bool Equals(object? obj) => base.Equals(obj);
				public override sealed int GetHashCode() => base.GetHashCode();
				public override sealed string? ToString() => base.ToString();
			}
			""";

		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, []);
	}

	[Test]
	public static async Task AnalyzeWhenInterfaceNoMockableMembersAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockCreate<NoMockables>]

			public interface NoMockables { }
			""";

		var diagnostic = new DiagnosticResult(TypeHasNoMockableMembersDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 35);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenInterfaceNoMockableMembersAndBuildTypeIsMakeAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: RockMake<NoMockables>]

			public interface NoMockables { }
			""";

		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, []);
	}
}