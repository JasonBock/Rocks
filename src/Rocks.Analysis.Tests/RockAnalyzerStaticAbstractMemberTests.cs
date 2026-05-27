using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Analysis.Descriptors;

namespace Rocks.Analysis.Tests;

internal static class RockAnalyzerStaticAbstractMemberTests
{
	[Test]
	public static async Task AnalyzeWhenMockIsCreatedWithSealedTypeAsync()
	{
		var code =
			"""
			using Rocks;

			[assembly: Rock(typeof(IHaveStaticAbstractMethod), BuildType.Create | BuildType.Make)]

			public interface IHaveStaticAbstractMethod 
			{ 
				static abstract void Foo();
			}
			""";

		var staticAbstractMembersDiagnostic = new DiagnosticResult(DescriptorIdentifiers.InterfaceHasStaticAbstractMembersId, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 86).WithArguments("IHaveStaticAbstractMethod");
		var noMembersDiagnostic = new DiagnosticResult(DescriptorIdentifiers.TypeHasNoMockableMembersId, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 86).WithArguments("IHaveStaticAbstractMethod");
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(
			code, 
			[staticAbstractMembersDiagnostic, staticAbstractMembersDiagnostic, noMembersDiagnostic]);
	}
}