using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Rocks.Descriptors;

namespace Rocks.Tests;

public static class RockAnalyzerStaticAbstractMemberTests
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

		var staticAbstractMembersDiagnostic = new DiagnosticResult(InterfaceHasStaticAbstractMembersDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 86).WithArguments("IHaveStaticAbstractMethod");
		var noMembersDiagnostic = new DiagnosticResult(TypeHasNoMockableMembersDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 86).WithArguments("IHaveStaticAbstractMethod");
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(
			code, 
			[staticAbstractMembersDiagnostic, staticAbstractMembersDiagnostic, noMembersDiagnostic]);
	}
}