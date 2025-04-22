using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Descriptors;

namespace Rocks.Analysis.Tests;

public static class RockAnalyzerNoMockableMembersTests
{
	[Test]
	public static async Task AnalyzeWhenClassNoMockableMembersAsync()
	{
		var code =
			"""
			using Rocks.Runtime;

			[assembly: Rock(typeof(NoMockables), BuildType.Create | BuildType.Make)]

			public class NoMockables
			{
				public override sealed bool Equals(object? obj) => base.Equals(obj);
				public override sealed int GetHashCode() => base.GetHashCode();
				public override sealed string? ToString() => base.ToString();
			}
			""";

		var diagnostic = new DiagnosticResult(TypeHasNoMockableMembersDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 72);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic]);
	}

	[Test]
	public static async Task AnalyzeWhenInterfaceNoMockableMembersAsync()
	{
		var code =
			"""
			using Rocks.Runtime;

			[assembly: Rock(typeof(NoMockables), BuildType.Create | BuildType.Make)]

			public interface NoMockables { }
			""";

		var diagnostic = new DiagnosticResult(TypeHasNoMockableMembersDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 72);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic]);
	}
}