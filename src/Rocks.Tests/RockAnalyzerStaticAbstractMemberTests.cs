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

			[assembly: RockCreate<IHaveStaticAbstractMethod>]

			public interface IHaveStaticAbstractMethod 
			{ 
				static abstract void Foo();
			}
			""";

		var diagnostic = new DiagnosticResult(InterfaceHasStaticAbstractMembersDescriptor.Id, DiagnosticSeverity.Error)
			.WithSpan(3, 12, 3, 49).WithArguments("IHaveStaticAbstractMethod");
		var compilerDiagnostic = DiagnosticResult.CompilerError("CS8920").WithSpan(3, 23, 3, 48);
		await TestAssistants.RunAnalyzerAsync<RockAnalyzer>(code, [diagnostic, compilerDiagnostic]);
	}
}